using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryRévisions : RepositoryGeneric<Révisions>
    {
        public RepositoryRévisions(ErrorHandler eh)
        {
            this._eh = eh;
        }


        public override Révisions findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Révisions.Include("Modeles").Include("Modeles.Marques").Where(m => m.id == id).FirstOrDefault();
            }
        }

        public override List<Révisions> findAll(List<string> pIncludes = null)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                List<Révisions> res = context.Révisions.Include("Modeles").Include("Modeles.Marques").ToList();
                return res;
            }
        }

        public override void ValidationTest(Révisions obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                Révisions test = context.Révisions.Where(m => m.label.Equals(obj.label) && m.km == obj.km).FirstOrDefault(); 
                if (test != null && test.id != obj.id)
                {
                    this._eh.addError("La révision demandée existe déjà");
                }
            }
        }

        public override void Insert(Révisions obj)
        {
            base.Insert(obj);
        }

    }
}
