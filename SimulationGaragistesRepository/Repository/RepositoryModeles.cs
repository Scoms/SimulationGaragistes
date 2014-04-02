using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    class RepositoryModeles : RepositoryGeneric<Modeles>
    {
        public RepositoryModeles(ErrorHandler pEh)
        {
            this._eh = pEh;
        }
    }
}
