using SimulationGaragistesDAL.Model;
using SimlulationGaragistesService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils;
using SimulationGaragistes.ViewModels;
using Enumerations;

namespace SimulationGaragistes.Controllers
{
    public class GaragistesController : Controller
    {
        public ActionResult Index()
        {
            List<string> lIncludes = new List<string>();
            lIncludes.Add("Franchises");
            ServiceGaragistes service = new ServiceGaragistes(new ErrorHandler());
            List<Garagistes> lGaragistes = service.findAll(lIncludes);
            return View(lGaragistes);
        }

        [HttpGet]
        public ActionResult Insert(int id = -1)
        {
            //Init
            ErrorHandler eh = new ErrorHandler();
            VMGaragistes vmGaragiste = new VMGaragistes();
            ServiceGaragistes serviceG = new ServiceGaragistes(eh);
            ServiceFranchises serviceF = new ServiceFranchises(eh);
            Garagistes garagiste = null;

            //Get garagiste
            if(id != -1)
            {
                garagiste = serviceG.findById(id);
            }

            if (garagiste == null)
            {
                garagiste = new Garagistes();
            }

            //Populate the VM
            vmGaragiste.lFranchises = new List<SelectListItem>();
            List<Franchises> listFranchises = serviceF.findAll();
            if(garagiste.Franchises != null)
            {
                vmGaragiste.lFranchises.Add(new SelectListItem() { Text = garagiste.Franchises.label, Value = garagiste.Franchises.id.ToString() });
                listFranchises.Remove(listFranchises.Find(p => p.id == garagiste.Franchises.id));
            }
            //Fill franchises list
            foreach (Franchises franchise in listFranchises)
            {
                vmGaragiste.lFranchises.Add(new SelectListItem() { Text = franchise.label, Value = franchise.id.ToString() });
            }

            vmGaragiste.Garagiste = garagiste;
            return View(vmGaragiste);
        }

        [HttpPost]
        public ActionResult Insert(Garagistes garagiste)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceFranchises serviceF = new ServiceFranchises(eh);
            ServiceGaragistes serviceG = new ServiceGaragistes(eh);
            VMGaragistes vm = new VMGaragistes();
            List<Franchises> listFranchises = serviceF.findAll();
            string message = String.Empty;
            garagiste.Franchises = listFranchises.Find(l => l.id == int.Parse(this.Request.Form["franchise"]));

            if (garagiste.id == 0)
            {
                serviceG.Insert(garagiste);
                message = "Le garagiste a bien été crée";
            }
            else
            {
                serviceG.Edit(garagiste);
                message = "Le garagiste a bien été modifié";
            }

            if (eh.hasErrors())
            {
                ModelState.AddModelError("error", eh.getErrors());
                vm.Garagiste = garagiste;
                vm.lFranchises = new List<SelectListItem>();
                if (garagiste.Franchises != null)
                {
                    vm.lFranchises.Add(new SelectListItem() { Text = garagiste.Franchises.label, Value = garagiste.Franchises.id.ToString() });
                    listFranchises.Remove(listFranchises.Find(p => p.id == garagiste.Franchises.id));
                }
                //Fill franchises list
                foreach (Franchises franchise in listFranchises)
                {
                    vm.lFranchises.Add(new SelectListItem() { Text = franchise.label, Value = franchise.id.ToString() });
                }
                return View(vm);
            }
            TempData["message"] = message;
            return RedirectToAction("Insert");
        }

        public ActionResult Delete(int id){
            ErrorHandler eh = new ErrorHandler();
            ServiceGaragistes service = new ServiceGaragistes(eh);
            Garagistes garagiste = service.findById(id);
            if (garagiste == null)
            {
                ModelState.AddModelError("error", "Le garagiste spécifié n'existe pas.");
            }
            else
            {
                service.Delete(garagiste);
                if(!eh.hasErrors())
                {
                    TempData["success"] = "Le garagiste " + garagiste.nom +" bien été supprimé.";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ConfigureRevisions(int id)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceGaragistes serviceGaragiste = new ServiceGaragistes(eh);
            ServiceRévisions serviceRevision = new ServiceRévisions(eh);

            VMRevisions vm = new VMRevisions();
            //List<Révisions> revSpéciales = serviceGaragiste.GetRevisions(id);

            Garagistes garagiste = serviceGaragiste.findById(id);
            if (garagiste == null)
            {
                return RedirectToAction("ConfigureRevisions",new{ id = id });   
            }



            vm.lRevisions = serviceGaragiste.GetRevisions(id);// serviceRevision.findAll(new List<string>() { "Modeles" });
            vm.idGaragiste = garagiste.id;

            return View(vm);
        }

        [HttpGet]
        public ActionResult ChangeDureeRevision(int idGaragiste = -1, int idRevision = -1, int pDuree = -1)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceGaragistes service = new ServiceGaragistes(eh);
            Revisions_Garagistes revGar = new Revisions_Garagistes();
            revGar.duree = pDuree;
            revGar.garagiste_id = idGaragiste;
            revGar.revision_id = idRevision;
            service.ChangeDureeRevision(revGar);
            if(eh.hasErrors())
            {
                ModelState.AddModelError("error",eh.getErrors());
            }
            return RedirectToAction("ConfigureRevisions", new {id = idGaragiste });
        }

        [HttpGet]
        public ActionResult ConfigureVacances(int id, string errors = "-1")
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceGaragistes service = new ServiceGaragistes(eh);
            Garagistes garagiste = service.findById(id);
            VMVacances vm = new VMVacances();
            vm.Garagiste = garagiste;
            Vacances vacance = new Vacances();
            vacance.garagiste_id = id;
            vm.Vacance = vacance;
            vm.Vacance.debut = DateTime.Now;
            vm.Vacance.fin = DateTime.Now.AddDays(1);

            if (garagiste == null)
            {
                return RedirectToAction("Index");
            }
            if(!errors.Equals("-1"))
            {
                ModelState.AddModelError("error", errors);
            }
            

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddVacance(Vacances vacance)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceVacances service = new ServiceVacances(eh);
            service.Insert(vacance);
            
            return RedirectToAction("ConfigureVacances", new { id = vacance.garagiste_id , errors = eh.getErrors()});

        }

        [HttpPost]
        public ActionResult EditVacance(Vacances vacance)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceVacances service = new ServiceVacances(eh);
            service.Edit(vacance);

            vacance.fin = DateTime.Parse( this.Request.Params["vacance.finModif"]);

            if (eh.hasErrors())
            {
                this.ModelState.AddModelError("error", eh.getErrors());
                return RedirectToAction("ConfigureVacances", new { id = vacance.garagiste_id });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]

        public ActionResult DeleteVacance(int id, int idGaragiste)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceVacances service = new ServiceVacances(eh);
            Vacances vacance = service.findById(id);
            service.Delete(vacance);
            return RedirectToAction("ConfigureVacances", new { id = idGaragiste });
        }
    }
}