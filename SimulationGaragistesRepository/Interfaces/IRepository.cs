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
        void ValidationTest(T obj);
        List<T> findAll(List<string> lIncludes);
        T findById(int id);
    }
}
