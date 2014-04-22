using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationGaragistesDAL.ViewModel
{
    public class VMIntervention
    {
        public Garagistes Garagiste { get; set; }
        public SimulationGaragistesDAL.Model.Garagistes.Creneau Debut { get; set; }
        public SimulationGaragistesDAL.Model.Garagistes.Creneau Fin { get; set; }
        public int JoursArrets { get; set; }
    }
}
