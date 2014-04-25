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

        private int nbJoursArrets = 0;

        public Voiture(Modeles pModele, int pId)
        {
            this.id = pId;
            this.modele = pModele;

            this.rand = new Random();
            Thread.Sleep(SLEEPTIME);
            this.km = this.rand.Next(MIN_KM, MAX_KM);
            this.prochaineRevision = this.getProchaineRevision();   
        }

        public Voiture(Modeles pModele, int pId, int km)
        {
            this.id = pId;
            this.modele = pModele;

            this.rand = new Random();
            Thread.Sleep(SLEEPTIME);
            this.km = km;
            this.prochaineRevision = this.getProchaineRevision(); 
        }

        public Révisions getProchaineRevision()
        {
            Révisions res = new Révisions();
            bool revisionFounded = false;
            if (this.modele.Révisions != null)
            {
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
            res.km = -1;
            return res;
        }

        public string Roule(DateTime date, int indexJour,List<Garagistes> lGaragistes)
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

            //si la voiture est arrété
            if(nbJoursArrets > 0)
            {
                repport = String.Format("{0} n'a pas roulée, en réparation jusqu'au jour {1}", this.ToString(), indexJour + nbJoursArrets);
                nbJoursArrets--;
            }
            else
            {
                if (nbJoursArrets == -1)
                {
                    repport = String.Format("{0} n'a pas roulée, en attente de reservation pour une réparation", this.ToString());
                    nbJoursArrets = 0;
                    VMIntervention vm = this.reserverIntervention(this.prochaineRevision, indexJour, lGaragistes);
                    if (vm.Garagiste.id == 0)
                    {
                        repport = String.Format("Pas de garagiste disponile pour {0}", this);
                        this.nbJoursArrets = -1;
                    }
                    else
                    {
                        repport = String.Format("{0} répare {1}  du {2} - {3}H, jusqu'au {4} - {5}H", vm.Garagiste, this, vm.Debut.Jour, vm.Debut.Heure, vm.Fin.Jour, vm.Fin.Heure);
                        this.nbJoursArrets = vm.Fin.Jour - indexJour;

                    }
                }
                else
                {
                    if (this.prochaineRevision.km <= this.km + dayKm)
                    {
                        this.km = (int)this.prochaineRevision.km;
                        VMIntervention vm = this.reserverIntervention(this.prochaineRevision, indexJour, lGaragistes);

                        if (vm.Garagiste.id == 0)
                        {
                            repport = String.Format("Pas de garagiste disponile pour {0}", this);
                            this.nbJoursArrets = -1;
                        }
                        else
                        {
                            repport = String.Format("{0} répare {1}  du {2} - {3}H, jusqu'au {4} - {5}H", vm.Garagiste, this, vm.Debut.Jour, vm.Debut.Heure, vm.Fin.Jour, vm.Fin.Heure);
                            this.nbJoursArrets = vm.Fin.Jour - indexJour;

                        }
                        this.prochaineRevision = this.getProchaineRevision();
                    }
                    else
                    {
                        this.km += dayKm;
                        repport = String.Format("{0} a roulée {1} kms ({2})", this.ToString(), dayKm, this.km);
                    }
                }
            }
            return repport;
        }

        private VMIntervention reserverIntervention(Révisions revision,int indexJour,List<Garagistes> lGaragistes)
        {
            VMIntervention vmInter = new VMIntervention();
            vmInter.Fin = new Garagistes.Creneau();
            vmInter.Debut = new Garagistes.Creneau();
            Garagistes.Creneau plusTot = new Garagistes.Creneau();
            plusTot.Heure = -1;

           
            bool founded = false;
            Garagistes garagisteChoisi = new Garagistes();
            foreach (Garagistes garagiste in lGaragistes)
            {
                //Si le garagiste n'est pas en vacances
                if(!garagiste.estEnVacances(indexJour))
                {
                    if (!founded)
                    {
                        //Si premier passage dans la boucle 
                        if (plusTot.Heure == -1)
                        {
                            garagisteChoisi = garagiste;
                            plusTot = garagiste.getProchaineDispo();
                            //Si premier RDV de la journée, pas mieux 
                            if (garagiste.getProchaineDispo().Jour < indexJour)
                            {
                                plusTot.Jour = indexJour;
                                founded = true;
                            }
                        }
                        else
                        {
                            // Si le jour est plus tot ou équal
                            if (garagiste.getProchaineDispo().Jour <= plusTot.Jour)
                            {
                                //Si l'heure est plus tot -> meilleur cas 
                                if (garagiste.getProchaineDispo().Heure < plusTot.Heure)
                                {
                                    garagisteChoisi = garagiste;
                                    plusTot = garagiste.getProchaineDispo();
                                    if (garagiste.getProchaineDispo().Jour < indexJour)
                                    {
                                        plusTot.Jour = indexJour;
                                        founded = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            SimulationGaragistesDAL.Model.Garagistes.Creneau Debut;
            garagisteChoisi.reserveJour(indexJour, revision, out Debut);
            vmInter.Garagiste = garagisteChoisi;
            vmInter.JoursArrets = garagisteChoisi.getProchaineDispo().Heure == 1 ? garagisteChoisi.getProchaineDispo().Jour - indexJour : 0;
            vmInter.Fin = garagisteChoisi.getProchaineDispo();
            vmInter.Debut = Debut;
            
            return vmInter;
        }

        public override string ToString()
        {
            return this.modele.Marques.label + " " + this.modele.label + "(" + this.id + ")";
        }
    }
}
