using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SimulationGaragistesRepository.Repository
{
    public class RepositoryStatistiques : RepositoryGeneric<Statistiques>
    {
        public RepositoryStatistiques(ErrorHandler eh)
        {
            this._eh = eh;
        }

        public override void Insert(Statistiques obj)
        {
            base.Insert(obj);
        }
    }
}
