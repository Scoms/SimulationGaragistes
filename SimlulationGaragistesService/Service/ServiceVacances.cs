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
    public class ServiceVacances : ServiceGeneric<Vacances>
    {
        private DateTime defaultDate = new DateTime();
        public ServiceVacances(ErrorHandler eh)
        {
            this._eh = eh;
            this._repo = new RepositoryVacances(this._eh);
        }

        public override void ValidationTest(Vacances vacance)
        {
            if (vacance.debut == defaultDate || vacance.fin == defaultDate)
            {
                this._eh.addError("Date(s) invalide(s)");
            }
            if (vacance.debut >= vacance.fin)
            {
                this._eh.addError("La date début doit être antérieur à la date de fin");
            }
        }
    }
}
