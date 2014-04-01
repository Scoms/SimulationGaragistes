using SimulationGaragistesDAL.Model;
using SimulationGaragistesRepository.Interfaces;
using System;
using Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryFranchises : RepositoryGeneric<Franchises>
    {
        public RepositoryFranchises(ErrorHandler eh)
        {
            this._eh = eh;
        }

        override public void Insert(Franchises pFranchise)
        {
           using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
           {
               if (pFranchise.id > 0)
               {
                   context.Entry(pFranchise).State = System.Data.Entity.EntityState.Modified;
               }
               else
               {
                   context.Franchises.Add(pFranchise);
               }
               context.SaveChanges();
           }
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

        public void Delete(Franchises pFran)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                context.Entry(pFran).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
