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
    public class ServiceStatistiques : ServiceGeneric<Statistiques>
    {
        public ServiceStatistiques(ErrorHandler eh)
        {
            this._eh = eh;
            this._repo = new RepositoryStatistiques(this._eh);
        }

        public override void Insert(Statistiques obj)
        {
            base.Insert(obj);
        }
        public override void ValidationTest(Statistiques obj)
        {
            if (obj.garagiste_id == 0)
            {
                this._eh.addError("Pas de Garagiste");
            }
        }


    }
}
