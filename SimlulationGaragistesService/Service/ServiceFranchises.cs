using SimlulationGaragistesService.Interfaces;
using SimulationGaragistesDAL.Model;
using Utils;
using SimulationGaragistesRepository.Repository;
using System.Collections.Generic;

namespace SimlulationGaragistesService.Service
{
    public class ServiceFranchises : ServiceGeneric<Franchises>
    {
        public ServiceFranchises(ErrorHandler eh)
        {
            this._eh = eh;
            this._repo = new RepositoryFranchises(this._eh);
        }

        override public void ValidationTest(Franchises pFranchise)
        {
            if (pFranchise.label == null || pFranchise.label == string.Empty)
            {
                this._eh.addError("La franchise doit contenir un label");
            }

            if (((RepositoryFranchises)this._repo).findByLabel(pFranchise.label) != null)
            {
                this._eh.addError("Le label de la franchises existe déjà.");
            }
        }
        
        override public void Delete(Franchises fran)
        {
            RepositoryFranchises repo = new RepositoryFranchises(this._eh);
            repo.Delete(fran);
        }
    }
}
