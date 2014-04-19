using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationGaragistes.ViewModels
{
    public class VMSimulation
    {
        public DateTime dateStart { get; set; }
        public int nbJours { get; set; }
        public List<GaragistesConf> GaragistesConfs { get; set; }
        public List<ModelesConf> ModelesConfs { get; set; }

        public class GaragistesConf
        {
            public GaragistesConf(Garagistes pGaragiste, bool confirmed)
            {
                this.Garagiste = pGaragiste;
                this.Comfirmed = confirmed;
            }
            public Garagistes Garagiste { get; set; }
            public bool Comfirmed { get; set; }
        }

        public class ModelesConf
        {
            public ModelesConf(Modeles pModele, int pQuant)
            {
                this.Modele = pModele;
                this.Quantite = pQuant;
            }
            public Modeles Modele { get; set; }
            public int Quantite { get; set; }
        }
    }
}