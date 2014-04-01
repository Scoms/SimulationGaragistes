using SimulationGaragistesDAL.Model;
using SimulationGaragistesRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryGaragistes : RepositoryGeneric<Garagistes>
    {
        public RepositoryGaragistes(ErrorHandler pEh)
        {
            this._eh = pEh;
        }
    }
}
