﻿using SimulationGaragistesDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationGaragistesDAL.ViewModel
{
    public class VMGaragiste
    {
        public Garagistes Garagiste { get; set; }
        public List<int> IndexVacances {get;set;}
        
        public Creneau ProchaineDispo;

        public VMGaragiste(Garagistes pGaragiste)
        {
            //this.Garagiste.Revisions_Garagistes = new HashSet<Revisions_Garagistes>();
            //this.Garagiste.Vacances = new HashSet<Vacances>();
            this.ProchaineDispo = new Creneau();
            this.ProchaineDispo.Jour = 0;
            this.ProchaineDispo.Heure = 1;
            this.IndexVacances = new List<int>();
            this.Garagiste = pGaragiste;
        }


        // indexJour, heure

        public Creneau getProchaineDispo(int indexJour)
        {
            if (ProchaineDispo.Jour >= indexJour)
            {
                return this.ProchaineDispo;
            }

            while (this.estEnVacances(indexJour))
            {
                indexJour++;
            }
            this.ProchaineDispo.Jour = indexJour;
            this.ProchaineDispo.Heure = 1;
            return this.ProchaineDispo;
        }

        public void activerVacances(DateTime debutSimu,int nbJours)
        {
            //Vacances du monsieur
            foreach (var item in this.Garagiste.Vacances)
            {
                var dif1 = item.debut.Date - debutSimu.Date;
                var dif2 = item.fin.Date - debutSimu.Date;

                for (int i = dif1.Days; i < dif2.Days; i++)
                {
                    this.IndexVacances.Add(i+1);
                }
            }

            //Jour non travaillés de manière générale
            for (int i = 0; i < nbJours; i++)
            {
                DayOfWeek dayOfWeek = debutSimu.AddDays(i).DayOfWeek;
                if(dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday)
                {
                    this.IndexVacances.Add(i+1);
                }
            }
        }

        public bool estEnVacances(int indexJour)
        {
            return this.IndexVacances.Contains(indexJour);
        }

        public void reserveJour(int indexJour,Révisions revision, out Creneau debut, out Statistiques stat)
        {
            stat = new Statistiques();
            stat.garagiste_id = this.Garagiste.id;
            stat.revision_id = revision.id;
            stat.garagiste = this.ToString();
            stat.revision = revision.label;
            stat.km = (int)revision.km;
            
            
            int duree = revision.defaultTime;
            foreach (var item in this.Garagiste.Revisions_Garagistes)
            {
                if (item.revision_id == revision.id)
                {
                    duree = item.duree;
                }
            }
            stat.duree = duree;
            this.ProchaineDispo.maj(indexJour,duree,out debut,this);
        }

        public override string ToString()
        {
            return this.Garagiste.nom + "(" + this.Garagiste.Franchises.label + ")";
        }
    }
}
