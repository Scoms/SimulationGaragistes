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
        public VMGaragiste VMGaragiste { get; set; }
        public Creneau Debut { get; set; }
        public Creneau Fin { get; set; }
        public int JoursArrets { get; set; }
        public Statistiques Stat { get; set; }
    }
}
