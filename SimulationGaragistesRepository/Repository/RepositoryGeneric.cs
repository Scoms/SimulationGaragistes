using SimulationGaragistesRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using SimulationGaragistesDAL.Model;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryGeneric<T> : IRepository<T> where T : class
    {
        protected ErrorHandler _eh;

        virtual public void Insert(T obj)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public List<T> findAll()
        {
            using(SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Set<T>().ToList();
            }
        }

        virtual public T findById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
