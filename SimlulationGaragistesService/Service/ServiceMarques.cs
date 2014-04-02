﻿using SimulationGaragistesDAL.Model;
using SimulationGaragistesRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimlulationGaragistesService.Service
{
    public class ServiceMarques : ServiceGeneric<Marques>
    {
        public ServiceMarques(ErrorHandler pEh)
        {
            this._eh = pEh;
            this._repo = new RepositoryMarques(this._eh);
        }

        public override void Insert(Marques obj)
        {
            
        }
    }
}
