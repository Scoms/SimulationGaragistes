using SimlulationGaragistesService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimulationGaragistes.Controllers
{
    public class MarquesController : Controller
    {
        //
        // GET: /Marques/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Insert()
        {
            //ServiceMarques service = new ServiceMarques();
            return View();
        }
	}
}