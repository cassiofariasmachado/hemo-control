using HemoControl.Entities;

namespace HemoControl.Models.Shared
{
    public class FactorResponse
    {
        public string Brand { get; set; }
        public int Unity { get; set; }
        public string Lot { get; set; }

        public static FactorResponse Map(Factor factor)
        {
            if (factor == default)
                return default;

            return new FactorResponse()
            {
                Brand = factor.Brand,
                Unity = factor.Unity,
                Lot = factor.Lot
            };
        }
    }
}