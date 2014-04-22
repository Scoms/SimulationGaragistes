using SimlulationGaragistesService.Service;
using SimulationGaragistes.ViewModels;
using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace SimulationGaragistes.Controllers
{
    public class SimulationController : Controller
    {
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

            vmSimuData.lGaragistes = new List<Garagistes>();
            vmSimuData.lVoitures = new List<Voiture>();

            foreach (var item in garagistesIds)
            {
                vmSimuData.lGaragistes.Add(serviceGaragiste.findById(item));
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
            return runSimulation(vmSimuData);
        }

        public ActionResult runSimulation(VMSimulationData vmSimuData)
        {
            List<VMSimulationData.DayRepport> lRepports = new List<VMSimulationData.DayRepport>();
            DateTime currentDate = vmSimuData.debut;

            for (int i = 1; i <= vmSimuData.nbJours; i++)
            {
                VMSimulationData.DayRepport repport = new VMSimulationData.DayRepport();
                repport.evenements = new List<string>();
                repport.jour = currentDate;
                repport.indexJour = i;
                currentDate.AddDays(1);

                foreach (Voiture voiture in vmSimuData.lVoitures)
                {
                    repport.evenements.Add(voiture.Roule(currentDate,i));
                }

                lRepports.Add(repport);
            }
            return View(lRepports);
        }
	}
}