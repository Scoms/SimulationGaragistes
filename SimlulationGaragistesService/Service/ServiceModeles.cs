using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimlulationGaragistesService.Service
{
    public class ServiceModeles : ServiceGeneric<Modeles>
    {
        public ServiceModeles(ErrorHandler pEh)
        {
            this._eh = pEh;
        }
    }
}
