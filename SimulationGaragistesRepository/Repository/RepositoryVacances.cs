using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryVacances : RepositoryGeneric<Vacances>
    {
        private RepositoryVacances _repo;
        public RepositoryVacances(ErrorHandler eh)
        {
            this._eh = eh;
        }

        public override void Insert(Vacances obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                RepositoryGaragistes repoGaragiste = new RepositoryGaragistes(this._eh);
                Garagistes garagiste = repoGaragiste.findById(obj.garagiste_id);
                obj.Garagistes = garagiste;

                List<Vacances> lVacances = garagiste.Vacances.ToList();

                foreach (var item in lVacances)
                {
                    if (item.debut <= obj.debut && obj.debut <= item.fin)
                    {
                        this._eh.addError("La date de début se trouve pendant des vacances");
                    }
                    if (item.debut <= obj.fin && obj.fin <= item.fin)
                    {
                        this._eh.addError("La date de fin se trouve pendant des vacances");
                    }
                    if (obj.debut <= item.debut && item.debut <= obj.fin)
                    {
                        this._eh.addError("Les vacances spécifiées en englobe d'autres");
                    }
                    if (obj.debut >= obj.fin)
                    {
                        this._eh.addError("Les dates ne sont pas cohérentes");
                    }
                }

                if (!_eh.hasErrors())
                {
                    context.Garagistes.Attach(garagiste);
                    context.Vacances.Add(obj);
                    context.SaveChanges();
                }
            }
        }

        public override void Edit(Vacances obj, List<object> toAttach = null)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                RepositoryGaragistes repoGaragiste = new RepositoryGaragistes(this._eh);
                Garagistes garagiste = repoGaragiste.findById(obj.garagiste_id);

                List<Vacances> lVacances = garagiste.Vacances == null ? null : garagiste.Vacances.ToList();

                foreach (var item in lVacances)
                {
                    if (item.debut <= obj.debut || obj.debut <= obj.fin)
                    {
                        this._eh.addError("La date de début se trouve pendant des vacances");
                    }
                    if (item.debut <= obj.fin || obj.fin <= obj.fin)
                    {
                        this._eh.addError("La date de fin se trouve pendant des vacances");
                    }
                    if (obj.debut <= item.debut || item.debut <= obj.fin)
                    {
                        this._eh.addError("Les vacances spécifiées en englode d'autres.");
                    }
                }

                context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public override void Delete(Vacances obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public override Vacances findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Vacances.Where(v => v.id == id).FirstOrDefault();
            }
        }
    }
}
