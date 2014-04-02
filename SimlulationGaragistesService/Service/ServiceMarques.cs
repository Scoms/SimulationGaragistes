using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimlulationGaragistesService.Service
{
    class ServiceMarques : ServiceGeneric<Marques>
    {
        public ServiceMarques()
        {
            this._eh = new ErrorHandler();
        }
    }
}
