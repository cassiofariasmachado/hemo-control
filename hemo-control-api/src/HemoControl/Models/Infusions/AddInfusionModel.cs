using System;

namespace HemoControl.Models.Infusions
{
    public class AddInfusionModel
    {
        public DateTime Date { get;  set; }

        public decimal UserWeigth { get; set; }

        public int FactorUnity { get; set; }

        public string FactorBrand { get; set; }

        public string FactorLot { get; set; }

        public bool IsHemarthrosis { get; set; }
        
        public bool IsBleeding {get; set;}

        public bool  IsTreatmentContinuation { get; set; }

        public string BleedingLocal { get; set; }

        public string TreatmentLocal { get; set; }
    }
}