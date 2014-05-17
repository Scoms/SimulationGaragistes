using SimlulationGaragistesService.Service;
using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace SimulationGaragistes.Controllers
{
    public class RévisionsController : Controller
    {
        private ErrorHandler _eh;
        private ServiceRévisions _serviceR;

        public RévisionsController()
        {
            this._eh = new ErrorHandler();
            this._serviceR = new ServiceRévisions(this._eh); 
        }

        //
        // GET: /Révisions/
        /*
        public ActionResult Index()
        {
            return View(this._serviceR.findAll());
        }*/

        [HttpGet]
        public ActionResult Insert(int id = 0)
        {
            Révisions revision = new Révisions();
            if (id != 0)
            {
                revision = this._serviceR.findById(id);
                if (revision == null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(revision);
        }

        [HttpPost]
        public ActionResult Insert(Révisions revision)
        {
            string message = String.Empty;
            //int id = (int)this._serviceR.findById(revision.id).modele_id;
            int id = (int)revision.modele_id;
            if (revision.id <= 0)
            {
                this._serviceR.Insert(revision);
                message = "La révision à bien été ajoutée.";
                revision = new Révisions();
            }
            else
            {
                message = "La révision à bien été modifiée";
                this._serviceR.Edit(revision);
            }

            if (this._eh.hasErrors())
            {
                TempData["error"] = this._eh.getErrors();
                ViewBag.errors = this._eh.getErrors();
            }
            else
            {
                TempData["success"] = message;
            }
      
            return RedirectToAction("seeRevisions", "Modeles", new { id = id, errors = this._eh.getErrors()});
        }

        public ActionResult Delete(int id)
        {
            Révisions revision = this._serviceR.findById(id);
            id = (int)revision.modele_id;
            if (revision != null)
            {
                this._serviceR.Delete(revision);
            }
            return RedirectToAction("seeRevisions", "Modeles", new { id = id, errors = this._eh.getErrors() });
        }
	}
}