using HemoControl.Entities;
using HemoControl.Models.Shared;
using Xunit;

namespace HemoControl.Test.Extensions
{
    public static class FactorExtensions
    {
        public static void AssertResponse(this Factor actual, FactorResponse expected)
        {
            Assert.NotNull(actual);
            Assert.NotNull(expected);

            Assert.Equal(expected.Brand, actual.Brand);
            Assert.Equal(expected.Lot, actual.Lot);
            Assert.Equal(expected.Unity, actual.Unity);
        }

        public static void AssertRequest(this Factor actual, FactorRequest expected)
        {
            Assert.NotNull(actual);
            Assert.NotNull(expected);

            Assert.Equal(expected.Brand, actual.Brand);
            Assert.Equal(expected.Lot, actual.Lot);
            Assert.Equal(expected.Unity, actual.Unity);
        }
    }
}