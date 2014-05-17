using SimlulationGaragistesService.Service;
using SimulationGaragistes.ViewModels;
using SimulationGaragistesDAL.Model;
using SimulationGaragistesDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace SimulationGaragistes.Controllers
{
    public class StatistiquesController : Controller
    {
        //
        // GET: /Statistiques/
        public ActionResult Index()
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceSimulations service = new ServiceSimulations(eh);
            return View(service.findAll(new List<string>() {}));
        }

        public ActionResult Details(int id)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceSimulations service = new ServiceSimulations(eh);
            ServiceRévisions serviceRev = new ServiceRévisions(eh);
            ServiceGaragistes serviceGar = new ServiceGaragistes(eh);
            VMStatistiques vm = new VMStatistiques();
            vm.Simulation = service.findById(id);
            vm.lGaragistesLibres = new List<string>();
            List<string> lGaragistesOccu = new List<string>();


            List<VMStatistiques.OccupationGaragiste> occupations = new List<VMStatistiques.OccupationGaragiste>();
            foreach (var item in vm.Simulation.Statistiques)
	        {
                if (item.revision_id != -1)
                {
                    //VMStatistiques.OccupationGaragiste prevRow = occupations.Find(o => o.Garagiste.id == item.garagiste_id);
                    VMStatistiques.OccupationGaragiste prevRow = occupations.Find(o => o.Garagiste_id == item.garagiste_id);
                    int duree = 0;
                    if (item.duree != null)
                    {
                        duree = (int)item.duree;
                    }
                    duree = duree == -1 ? serviceRev.findById(item.revision_id).defaultTime : duree;

                    //si le garagiste n'est pas encore rentré
                    if (prevRow == null)
                    {
                        prevRow = new VMStatistiques.OccupationGaragiste();
                        prevRow.Garagiste_id = item.garagiste_id;
                        prevRow.Interventions = 1;
                        //prevRow.Garagiste = item.Garagistes;
                        prevRow.DureeTotal = duree;
                        prevRow.Garagiste = item.garagiste;
                        //VMGaragiste vmGaragiste = new VMGaragiste(serviceGar.findById(item.garagiste_id));
                        //vmGaragiste.activerVacances((DateTime)vm.Simulation.debut, (int)vm.Simulation.duree);
                        //List<int> indexVacances = vmGaragiste.IndexVacances.Where(v => v > (int)vm.Simulation.duree).ToList();
                        prevRow.JourTravailles = (int)vm.Simulation.duree - item.nbvacances;
                        occupations.Add(prevRow);
                    }
                    else
                    {
                        prevRow.Interventions++;
                        prevRow.DureeTotal += duree;
                    }
                }
	        }

            
            vm.Occupations = occupations;
            return View(vm);
        }

        private int getDureeRevision(int idGaragiste, int idRevision)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                Revisions_Garagistes rev = context.Revisions_Garagistes.Where(r => r.garagiste_id == idGaragiste && r.revision_id == idRevision).FirstOrDefault();
                if(rev == null)
                {
                    return -1;
                }
                return rev.duree;
            }
        }
	}
}