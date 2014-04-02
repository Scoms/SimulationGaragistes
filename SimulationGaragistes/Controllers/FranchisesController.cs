using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils;
using SimlulationGaragistesService.Service;
using SimulationGaragistesDAL.Model;
using System.Net.Http;

namespace SimulationGaragistes.Controllers
{
    public class FranchisesController : Controller 
    {
        private ErrorHandler _eh;
        private ServiceFranchises _service;

        public FranchisesController()
        {
            this._eh = new ErrorHandler();
            this._service = new ServiceFranchises(this._eh);
        }

        public ActionResult Index()
        {
            List<Franchises> lFranchises = this._service.findAll();
            ViewData["lFranchises"] = lFranchises;
            return View();
        }

        [HttpGet]
        public ActionResult Insert(int id = -1)
        {
            Franchises fran = new Franchises();
            
            //Init Franchise
            if (id != -1)
            {
                fran = this._service.findById(id);
                if (fran == null)
                {
                    ModelState.AddModelError("error", "La franchise demandée n'existe pas.");
                    return View(new Franchises());
                }
            }

            return View(fran);
        }

        [HttpPost]
        public ActionResult Insert(Franchises fran)
        {
            //Franchises fran = id != -1 ? new Franchises() : this._service.findById(id);
            string message = String.Empty;
            if (fran.id != -1)
            {
                this._service.Edit(fran);
                message = "La franchise a bien été modifiée";
            }
            //Insert
            else
            {
                this._service.Insert(fran);
                message = "La franchise a bien été créer";
            }

            if (this._eh.hasErrors())
            {
                ModelState.AddModelError("error", this._eh.getErrors());
                return View(fran);
            }
            TempData["message"] = message;
            return RedirectToAction("Insert");
        }

        public ActionResult Delete(int id)
        {
            Franchises fran = this._service.findById(id);
            if (fran == null)
            {
                ModelState.AddModelError("error", "La franchise spécifiée n'existe pas.");
            }
            else
            {
                this._service.Delete(fran);
                if(!this._eh.hasErrors())
                {
                    TempData["success"] = "La franchise " + fran.label +" bien été supprimée.";
                }
            }
            return RedirectToAction("Index");
        }
    }
}