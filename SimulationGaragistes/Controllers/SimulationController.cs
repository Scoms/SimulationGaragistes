using SimlulationGaragistesService.Service;
using SimulationGaragistes.ViewModels;
using SimulationGaragistesDAL.Model;
using SimulationGaragistesDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace SimulationGaragistes.Controllers
{
    public class SimulationController : Controller
    {
        private string filePath = "Logs/";
        private int NBJOURS_DEFAULT = 100;
        //
        // GET: /Simulation/
        [HttpGet]
        public ActionResult Index()
        {
            ErrorHandler eh = new ErrorHandler();
            VMSimulation vmSimu = new VMSimulation();

            //Services
            ServiceGaragistes sGaragiste = new ServiceGaragistes(eh);
            ServiceModeles sModeles = new ServiceModeles(eh);

            //Populare VM
            List<Garagistes> lGaragistes = sGaragiste.findAll(new List<String>(){"Franchises"});
            List<Modeles> lModeles = sModeles.findAll(new List<String>() { "Marques" });
            vmSimu.GaragistesConfs = new List<VMSimulation.GaragistesConf>();
            vmSimu.ModelesConfs = new List<VMSimulation.ModelesConf>();

            foreach (var item in lGaragistes)
            {
                vmSimu.GaragistesConfs.Add(new VMSimulation.GaragistesConf(item,true));
            }

            foreach (var item in lModeles)
            {
                vmSimu.ModelesConfs.Add(new VMSimulation.ModelesConf(item, 0));
            }

            vmSimu.dateStart = DateTime.Now;
            vmSimu.nbJours = NBJOURS_DEFAULT;

            return View(vmSimu);
        }

        [HttpPost]
        public ActionResult startSimulation(VMSimulation pVmSimu)
        {
            VMSimulationData vmSimuData = new VMSimulationData();

            List<int> garagistesIds = new List<int>();
            List<int[]> modelesQuantity = new List<int[]>();

            string sGaragiste = "garagiste";
            string sModele = "modele";
            foreach (string item in this.Request.Params)
            {
                string val = this.Request.Params[item];
                // Concern a garagiste
                if (item.StartsWith(sGaragiste))
                {
                    if(val.StartsWith("true"))
                    {
                        garagistesIds.Add(int.Parse(item.Substring(sGaragiste.Length,item.Length - sGaragiste.Length)));
                    }
                }
                else if (item.ToString().StartsWith(sModele))
                {
                    int[] row = new int[2];
                    row[0] = int.Parse(item.Substring(sModele.Length,item.Length - sModele.Length));
                    row[1] = int.Parse(val);
                    if(row[1] >= 0)
                        modelesQuantity.Add(row);
                }
                var x = item;
                //garagiste // model
            }

            //Populate VMSIMUDATA

            vmSimuData.debut = pVmSimu.dateStart;
            vmSimuData.nbJours = pVmSimu.nbJours;

            ErrorHandler eh = new ErrorHandler();
            ServiceGaragistes serviceGaragiste = new ServiceGaragistes(eh);
            ServiceModeles serviceModele = new ServiceModeles(eh);

            vmSimuData.lVMGaragistes = new List<VMGaragiste>();
            vmSimuData.lVoitures = new List<Voiture>();

            foreach (var item in garagistesIds)
            {
                vmSimuData.lVMGaragistes.Add(new VMGaragiste(serviceGaragiste.findById(item)));
            }

            foreach (var item in modelesQuantity)
            {
                Modeles modele = serviceModele.findById(item[0]);
                for (int i = 1; i <= item[1]; i++)
			    {
                    Voiture voiture = new Voiture(modele,i);
                    vmSimuData.lVoitures.Add(voiture);
			    }
            }

            vmSimuData.nom = pVmSimu.nom == String.Empty ? "sanstitre" : pVmSimu.nom;


            return runSimulation(vmSimuData);
        }

        private void enregistreStats(VMSimulationData vmSimuData)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceStatistiques serviceStatistiques = new ServiceStatistiques(eh);
            ServiceSimulations serviceSimulations = new ServiceSimulations(eh);

            Simulations simulation = new Simulations();
            simulation.nom = vmSimuData.nom;
            simulation.created = DateTime.Now;
            serviceSimulations.Insert(simulation);

            foreach (var stat in vmSimuData.statistiques)
            {
                stat.simulation_id = simulation.id;
                if(stat.garagiste_id != 0)
                    serviceStatistiques.Insert(stat);
            }
        }

        public ActionResult runSimulation(VMSimulationData vmSimuData)
        {
            List<VMSimulationData.DayRepport> lRepports = new List<VMSimulationData.DayRepport>();
            DateTime currentDate = vmSimuData.debut;
            Statistiques stat = new Statistiques();
            List<Statistiques> lStats = new List<Statistiques>();

            for (int i = 1; i <= vmSimuData.nbJours; i++)
            {
                VMSimulationData.DayRepport repport = new VMSimulationData.DayRepport();
                repport.evenements = new List<string>();
                currentDate.AddDays(i);
                repport.jour = currentDate;
                repport.indexJour = i;

                foreach (Voiture voiture in vmSimuData.lVoitures)
                {
                    repport.evenements.Add(voiture.Roule(currentDate,i,vmSimuData.lVMGaragistes, out stat));
                    lStats.Add(stat);
                }

                lRepports.Add(repport);
            }

            //écriture dans le fichier
            String rootPath = Server.MapPath("~");
            FileStream fichier = System.IO.File.Create(rootPath + filePath + vmSimuData.nom);
            fichier.Close();
            StreamWriter file = new StreamWriter(rootPath + filePath + vmSimuData.nom,true);
            file.Flush();
            int i2 = 0;
            foreach (var repport in lRepports)
            {
                file.WriteLine(repport.indexJour + " " + repport.jour.AddDays(i2).DayOfWeek);
                foreach (var evenement in repport.evenements)
                {
                   file.WriteLine("  " + evenement);    
                }
                i2++;
            }
            file.Close();

            //Enregsitre les stats
            vmSimuData.statistiques = lStats;
            enregistreStats(vmSimuData);

            return View(lRepports);
        }

        [HttpGet]
        public ActionResult Demo()
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceGaragistes serviceGaragiste = new ServiceGaragistes(eh);
            ServiceModeles serviceModele = new ServiceModeles(eh);
            VMSimulationData vmSimuData = new VMSimulationData();

            //Durée
            vmSimuData.debut = DateTime.Now.AddDays(1);
            vmSimuData.nbJours = 10;
            vmSimuData.nom = "demo";
            
            //1 garagiste
            List<VMGaragiste> lVMGaragiste = new List<VMGaragiste>();

            int nbGaragiste = 1;
            foreach (var garagiste in serviceGaragiste.findAll(new List<string>() { "Revisions_Garagistes", "Vacances","Franchises"}))
            {
                if (nbGaragiste <= 1)
                {
                    VMGaragiste vmGara = new VMGaragiste(garagiste);
                    vmGara.activerVacances(vmSimuData.debut,vmSimuData.nbJours);
                    lVMGaragiste.Add(vmGara);
                    nbGaragiste++;
                }
            }
            
            vmSimuData.lVMGaragistes = lVMGaragiste;

            //2 voitures
            List<Voiture> lVoiture = new List<Voiture>();
            List<Modeles> lModeles = new List<Modeles>();
            lModeles.Add(serviceModele.findAll(new List<string>(){"Révisions","Marques"}).FirstOrDefault());
            foreach (var item in lModeles)
            {
                for (int i = 1; i <= 5; i++)
                {
                    Voiture voiture = new Voiture(item, i,119999);
                    lVoiture.Add(voiture);
                }
            }
            vmSimuData.lVoitures = lVoiture;

            return runSimulation(vmSimuData);
            /**
             * TODO :
             *  Affichage des stats
             *  CSS
             *  
             * Bonus :
             * Gestion des pannes
             **/
        }
	}
}