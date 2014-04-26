using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositorySimulations : RepositoryGeneric<Simulations>
    {
        public RepositorySimulations(ErrorHandler eh)
        {
            this._eh = eh;
        }
    }
}
