using SimulationGaragistesDAL.Model;
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
                return context.Franchises.Where(f => f.id == id).FirstOrDefault();
            }
        }
    }
}
