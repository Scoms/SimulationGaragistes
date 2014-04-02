﻿using SimulationGaragistesDAL.Model;
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

        public override List<Garagistes> findAll(List<string> pIncludes = null)
        {
 	         return base.findAll(pIncludes);
        }

        public override void Insert(Garagistes obj)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                ValidationTest(obj);
                if (!this._eh.hasErrors())
                {
                    context.Franchises.Attach(obj.Franchises);
                    context.Garagistes.Add(obj);
                    context.SaveChanges();        
                }
             }
        }

        public override void Edit(Garagistes obj, List<object> toAttach = null)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                // bof moyen -> a revoir
                ValidationTest(obj);
                if (!this._eh.hasErrors())
                {
                    obj.franchise_id = obj.Franchises.id;
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        public override Garagistes findById(int id)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                return context.Garagistes.Include("Franchises").Where(g => g.id == id).FirstOrDefault();
            }
        }
        public override void ValidationTest(Garagistes garagiste)
        {
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                Garagistes gara = context.Garagistes.Include("Franchises").Where(g => g.nom == garagiste.nom && g.Franchises.id == garagiste.Franchises.id).FirstOrDefault();
                if (gara != null)
                {
                    this._eh.addError("Un garagiste porte déjà le même nom au sein de cette franchise");
                }
            }
        }
    }
}
