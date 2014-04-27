using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationGaragistes.ViewModels
{
    public class VMStatistiques
    {
        public Simulations Simulation { get; set; }

        public List<OccupationGaragiste> Occupations { get; set; }

        public class OccupationGaragiste
        {
            public Garagistes Garagiste { get; set; }
            public int JourTravailles { get; set; }
            public int Interventions { get; set; }
            public int DureeTotal { get; set; }
        }
    }
}