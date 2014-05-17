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
    public class MarquesController : Controller
    {
        //
        // GET: /Marques/
        public ActionResult Index()
        {
            ServiceMarques service = new ServiceMarques(new ErrorHandler());
            List<Marques> lMarques = service.findAll();
            return View(lMarques);
        }

        [HttpGet]
        public ActionResult Insert(int id = 0)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceMarques service = new ServiceMarques(eh);
            Marques marque = new Marques();

            //if Edit
            if(id != 0)
            {
                marque = service.findById(id);
                if (marque == null)
                {
                    TempData["error"] = "La marque spécifiée n'existe pas";
                    marque = new Marques();
                }
            }

            return View(marque);
        }

        [HttpPost]
        public ActionResult Insert(Marques marque)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceMarques service = new ServiceMarques(eh);
            string message = String.Empty;

            if(marque.id == 0)
            {
                message = "La marque à bien été ajoutée";
                service.Insert(marque);
            }
            else
            {
                message = "La marque à bien été modifiée";
                service.Edit(marque);
            }

            if (eh.hasErrors())
            {
                TempData["error"] = eh.getErrors();
                return View(marque);
            }
            TempData["success"] = message;
            return RedirectToAction("Insert");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ErrorHandler eh = new ErrorHandler();
            ServiceMarques service = new ServiceMarques(eh);
            Marques marques = service.findById(id);

            if(marques != null)
            {
                service.Delete(marques);
            }

            return RedirectToAction("Index");
        }
	}
}