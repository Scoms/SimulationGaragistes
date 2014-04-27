using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositorySimulations : RepositoryGeneric<Simulations>
    {
        public RepositorySimulations(ErrorHandler eh)
        {
            this._eh = eh;
        }

        public override Simulations findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Simulations.Include("Statistiques").Include("Statistiques.Révisions").Include("Statistiques.Garagistes").Include("Statistiques.Garagistes.Franchises").Include("Statistiques.Garagistes.Vacances").Where(m => m.id == id).FirstOrDefault();
            }
        }
    }
}
