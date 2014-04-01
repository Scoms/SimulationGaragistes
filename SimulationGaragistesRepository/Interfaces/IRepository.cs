using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationGaragistesRepository.Interfaces
{
    public interface IRepository<T>
    { 
        void Insert(T obj);
        void Edit(T obj);
        void Delete(T obj);
        List<T> findAll();
        T findById(int id);
    }
}
