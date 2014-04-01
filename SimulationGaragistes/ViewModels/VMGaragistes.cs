using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimulationGaragistes.ViewModels
{
    public class VMGaragistes
    {
        public Garagistes Garagiste { get; set; }
        public List<SelectListItem> lFranchises { get; set; }
    }
}