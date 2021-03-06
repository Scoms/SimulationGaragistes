﻿using SimulationGaragistesDAL.ViewModel;
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
        public List<Panne> lPannes { get; set; }

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
            this.lPannes = new List<Panne>();
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

        public string Roule(DateTime date, int indexJour,List<VMGaragiste> lVMGaragistes, out Statistiques pStat, int jourMAX)
        {
            // Init
            pStat = new Statistiques();
            string repport = String.Empty;
            int min, max = 0;
            int dayKm = 0;
            VMIntervention vm = new VMIntervention();
            vm.VMGaragiste = new VMGaragiste(new Garagistes());

            //Determine le nombre de km à parcourir le jour
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
            dayKm = this.rand.Next(min,max);

            //On regarde si panne le jour même 

            //si la voiture est arrétée
            if (nbJoursArrets == jourMAX)
            {
                repport = String.Format("{0} ne sera pas réparée par manque de disponibilité des garagistes", this.ToString());
            }
            else if(nbJoursArrets > 0)
            {
                repport = String.Format("{0} n'a pas roulée, en réparation jusqu'au jour {1}", this.ToString(), indexJour + nbJoursArrets - 1);
                nbJoursArrets--;
            }
            else
            {
                //La voiture est arretée 
                if (nbJoursArrets == -1)
                {
                    repport = String.Format("{0} n'a pas roulée, en attente de reservation pour une réparation", this.ToString());
                    nbJoursArrets = 0;
                    vm = this.chercherIntervention(this.prochaineRevision, indexJour, lVMGaragistes);
                }
                else
	            {
                    //Une chance sur 10 
                    if (this.rand.Next(1,10) == 1)
                    {
                        List<Révisions> lRev;
                        //test 
                        using (SimulationGaragistesEntities context = new SimulationGaragistesEntities())
                        {
                            lRev = context.Révisions.Where(r => r.modele_id == null).ToList();
                        }

                        Révisions rev = lRev.Skip(this.rand.Next(0,lRev.Count())).First();
                        this.prochaineRevision = rev;
                    }
                    if (this.prochaineRevision.km <= this.km + dayKm)
                    {
                        this.km = (int)this.prochaineRevision.km;
                        vm = this.chercherIntervention(this.prochaineRevision, indexJour, lVMGaragistes);
                    }
                    else
                    {
                        this.km += dayKm;
                        repport = String.Format("{0} à roulée {1} km ({2})", this, dayKm, this.km);
                        return repport;
                    }
	            }

                pStat = vm.Stat;
                if (vm.VMGaragiste.Garagiste.id == 0)
                {
                    repport = String.Format("Pas de garagiste disponile pour {0}", this);
                    this.nbJoursArrets = -1;
                }
                else
                {
                    if (vm.Fin.Jour > jourMAX)
                    {
                        repport = String.Format("{0} ne sera pas réparée par manque de disponibilité des garagistes",this.ToString());
                        this.nbJoursArrets = jourMAX;
                        pStat = null;
                    }
                    else
                    {
                        repport = String.Format("{0} répare {1}  du {2} - {3}H, jusqu'au {4} - {5}H {6}", vm.VMGaragiste, this, vm.Debut.Jour, vm.Debut.Heure, vm.Fin.Jour, vm.Fin.Heure,this.prochaineRevision.label);
                        this.nbJoursArrets = vm.Fin.Jour - indexJour;
                        this.prochaineRevision = this.getProchaineRevision();
                    }
                }
            }
            return repport;
        }

        private VMIntervention reparePanne(Panne panne, int indexJour, List<VMGaragiste> lVMGaragistes)
        {
            throw new NotImplementedException();
        }

        private VMIntervention chercherIntervention(Révisions revision,int indexJour,List<VMGaragiste> lVMGaragistes)
        {
            VMIntervention vmInter = new VMIntervention();
            vmInter.Fin = new Creneau();
            vmInter.Debut = new Creneau();
            Creneau courant = new Creneau();
            Creneau plusTot = new Creneau();
            Statistiques stat = new Statistiques();
            VMGaragiste garagisteChoisi = new VMGaragiste(new Garagistes());

            foreach (VMGaragiste vmGaragiste in lVMGaragistes)
            {
                courant = vmGaragiste.getProchaineDispo(indexJour);

                //Le garagiste n'accepte personne si il est plein
                if(courant.Jour == indexJour)
                {
                    if (plusTot.Jour == 0)
                    {
                        plusTot = courant;
                        garagisteChoisi = vmGaragiste;
                    }
                    else
                    {
                        if (courant.Heure < plusTot.Heure)
                        {
                            plusTot = courant;
                            garagisteChoisi = vmGaragiste;
                        }
                    }
                }
            }

            if (garagisteChoisi.Garagiste.id != 0)
            {
                Creneau Debut;
                garagisteChoisi.reserveJour(indexJour, revision, out Debut, out stat);
                vmInter.Debut = Debut;
            }
            vmInter.VMGaragiste = garagisteChoisi;
            vmInter.JoursArrets = garagisteChoisi.getProchaineDispo(indexJour).Heure == 1 ? garagisteChoisi.getProchaineDispo(indexJour).Jour - indexJour : 0;
            vmInter.Fin = garagisteChoisi.getProchaineDispo(indexJour);
            stat.modele_id = this.modele.id;
            stat.voiture = this.modele.label + " (" + this.id + ")";
            vmInter.Stat = stat;
            
            return vmInter;
        }

        public override string ToString()
        {
            return this.modele.Marques.label + " " + this.modele.label + "(" + this.id + ")";
        }
    }
}
