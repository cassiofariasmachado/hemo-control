using HemoControl.Entities;
using Xunit;

namespace HemoControl.Test.Entities
{
    public class FactorTest
    {
        [Fact]
        public void ShouldCreate()
        {
            var factor = new Factor("Brand", 1500, "ABC-123");

            Assert.Equal("Brand", factor.Brand);
            Assert.Equal(1500, factor.Unity);
            Assert.Equal("ABC-123", factor.Lot);
        }
    }
}