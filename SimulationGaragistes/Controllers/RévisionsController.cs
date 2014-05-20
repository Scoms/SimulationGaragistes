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
            bool aleatoire = revision.modele_id == null;
            if(!aleatoire)
             id = (int)revision.modele_id;
            if (revision != null)
            {
                this._serviceR.Delete(revision);
            }
            if(aleatoire)
                return RedirectToAction("IndexAleatoire");
            return RedirectToAction("seeRevisions", "Modeles", new { id = id, errors = this._eh.getErrors() });
        }

        /*
         * Partie effectué tardivement d'ou le coté "sale" du code
         * */
        [HttpGet]
        public ActionResult IndexAleatoire()
        {
            List<Révisions> lRev = this._serviceR.findAll().Where(r => r.modele_id == null).ToList();
            return View(lRev);
        }

        [HttpGet]
        public ActionResult InsertAleatoire(int id = 0)
        {
            Révisions revision = new Révisions();
            if (id != 0)
            {
                revision = this._serviceR.findById(id);
                if (revision == null)
                {
                    revision.km = 0;
                    revision.modele_id = null;
                    return RedirectToAction("IndexAleatoire");
                }
            }
            return View(revision);
        }

        [HttpPost]
        public ActionResult InsertAleatoire(Révisions revision)
        {
            string message = String.Empty;
            revision.km = 0;
            revision.modele_id = null;
            int maxCount = 1;
            //int id = (int)this._serviceR.findById(revision.id).modele_id;
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                if (revision.id > 0)
                {
                    maxCount = 2;
                }
                if(revision.defaultTime <= 0)
                {
                    this._eh.addError("Durée invalide");
                }
                if (context.Révisions.Where(r => r.label.ToUpper().Equals(revision.label)).Count() == maxCount)
                {
                    this._eh.addError("La panne existe déjà");
                }

                if (!this._eh.hasErrors())
                {
                    if (revision.id != 0)
                    {
                        TempData["success"] = "La panne à bien été modifiée";
                        context.Entry(revision).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        TempData["success"] = "La panne à bien été ajoutée";
                        context.Révisions.Add(revision);
                    }
                    context.SaveChanges();
                    return RedirectToAction("IndexAleatoire");
                    return View();
                }
                else
                {
                    TempData["errors"] = this._eh.getErrors();
                    return View(revision);
                }
            }
            return View(revision);
        }
	}
}