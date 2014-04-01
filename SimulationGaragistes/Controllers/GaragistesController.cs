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
            ErrorHandler eh = new ErrorHandler();
            VMGaragistes vmGaragiste = new VMGaragistes();
            ServiceGaragistes serviceG = new ServiceGaragistes(eh);
            ServiceFranchises serviceF = new ServiceFranchises(eh);
            Garagistes garagiste = new Garagistes();
            List<Franchises> listFranchises = serviceF.findAll();

            //Get garagiste
            if(id != -1)
            {
                garagiste = serviceG.findById(id);
            }

            if (garagiste == null)
            {
                garagiste = new Garagistes();
            }

            if (this.Request.HttpMethod == Enums.HttpMethod.POST.ToString())
            {
                garagiste.nom = this.Request.Form["nom"];
                //garagiste.franchise_id = int.Parse(this.Request.Form["franchise"]);
                garagiste.Franchises = listFranchises.Find(l => l.id == int.Parse(this.Request.Form["franchise"]));

                if (garagiste.id == 0)
                {
                    serviceG.Insert(garagiste);
                }
                else
                {
                    serviceG.Edit(garagiste);
                }

                if(eh.hasErrors())
                {
                    ModelState.AddModelError("error", eh.getErrors());
                }
                else
                {
                    if (garagiste.id != id)
                    {
                        TempData["success"] = "Le garagiste a bien été crée";
                        garagiste = new Garagistes();
                    }
                    else
                    {
                        TempData["success"] = "Le garadiste a bien été modifié";
                    }
                }
            }

            //Populate the VM
            vmGaragiste.lFranchises = new List<SelectListItem>();
            
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
    }
}