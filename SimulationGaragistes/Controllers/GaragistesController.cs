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
            ServiceGaragistes service = new ServiceGaragistes(new ErrorHandler());
            List<Garagistes> lGaragistes = service.findAll();
            return View(lGaragistes);
        }

        public ActionResult Insert(int id = -1)
        {
            //Init
            VMGaragistes vmGaragiste = new VMGaragistes();
            ServiceGaragistes serviceG = new ServiceGaragistes(new ErrorHandler());
            ServiceFranchises serviceF = new ServiceFranchises(new ErrorHandler());
            Garagistes garagiste = new Garagistes();
            List<Franchises> lFranchises = serviceF.findAll();
            vmGaragiste.lFranchises = new List<SelectListItem>();
            vmGaragiste.Garagiste = garagiste;
            foreach (Franchises franchise in lFranchises)
            {
                vmGaragiste.lFranchises.Add(new SelectListItem(){ Text = franchise.label, Value = franchise.id.ToString()});   
            }

            if (id != -1)
            {

            }

            if (this.Request.HttpMethod == Enums.HttpMethod.POST.ToString())
            {
                serviceG.Insert(garagiste);
            }

            return View(vmGaragiste);
        }
    }
}