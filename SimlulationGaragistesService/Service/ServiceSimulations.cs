using SimulationGaragistesDAL.Model;
using SimulationGaragistesRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimlulationGaragistesService.Service
{
    public class ServiceSimulations : ServiceGeneric<Simulations>
    {
        public ServiceSimulations(ErrorHandler eh)
        {
            this._eh = eh;
            this._repo = new RepositorySimulations(this._eh);
        }
    }
}
