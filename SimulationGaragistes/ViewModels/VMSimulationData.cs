using SimulationGaragistesDAL.Model;
using SimulationGaragistesDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationGaragistes.ViewModels
{
    public class VMSimulationData
    {
        public List<VMGaragiste> lVMGaragistes { get; set; }
        public List<Voiture> lVoitures { get; set; }
        public int nbJours { get; set; }
        public string nom { get; set; }
        public DateTime debut { get; set; }
        public List<DayRepport> repports { get; set; }
        public List<Statistiques> statistiques { get; set; }
        public List<Panne> lPannes { get; set; }

        public class DayRepport
        {
            public DateTime jour { get; set; }
            public int indexJour { get; set; }
            public List<string> evenements { get; set; }
        }
    }
}