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
        }
    
        public int id { get; set; }
        public string nom { get; set; }
        public int franchise_id { get; set; }
    
        public virtual Franchises Franchises { get; set; }
        public virtual ICollection<Revisions_Garagistes> Revisions_Garagistes { get; set; }
        public virtual ICollection<Vacances> Vacances { get; set; }
    }
}
