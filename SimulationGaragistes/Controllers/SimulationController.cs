using SimlulationGaragistesService.Service;
using SimulationGaragistes.ViewModels;
using SimulationGaragistesDAL.Model;
using SimulationGaragistesDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
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

        public VMSimulationData prepareSimulationData(VMSimulation pVmSimu)
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
                    if (val.StartsWith("true"))
                    {
                        garagistesIds.Add(int.Parse(item.Substring(sGaragiste.Length, item.Length - sGaragiste.Length)));
                    }
                }
                else if (item.ToString().StartsWith(sModele))
                {
                    int[] row = new int[2];
                    row[0] = int.Parse(item.Substring(sModele.Length, item.Length - sModele.Length));
                    row[1] = int.Parse(val);
                    if (row[1] >= 0)
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
                VMGaragiste vmGaragiste = new VMGaragiste(serviceGaragiste.findById(item));
                vmGaragiste.activerVacances(pVmSimu.dateStart, pVmSimu.nbJours);
                vmSimuData.lVMGaragistes.Add(vmGaragiste);
            }

            foreach (var item in modelesQuantity)
            {
                Modeles modele = serviceModele.findById(item[0]);
                for (int i = 1; i <= item[1]; i++)
                {
                    Voiture voiture = new Voiture(modele, i);
                    vmSimuData.lVoitures.Add(voiture);
                }
            }

            vmSimuData.nom = pVmSimu.nom == String.Empty ? "sanstitre" : pVmSimu.nom;
            return vmSimuData;
        }
        
        [HttpPost]
        public ActionResult startSimulation(VMSimulation vmSimu)
        {
            VMSimulationData vmSimuData = prepareSimulationData(vmSimu);
            //this.Session["vmSimuData"] = vmSimuData;
            //return View(vmSimuData.lVoitures);
            List<VMSimulationData.DayRepport> lRepports = runSimulation(vmSimuData);
            return View(lRepports);
        }

        private int enregistreStats(VMSimulationData vmSimuData)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceStatistiques serviceStatistiques = new ServiceStatistiques(eh);
            ServiceSimulations serviceSimulations = new ServiceSimulations(eh);

            Simulations simulation = new Simulations();
            simulation.nom = vmSimuData.nom;
            simulation.created = DateTime.Now;
            simulation.duree = vmSimuData.nbJours;
            simulation.debut = vmSimuData.debut;
            serviceSimulations.Insert(simulation);

            foreach (var stat in vmSimuData.statistiques)
            {
                if (stat != null)
                {
                    stat.simulation_id = simulation.id;
                    if (stat.garagiste_id != 0)
                    {
                        serviceStatistiques.Insert(stat);
                    }
                }
            }

            foreach (var item in vmSimuData.lVMGaragistes)
            {
                Statistiques stat = new Statistiques();
                stat.simulation_id = simulation.id;
                stat.garagiste_id = item.Garagiste.id;
                stat.revision_id = -1;
                stat.revision = "";
                stat.voiture = "";
                stat.garagiste = item.Garagiste.nom + "("+item.Garagiste.Franchises.label+")";
                serviceStatistiques.Insert(stat);
            }
            return simulation.id;
        }

        public List<VMSimulationData.DayRepport> runSimulation(VMSimulationData vmSimuData)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceGaragistes serviceGar = new ServiceGaragistes(eh);
            var x = this.Request.Params;
            vmSimuData.lPannes = new List<Panne>();
            vmSimuData.lPannes = GetPannes(this.Request);

            foreach (Panne panne in vmSimuData.lPannes)
            {
                Voiture voiture = vmSimuData.lVoitures.Where(v => v.id == panne.Voiture.modele.id).First();
                if(voiture.lPannes == null){
                    voiture.lPannes = new List<Panne>();
                }
                voiture.lPannes.Add(panne);
            }

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
                    repport.evenements.Add(voiture.Roule(currentDate,i,vmSimuData.lVMGaragistes, out stat,vmSimuData.nbJours));

                    if(stat != null)
                        if(stat.garagiste_id != 0)
                        {
                            VMGaragiste vmGara = new VMGaragiste(serviceGar.findById(stat.garagiste_id));
                            vmGara.activerVacances(vmSimuData.debut,vmSimuData.nbJours);
                            stat.nbvacances = vmGara.IndexVacances.Distinct().Where(v => v > 0 && v <= vmSimuData.nbJours).Count();
                            lStats.Add(stat);
                        }
                }
                lRepports.Add(repport);
            }

            //écriture dans le fichier
            String rootPath = Server.MapPath("~");
            FileStream fichier = System.IO.File.Create(rootPath + filePath + vmSimuData.nom + ".txt");
            fichier.Close();
            StreamWriter file = new StreamWriter(new FileStream(rootPath + filePath + vmSimuData.nom + ".txt",FileMode.OpenOrCreate,FileAccess.ReadWrite) , Encoding.UTF8);
            file.Flush();
            int i2 = 0;
            var french = new System.Globalization.CultureInfo("FR-fr");
            var info = french.DateTimeFormat;
           
            foreach (var repport in lRepports)
            {
                string jourSemaine = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(repport.jour.AddDays(i2).DayOfWeek);
                file.WriteLine(repport.indexJour + " " + jourSemaine);
                foreach (var evenement in repport.evenements)
                {
                   file.WriteLine("  " + evenement);    
                }
                i2++;
            }
            file.Close();

            //Enregsitre les stats
            vmSimuData.statistiques = lStats;
            int simuid = enregistreStats(vmSimuData);

            return lRepports;
            //return RedirectToAction("Details", "Statistiques", new { id = simuid });
        }

        private List<Panne> GetPannes(HttpRequestBase httpRequestBase)
        {
            string panneString = "panne";
            string nomString ="nom";
            string dureeString ="duree";
            string voitureString ="voiture";
            string jourString = "jour";
            List<Panne> lPannes = new List<Panne>();
            NameObjectCollectionBase.KeysCollection keys = httpRequestBase.Params.Keys;
            var Params = httpRequestBase.Params;
            foreach (var item in keys)
            {
                if (item.ToString().StartsWith(panneString) && item.ToString().EndsWith(nomString))
                {
                    string sansPanne = item.ToString().Substring(panneString.Length,item.ToString().Length - panneString.Length);
                    string idPanne = (sansPanne.Substring(0,sansPanne.Length - nomString.Length));
                    var y = httpRequestBase.Params[item.ToString()];

                    string voiture = httpRequestBase.Params[panneString + idPanne + voitureString];
                    string voitureSansPF = voiture.Substring(0, voiture.Length - 1);
                    int voitureID;
                    string voitureIDString = (voitureSansPF.Substring(voitureSansPF.Length - 1, voiture.Length - voitureSansPF.Length));
                    bool parsed = int.TryParse(voitureIDString,out voitureID);
                    int i = 2;
                    while(parsed){
                        voitureIDString = (voitureSansPF.Substring(voitureSansPF.Length - i, voitureSansPF.Length - voitureSansPF.Length));
                        parsed = int.TryParse(voitureIDString,out voitureID);
                        i++;
                    }


                    //int voitureID = ;
                    string marque;
                    string modeleString;
                    string[] voitureSplit;
                    char[] separators = new char[]{' '};
                    voitureSplit = voiture.Split(separators);
                    //TEMP PEUT ETRE A REVOIR 
                    marque = voitureSplit.First();
                    modeleString = voitureSplit.Skip(1).First();
                    modeleString = modeleString.Substring(0, modeleString.Length - 2 - voitureID.ToString().Length);
                    ServiceModeles serviceModele = new ServiceModeles(new ErrorHandler());
                    Modeles modele = serviceModele.Find(marque,modeleString);
                    
                    Panne panne = new Panne() { 
                        Duree = int.Parse(Params[panneString + idPanne + dureeString]),
                        IndexJour = int.Parse(Params[panneString + idPanne + jourString]),
                        Voiture = new Voiture(modele,voitureID)
                    };
                    lPannes.Add(panne);
                }
            }


            return lPannes;
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

            int i2 = 1;
            int nbGaragiste = 5;
            foreach (var garagiste in serviceGaragiste.findAll(new List<string>() { "Revisions_Garagistes", "Vacances","Franchises"}))
            {
                if (i2 < nbGaragiste)
                {
                    VMGaragiste vmGara = new VMGaragiste(garagiste);
                    vmGara.activerVacances(vmSimuData.debut,vmSimuData.nbJours);
                    lVMGaragiste.Add(vmGara);
                    i2++;
                }
            }
            
            vmSimuData.lVMGaragistes = lVMGaragiste;

            //2 voitures
            List<Voiture> lVoiture = new List<Voiture>();
            List<Modeles> lModeles = new List<Modeles>();
            lModeles.Add(serviceModele.findAll(new List<string>(){"Révisions","Marques"}).FirstOrDefault());
            foreach (var item in lModeles)
            {
                for (int i = 1; i <= 10; i++)
                {
                    Voiture voiture = new Voiture(item, i,119999);
                    lVoiture.Add(voiture);
                }
            }
            vmSimuData.lVoitures = lVoiture;
            TempData["vmSimuData"] = vmSimuData;
            List<VMSimulationData.DayRepport> lRepports = runSimulation(vmSimuData);
            return View(lRepports);
            /**
             * TODO :
             *  
             * Bonus :
             * Gestion des pannes
             **/
        }
	}
}