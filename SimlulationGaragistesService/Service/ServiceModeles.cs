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
    public class ServiceModeles : ServiceGeneric<Modeles>
    {
        public ServiceModeles(ErrorHandler pEh)
        {
            this._eh = pEh;
            this._repo = new RepositoryModeles(this._eh);
        }

        public override void ValidationTest(Modeles obj)
        {
            if (obj.label == null)
            {
                this._eh.addError("Le modèle doit être identifié par un label");
            }
        }
    }
}
