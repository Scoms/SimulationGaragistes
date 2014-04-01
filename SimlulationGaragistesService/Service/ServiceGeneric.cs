using SimlulationGaragistesService.Interfaces;
using SimulationGaragistesRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimlulationGaragistesService.Service
{
    public class ServiceGeneric<T> : IService<T> where T : class
    {
        protected ErrorHandler _eh;
        protected RepositoryGeneric<T> _repo;

        public virtual void Insert(T obj)
        {
            this.ValidationTest(obj);

            if (!this._eh.hasErrors())
            {
                this._repo.Insert(obj);
            }
        }
        public virtual void Edit(T obj)
        {
            this.ValidationTest(obj);

            if (!this._eh.hasErrors())
            {
                this._repo.Edit(obj);
            }
        }

        virtual public void Delete(T obj)
        {
            this._repo.Delete(obj);
        }

        public List<T> findAll()
        {
            return this._repo.findAll();
        }

        virtual public void ValidationTest(T obj)
        {
            throw new NotImplementedException();
        }

        public T findById(int Id)
        {
            return this._repo.findById(Id);
        }
    }
}
