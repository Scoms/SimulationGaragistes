using SimlulationGaragistesService.Interfaces;
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
    public class ServiceGaragistes : ServiceGeneric<Garagistes>
    {
        public ServiceGaragistes(ErrorHandler pEh)
        {
            this._eh = pEh;
            this._repo = new RepositoryGaragistes(this._eh);
        }

        public override void ValidationTest(Garagistes obj)
        {
            if (obj.nom == null)
            {
                this._eh.addError("Le garagiste doit avoir un nom, comme toute personne normal d'ailleurs.");
            }
        }
    }
}
