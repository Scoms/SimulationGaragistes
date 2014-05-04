using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationGaragistesDAL.Model
{
    public class Panne
    {
        public Voiture Voiture { get; set; }
        public int IndexJour { get; set; }
        public int Duree { get; set; }
        public string Label { get; set; }
    }
}
