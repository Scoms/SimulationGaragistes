using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryMarques : RepositoryGeneric<Marques>
    {
        public RepositoryMarques(ErrorHandler pEh)
        {
            this._eh = pEh;
        }

        override public void ValidationTest(Marques obj)
        {
            using(SimulationGaragistesEntities context = new SimulationGaragistesEntities())
	        {
                Marques test = null;
                test = context.Marques.Where(m => m.label.Equals(obj.label)).FirstOrDefault();
                if (test != null)
                {
                    _eh.addError("Cette marque existe déjà");
                }
	        }
        }

        public override Marques findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Marques.Where(m => m.id == id).FirstOrDefault();
            }
        }
    }
}
