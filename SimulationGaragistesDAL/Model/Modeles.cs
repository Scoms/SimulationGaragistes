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
    
    public partial class Modeles
    {
        public int id { get; set; }
        public string label { get; set; }
        public int marque_id { get; set; }
    
        public virtual Marques Marques { get; set; }
    }
}
