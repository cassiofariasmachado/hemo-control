using System;
using HemoControl.Models.Shared;

namespace HemoControl.Models.Infusions
{
    public class UpdateInfusionRequest
    {
        public DateTime Date { get; set; }
        public FactorRequest Factor { get; set; }
        public bool IsHemarthrosis { get; set; }
        public bool IsBleeding { get; set; }
        public bool IsTreatmentContinuation { get; set; }
        public string BleedingLocal { get; set; }
        public string TreatmentLocal { get; set; }
    }
}