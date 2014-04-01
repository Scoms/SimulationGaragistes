using SimulationGaragistesRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimlulationGaragistesService.Interfaces
{
    interface IService<T>
    {
        void Insert(T obj);

        void Edit(T obj);
        void Delete(T obj);
        void ValidationTest(T obj);
        List<T> findAll();
        T findById(int Id);
    }
}
