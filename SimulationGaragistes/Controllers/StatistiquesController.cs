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
            VMStatistiques vm = new VMStatistiques();
            vm.Simulation = service.findById(id);
            vm.lGaragistesLibres = new List<Garagistes>();


            List<VMStatistiques.OccupationGaragiste> occupations = new List<VMStatistiques.OccupationGaragiste>();
            foreach (var item in vm.Simulation.Statistiques)
	        {
                if (item.revision_id != null)
                {
                    //VMStatistiques.OccupationGaragiste prevRow = occupations.Find(o => o.Garagiste.id == item.garagiste_id);
                    VMStatistiques.OccupationGaragiste prevRow = occupations.Find(o => o.Garagiste.id == item.garagiste_id);                    
                    int duree = this.getDureeRevision(item.garagiste_id, (int)item.revision_id);
                    duree = duree == -1 ? item.Révisions.defaultTime : duree;

                    //si le garagiste n'est pas encore rentré
                    if (prevRow == null)
                    {
                        prevRow = new VMStatistiques.OccupationGaragiste();
                        prevRow.Interventions = 1;
                        prevRow.Garagiste = item.Garagistes;
                        prevRow.DureeTotal = duree;
                        VMGaragiste vmGaragiste = new VMGaragiste(item.Garagistes);
                        vmGaragiste.activerVacances((DateTime)vm.Simulation.debut, (int)vm.Simulation.duree);
                        prevRow.JourTravailles = (int)vm.Simulation.duree - vmGaragiste.IndexVacances.Count();
                        occupations.Add(prevRow);
                    }
                    else
                    {
                        prevRow.Interventions++;
                        prevRow.DureeTotal += duree;
                    }
                }
                else
                {
                    vm.lGaragistesLibres.Add(item.Garagistes);
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