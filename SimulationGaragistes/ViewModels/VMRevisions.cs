using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationGaragistes.ViewModels
{
    public class VMRevisions
    {
        public List<Révisions> lRevisions { get; set; }
        public int idGaragiste { get; set; }
    }
}