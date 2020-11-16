using System;
using HemoControl.Entities;
using HemoControl.Models.Shared;
using HemoControl.Models.Users;

namespace HemoControl.Models.Infusions
{
    public class InfusionResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal? UserWeigth { get; set; }
        public FactorResponse Factor { get; set; }
        public bool IsHemarthrosis { get; set; }
        public bool IsBleeding { get; set; }
        public bool IsTreatmentContinuation { get; set; }
        public string BleedingLocal { get; set; }
        public string TreatmentLocal { get; set; }
        public UserResponse User { get; set; }

        public static InfusionResponse Map(Infusion infusion)
        {
            if (infusion == default)
                return default;

            return new InfusionResponse
            {
                Id = infusion.Id,
                Date = infusion.Date,
                UserWeigth = infusion.UserWeigth,
                Factor = FactorResponse.Map(infusion.Factor),
                IsHemarthrosis = infusion.IsHemarthrosis,
                IsBleeding = infusion.IsBleeding,
                IsTreatmentContinuation = infusion.IsTreatmentContinuation,
                BleedingLocal = infusion.BleedingLocal,
                TreatmentLocal = infusion.TreatmentLocal,
                User = UserResponse.Map(infusion.User)
            };
        }
    }
}