using HemoControl.Services;
using Xunit;

namespace HemoControl.Test.Services
{
    public class PasswordServiceTest
    {
        [Theory]
        [InlineData("12345678", "$2a$11$TEKOyTo3QvE0pAooMyfeNu1sg0G1pHK2Czd/a.nBVk0j3YodLoem2")]
        [InlineData("q1w2e3r4", "$2a$11$wPRjjZFTe/ZB.UZODQeGZeoWo1va2JF92xpGWhMCcaMDooRothcCW")]
        public void VerifyPasswordShouldVerifyCorrectly(string plainPassword, string hashedPassword)
            => Assert.True(new PasswordService().Verify(plainPassword, hashedPassword));

        [Theory]
        [InlineData("12345678")]
        [InlineData("q1w2e3r4")]
        public void HashPasswordShouldHashCorrectly(string plainPassword)
        {
            var hashedPassword = new PasswordService().HashPassword(plainPassword);

            Assert.StartsWith("$2a$11", hashedPassword);
            Assert.Equal(60, hashedPassword.Length);
            Assert.NotEqual(plainPassword, hashedPassword);
        }
    }
}