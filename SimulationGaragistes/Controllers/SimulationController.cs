using SimlulationGaragistesService.Service;
using SimulationGaragistes.ViewModels;
using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace SimulationGaragistes.Controllers
{
    public class SimulationController : Controller
    {
        //
        // GET: /Simulation/
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
            vmSimu.Garagistes = new List<VMSimulation.GaragistesConf>();
            vmSimu.Modeles = new List<VMSimulation.ModelesConf>();

            foreach (var item in lGaragistes)
            {
                vmSimu.Garagistes.Add(new VMSimulation.GaragistesConf(item,true));
            }

            foreach (var item in lModeles)
            {
                vmSimu.Modeles.Add(new VMSimulation.ModelesConf(item, 1));
            }

            return View(vmSimu);
        }
	}
}