using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils;
using SimlulationGaragistesService.Service;
using SimulationGaragistesDAL.Model;

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

            if (this.Request.HttpMethod == "POST")
            {
                fran.label = this.Request.Form["label"];
                this._service.Insert(fran);
                if (this._eh.hasErrors())
                {
                    ModelState.AddModelError("error", this._eh.getErrors());
                }
                else
                {
                    if (id <= 0)
                    {
                        TempData["success"] = "La franchise a bien été créée";
                        // permet l'ajout en série de franchises
                        fran = new Franchises();
                    }
                    else
                    {
                        TempData["success"] = "La franchise a bien été modifiée";                        
                    }
                }
            }
            return View(fran);
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