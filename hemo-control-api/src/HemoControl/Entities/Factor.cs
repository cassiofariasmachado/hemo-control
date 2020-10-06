namespace HemoControl.Entities
{
    public class Factor
    {
        public string Brand { get; private set; }
        public int Unity { get; private set; }
        public string Lot { get; private set; }

        public Factor(string brand, int unity, string lot)
        {
            Brand = brand;
            Unity = unity;
            Lot = lot;
        }
    }
}