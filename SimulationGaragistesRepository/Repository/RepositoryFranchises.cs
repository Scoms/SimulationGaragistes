using SimulationGaragistesDAL.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryFranchises : RepositoryGeneric<Franchises>
    {
        public RepositoryFranchises(ErrorHandler eh)
        {
            this._eh = eh;
        }

        public Franchises findByLabel(string label)
        {
            using (SimulationGaragistesEntities context  = new SimulationGaragistesEntities())
            {
                return context.Franchises.Where(f => f.label.ToUpper().Equals(label.ToUpper())).FirstOrDefault();
            }
        }

        override public Franchises findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Franchises.Include("Garagistes").Where(f => f.id == id).FirstOrDefault();
            }
        }

        public override void Delete(Franchises obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                List<Garagistes> lG = obj.Garagistes.ToList();
                foreach (Garagistes garagiste in lG)
                {
                    context.Entry(garagiste).State = EntityState.Deleted;
                }
                context.SaveChanges();
                context.Entry(obj).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
