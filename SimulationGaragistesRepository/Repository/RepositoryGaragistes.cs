using SimulationGaragistesDAL.Model;
using SimulationGaragistesRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using System.Data.Entity;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryGaragistes : RepositoryGeneric<Garagistes>
    {
        public RepositoryGaragistes(ErrorHandler pEh)
        {
            this._eh = pEh;
        }

        public override List<Garagistes> findAll()
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Garagistes.Include("Franchises").ToList();
            }
        }

        public override void Insert(Garagistes obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                context.Franchises.Attach(obj.Franchises);
                context.Garagistes.Add(obj);
                context.SaveChanges();
            }
        }

        public override void Edit(Garagistes obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                //context.Franchises.Attach(obj.Franchises);
                //context.SaveChanges();
                obj.franchise_id = obj.Franchises.id;
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public override Garagistes findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Garagistes.Include("Franchises").Where(g => g.id == id).FirstOrDefault();
            }
        }
    }
}
