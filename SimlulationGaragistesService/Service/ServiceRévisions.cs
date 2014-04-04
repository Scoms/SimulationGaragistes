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
    public class ServiceRévisions : ServiceGeneric<Révisions>
    {
        public ServiceRévisions(ErrorHandler eh)
        {
            this._eh = eh;
            this._repo = new RepositoryRévisions(eh);
        }

        public override void ValidationTest(Révisions obj)
        {
            if(obj.label == null)
            {
                this._eh.addError("La révision doit comporter un label");
            }
            else if (obj.label.Trim().Equals(String.Empty))
            {
                this._eh.addError("La révision doit comporter un label valide");
            }
            if (obj.defaultTime <= 0 || obj.defaultTime == null)
            {
                this._eh.addError("La durée de la révision est invalide");
            }
            if (obj.km <= 0 || obj.km == null)
            {
                this._eh.addError("Le nombre de km est invalide");
            }
        }
    }
}
