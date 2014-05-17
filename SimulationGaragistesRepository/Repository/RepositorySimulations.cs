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

        public override void Insert(Simulations obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                Simulations oldSimu = context.Simulations.Where(s => s.nom.Equals(obj.nom)).FirstOrDefault();
                if (oldSimu != null)
                {
                    context.Entry(oldSimu).State = System.Data.Entity.EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            base.Insert(obj);
        }

        public override Simulations findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Simulations.Include("Statistiques").Where(m => m.id == id).FirstOrDefault();
            }
        }
    }
}
