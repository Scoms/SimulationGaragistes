using SimulationGaragistesDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationGaragistesDAL.Model
{
    public class Creneau
    {
        public int Jour { get; set; }
        public int Heure { get; set; }

        internal void maj(int indexJour, int duree, out Creneau debut, VMGaragiste VMgaragiste)
        {
            debut = new Creneau();
            debut.Jour = this.Jour;
            debut.Heure = this.Heure;
            //Si première resa
            if (this.Jour < indexJour)
            {
                this.Jour = indexJour;
            }
            this.Heure += duree;
            // TEst sur le + que 8
            while (this.Heure > 8)
            {
                this.Jour++;
                if (!VMgaragiste.estEnVacances(this.Jour))
                {
                    this.Heure -= 8;
                }
            }
        }
    }
}
