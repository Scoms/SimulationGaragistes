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

        public void ChangeDureeRevision(Revisions_Garagistes revGar)
        {
            if(revGar.revision_id == -1 || revGar.garagiste_id == -1)
            {
                this._eh.addError("Bizarre ..");
            }
            if (revGar.duree <= 0)
            {
                this._eh.addError("Durée non valide");
            }
            if(!this._eh.hasErrors())
            {
                ((RepositoryGaragistes)this._repo).ChangeDureeRevision(revGar);
            }
        }

        public List<Révisions> GetRevisions(int id)
        {
           return ((RepositoryGaragistes)this._repo).GetRevisions(id);
        }
    }
}
