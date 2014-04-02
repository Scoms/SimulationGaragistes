using SimulationGaragistesDAL.Model;
using SimulationGaragistesRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryGeneric<T> : IRepository<T> where T : class
    {
        protected ErrorHandler _eh;

        virtual public void Insert(T obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                this.ValidationTest(obj);
                if (!this._eh.hasErrors())
                {
                    context.Set<T>().Add(obj);
                    context.SaveChanges();        
                }
            }
        }

        virtual public void Edit(T obj, List<object> toAttach = null)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                this.ValidationTest(obj);
                if (!this._eh.hasErrors())
                {

                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
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

        virtual public List<T> findAll(List<String> pIncludes = null)
        {
            using(SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                IQueryable<T> query = null;  
                query = context.Set<T>();
                if(pIncludes != null)
                {
                    foreach (var param in pIncludes)
                    {
                        query = query.Include(param);
                    }
                }
                var result = query.ToList();
                return result;
            }
        }

        virtual public T findById(int id)
        {
            throw new NotImplementedException();
        }

        virtual public void ValidationTest(T obj)
        {
            //could be nothing 
        }
    }
}
