using System;

namespace HemoControl.Entities
{
    public class Infusion
    {
        public int Id { get;  private set; }

        public DateTime Date { get;  private set; }

        public decimal UserWeigth { get; private set; }

        public int FactorUnity { get; private set; }

        public string FactorBrand { get; private set; }

        public string FactorLot { get; private set; }

        public bool IsHemarthrosis { get; private set; }
        
        public bool IsBleeding {get; private set;}

        public bool  IsTreatmentContinuation { get; private set; }

        public string BleedingLocal { get; private set; }

        public string TreatmentLocal { get; private set; }

        private Infusion()
        {
            // EF
        }

        public Infusion(DateTime date, decimal userWeigth, int factorUnity, string factorBrand,
            string factorLot, bool isHemarthrosis, bool isBleeding, bool isTreatmentContinuation,
            string bleedingLocal, string treatmentLocal )
        {
            this.Date = date;
            this.UserWeigth = userWeigth;
            this.FactorUnity = factorUnity;
            this.FactorBrand = factorBrand;
            this.FactorLot = factorLot;
            this.IsHemarthrosis = isHemarthrosis;
            this.IsBleeding = isBleeding;
            this.IsTreatmentContinuation = isTreatmentContinuation;
            this.BleedingLocal = bleedingLocal;
            this.TreatmentLocal = treatmentLocal;
        }
    }
}