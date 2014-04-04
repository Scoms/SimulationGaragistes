using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryModeles : RepositoryGeneric<Modeles>
    {
        public RepositoryModeles(ErrorHandler pEh)
        {
            this._eh = pEh;
        }

        public override void ValidationTest(Modeles obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                Modeles test = context.Modeles.Where(m => m.label == obj.label && obj.marque_id == m.Marques.id).FirstOrDefault();
                if (test != null)
                {
                    this._eh.addError("Le modèle est déjà référencé dans la marque");
                }
            }
        }

        public override void Insert(Modeles obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                this.ValidationTest(obj);
                if (!this._eh.hasErrors())
                {
                    context.Marques.Attach(obj.Marques);
                    context.Modeles.Add(obj);
                    context.SaveChanges();
                }
            }
        }

        public override Modeles findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Modeles.Include("Marques").Include("Révisions").Where(m => m.id == id).FirstOrDefault();
            }
        }
    }
}
