//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimulationGaragistesDAL.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Garagistes
    {
        public Garagistes()
        {
            this.Revisions_Garagistes = new HashSet<Revisions_Garagistes>();
            this.Vacances = new HashSet<Vacances>();
            this.ProchaineDispo = new Creneau();
            this.ProchaineDispo.Jour = 1;
            this.ProchaineDispo.Heure = 1;
        }

    
        public int id { get; set; }
        public string nom { get; set; }
        public int franchise_id { get; set; }
    
        public virtual Franchises Franchises { get; set; }
        public virtual ICollection<Revisions_Garagistes> Revisions_Garagistes { get; set; }
        public virtual ICollection<Vacances> Vacances { get; set; }

        //Champ supplémentaire [indexJour,Heure,Durée]
        public virtual List<int[]> Agenda {get;set;}
        public Creneau ProchaineDispo { get;set;}

        // indexJour, heure
        public class Creneau
        {
            public int Jour { get; set; }
            public int Heure { get; set; }

            internal void maj(int indexJour, int duree)
            {
                //Si première resa
                if (this.Jour < indexJour)
                {
                    this.Jour = indexJour;
                }
                this.Heure += duree;
                // TEst sur le + que 8     
            }
        }
    
        internal void reserveJour(int indexJour,Révisions revision)
        {
            int duree = revision.defaultTime;
            foreach (var item in this.Revisions_Garagistes)
            {
                if (item.revision_id == revision.id)
                {
                    duree = item.duree;
                }
            }
            this.ProchaineDispo.maj(indexJour,duree);
        }

        public override string ToString()
        {
            return this.nom;
        }
    }
}
