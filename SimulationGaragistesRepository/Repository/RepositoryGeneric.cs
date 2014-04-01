using SimulationGaragistesDAL.Model;
using SimulationGaragistesRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using System.Data.Entity;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryGeneric<T> : IRepository<T> where T : class
    {
        protected ErrorHandler _eh;

        virtual public void Insert(T obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                context.Set<T>().Add(obj);
                context.SaveChanges();
            }
        }

        virtual public void Edit(T obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Delete(T obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                context.Entry(obj).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        virtual public List<T> findAll()
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
