using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimulationGaragistes.ViewModels
{
    public class VMModeles
    {
        public Modeles Modele { get; set; }
        public String marqueLabel { get; set; }
        public List<SelectListItem> Marques { get; set; }
        public List<Révisions> Revisions { get; set; }
        public Révisions Revision { get; set; }
    }
}