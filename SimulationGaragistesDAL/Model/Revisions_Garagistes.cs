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
    
    public partial class Revisions_Garagistes
    {
        public int garagiste_id { get; set; }
        public int revision_id { get; set; }
        public int duree { get; set; }
    
        public virtual Garagistes Garagistes { get; set; }
        public virtual Révisions Révisions { get; set; }
    }
}