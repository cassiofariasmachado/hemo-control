using System;

namespace HemoControl.Entities
{
    public class Infusion
    {
        public int Id { get; private set; }

        public DateTime Date { get; private set; }

        public decimal? UserWeigth { get; private set; }

        public Factor Factor { get; private set; }

        public bool IsHemarthrosis { get; private set; }

        public bool IsBleeding { get; private set; }

        public bool IsTreatmentContinuation { get; private set; }

        public string BleedingLocal { get; private set; }

        public string TreatmentLocal { get; private set; }

        public User User { get; private set; }

        private Infusion() { }

        public Infusion(
            DateTime date,
            Factor factor,
            bool isHemarthrosis,
            bool isBleeding,
            bool isTreatmentContinuation,
            string bleedingLocal,
            string treatmentLocal,
            User user
        )
        {
            Date = date;
            UserWeigth = user.Weigth;
            Factor = factor;
            IsHemarthrosis = isHemarthrosis;
            IsBleeding = isBleeding;
            IsTreatmentContinuation = isTreatmentContinuation;
            BleedingLocal = bleedingLocal;
            TreatmentLocal = treatmentLocal;
            User = user;
        }

        public void ChangeDate(DateTime date)
        {
            Date = date;
        }

        public void ChangeFactor(Factor factor)
        {
            Factor = factor;
        }

        public void ChangeIsHemarthrosis(bool isHemarthrosis)
        {
            IsHemarthrosis = isHemarthrosis;
        }

        public void ChangeIsTreatmentContinuation(bool isTreatmentContinuation)
        {
            IsTreatmentContinuation = isTreatmentContinuation;
        }

        public void ChangeIsBleeding(bool isBleeding)
        {
            IsBleeding = isBleeding;
        }

        public void ChangeBleedingLocal(string bleedingLocal)
        {
            BleedingLocal = bleedingLocal;
        }

        public void ChangeTreatmentLocal(string treatmentLocal)
        {
            TreatmentLocal = treatmentLocal;
        }
    }
}