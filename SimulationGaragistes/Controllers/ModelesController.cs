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
    public class ModelesController : Controller
    {
        //
        // GET: /Modeles/
        public ActionResult Index()
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceModeles service = new ServiceModeles(eh);
            List<Modeles> lModeles= service.findAll(new List<String>(){"Marques"});
            return View(lModeles);
        }

        [HttpGet]
        public ActionResult Insert(int id = 0)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceModeles serviceModele = new ServiceModeles(eh);
            ServiceMarques serviceMarque = new ServiceMarques(eh);
            Modeles modele = new Modeles();
            VMModeles vm = new VMModeles();
            vm.Marques = new List<SelectListItem>();

            if (id != 0)
            {
                modele = serviceModele.findById(id);
                if (modele == null)
                {
                    return RedirectToAction("Index");
                }
            }
            //Populate vm
            vm.Modele = modele;
            if (modele.Marques == null)
            {
                modele.Marques = new Marques();          
            }
            Marques marque = modele.Marques;
            foreach (var item in serviceMarque.findAll())
            {
                bool selected = modele.Marques.id == item.id;
                vm.Marques.Add(new SelectListItem() { Text = item.label, Value = item.id.ToString(), Selected = selected });
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Insert(Modeles modele)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceModeles serviceModele = new ServiceModeles(eh);
            ServiceMarques serviceMarque = new ServiceMarques(eh);
            VMModeles vm = new VMModeles();
            vm.Marques = new List<SelectListItem>();
            modele.Marques = serviceMarque.findAll().Find(m => m.id == int.Parse(this.Request.Form["marque"]));
            modele.marque_id = modele.Marques.id;
            string message = String.Empty;

            if (modele.id != 0)
            {
                serviceModele.Edit(modele);
                message = "Le modèle à bien été modifié";
            }
            else
            {
                serviceModele.Insert(modele);
                message = "Le modèle à bien été ajouté";
            }

            if (eh.hasErrors())
            {
                ModelState.AddModelError("error", eh.getErrors());
            }
            else
            {
                TempData["success"] = message;
            }
            //Populate vm
            vm.Modele = modele;
            if (modele.Marques == null)
            {
                modele.Marques = new Marques();
            }
            Marques marque = modele.Marques;
            foreach (var item in serviceMarque.findAll())
            {
                bool selected = modele.Marques.id == item.id;
                vm.Marques.Add(new SelectListItem() { Text = item.label, Value = item.id.ToString(), Selected = selected });
            }

            return View(vm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }
	}
}