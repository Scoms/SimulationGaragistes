using SimulationGaragistesDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationGaragistesDAL.Model
{
    public class Voiture
    {
        private int MIN_KM = 20000;
        private int MAX_KM = 200000;
        private int MIN_WEND = 50;
        private int MAX_WEND = 100;
        private int MIN_WEEK = 20;
        private int MAX_WEEK = 50;
        private Random rand;
        private Révisions prochaineRevision;
        private int SLEEPTIME = 20;

        public int id { get; set; }
        public int km { get; set; }
        public Modeles modele { get; set; }

        public Voiture(Modeles pModele, int pId)
        {
            this.id = pId;
            this.modele = pModele;

            this.rand = new Random();
            Thread.Sleep(SLEEPTIME);
            this.km = this.rand.Next(MIN_KM, MAX_KM);
            this.prochaineRevision = this.getProchaineRevision();
            
        }

        public Révisions getProchaineRevision()
        {
            Révisions res = new Révisions();
            bool revisionFounded = false;
            List<Révisions> lRevisions = this.modele.Révisions.OrderBy(r => r.km).ToList();
            foreach (Révisions revision in lRevisions)
            {
                if (!revisionFounded && revision.km > this.km)
                {
                    res = revision;
                    revisionFounded = true;
                }
            }
            return res;
        }

        public string Roule(DateTime date, int indexJour)
        {
            // Week end (20-50) Semaine (50-100)
            string repport = String.Empty;
            int min, max = 0;
            int dayKm = 0;
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                min = MIN_WEND;
                max = MAX_WEND;
            }
            else
            {
                min = MIN_WEEK;
                max = MAX_WEEK;
            }

            //Thread.Sleep(SLEEPTIME);
            dayKm = this.rand.Next(min,max);

            if(this.prochaineRevision.km <= this.km + dayKm)
            {
                this.km = (int)this.prochaineRevision.km;
                VMIntervention vm = this.reserverIntervention(this.prochaineRevision,indexJour);
                repport = String.Format("{0} répare {1}  du {2} - {3}H, jusqu'au {4} - {5}H", vm.Garagiste,this,vm.Debut.Jour,vm.Debut.Heure,vm.Fin.Jour,vm.Fin.Heure);
                this.prochaineRevision = this.getProchaineRevision();
            }
            else
            {
                this.km += dayKm;
                repport = String.Format("{0} a roulée {1} kms ({2})", this.ToString(), dayKm, this.km);
            }
            return repport;
        }

        private VMIntervention reserverIntervention(Révisions revision,int indexJour)
        {
            VMIntervention vmInter = new VMIntervention();
            vmInter.Fin = new Garagistes.Creneau();
            vmInter.Debut = new Garagistes.Creneau();
            using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
            {
                bool estReserve = false;
                List<Garagistes> lGaragistes = context.Garagistes.ToList();
                foreach (Garagistes garagiste in lGaragistes)
                {
                    if (!estReserve)
                    {
                        // si le garagiste est libre ce jour
                        if (garagiste.ProchaineDispo.Jour < indexJour)
                        {
                            garagiste.reserveJour(indexJour,revision);
                            vmInter.Garagiste = garagiste;
                            vmInter.JoursArrets = garagiste.ProchaineDispo.Heure == 1 ? garagiste.ProchaineDispo.Jour - indexJour : 0;
                            vmInter.Fin = garagiste.ProchaineDispo;

                            vmInter.Debut = vmInter.Debut;
                            estReserve = true;
                        }
                    }
                }
            }
            return vmInter;
        }

        public override string ToString()
        {
            return this.modele.Marques.label + " " + this.modele.label + "(" + this.id + ")";
        }
    }
}
