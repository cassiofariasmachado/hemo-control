using System;
using HemoControl.Entities;
using Xunit;

namespace HemoControl.Test.Entities
{
    public class InfusionTest
    {
        [Fact]
        public void ShouldCreate()
        {
            var user = new User("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", "cassiofariasmachado", "12345678");
            var factor = new Factor("Baxter", 2000, "ABC-123");

            var infusion = new Infusion(
                DateTime.Now,
                factor,
                true,
                true,
                true,
                "Cotovelo D",
                "Casa",
                user
            );

            Assert.Equal("Baxter", infusion.Factor.Brand);
            Assert.Equal(2000, infusion.Factor.Unity);
            Assert.Equal("ABC-123", infusion.Factor.Lot);
            Assert.True(infusion.IsBleeding);
            Assert.True(infusion.IsHemarthrosis);
            Assert.True(infusion.IsTreatmentContinuation);
            Assert.Equal("Cotovelo D", infusion.BleedingLocal);
            Assert.Equal("Casa", infusion.TreatmentLocal);
        }
    }
}