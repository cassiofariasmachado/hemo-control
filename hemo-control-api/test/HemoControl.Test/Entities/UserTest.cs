using Xunit;
using HemoControl.Entities;
using System;
using System.Linq;

namespace HemoControl.Test.Entities
{
    public class UserTest
    {
        private readonly User _user;

        public UserTest()
        {
            _user = new User("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", "cassiofariasmachado", "12345678");
        }

        [Fact]
        public void ShouldCreate()
        {
            Assert.Equal(0, _user.Id);
            Assert.Equal("Cássio", _user.Name);
            Assert.Equal("Farias Machado", _user.LastName);
            Assert.Equal("cassiofariasmachado@yahoo.com", _user.Email);
            Assert.Equal("cassiofariasmachado", _user.Username);
            Assert.NotEmpty(_user.Password);
            Assert.Empty(_user.Infusions);
        }

        [Fact]
        public void ShouldChangeEmail()
        {
            var user = new User("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", "cassiofariasmachado", "12345678");

            user.ChangeEmail("cfariasm@outlook.com");

            Assert.Equal("cfariasm@outlook.com", user.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotChangeEmailWhenIsNullOrWhitespace(string email)
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangeEmail(email));

            Assert.Equal("Email is required", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotChangeUsernameWhenIsNullOrWhitespace(string username)
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangeUsername(username));

            Assert.Equal("Username is required", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldNotChangePasswordWhenIsNullOrWhitespace(string password)
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangePassword(password));

            Assert.Equal("Password is required", ex.Message);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("ABCDEFG")]
        public void ShouldNotChangePasswordWhenIsLessThanEight(string password)
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangePassword(password));

            Assert.Equal("Password length must be greater than 8", ex.Message);
        }

        [Fact]
        public void ShouldChangeWeight()
        {
            _user.ChangeWeight(100);

            Assert.Equal(100, _user.Weigth);
        }

        [Fact]
        public void ShouldNotChangeWeightWhenIsANegativeNumber()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangeWeight(-11));

            Assert.Equal("Weight must be greater than zero", ex.Message);
        }

        [Fact]
        public void ShouldNotChangeWeightWhenIsZero()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangeWeight(0));

            Assert.Equal("Weight must be greater than zero", ex.Message);
        }

        [Fact]
        public void ShouldChangeBirthday()
        {
            var birthday = new DateTime(1996, 1, 18);

            _user.ChangeBirthday(birthday);

            Assert.Equal(birthday, _user.Birthday);
        }

        [Fact]
        public void AddInfusion()
        {
            var infusion = new Infusion(new DateTime(), _user.Weigth.GetValueOrDefault(), 2000, "Baxter", "XXX-123",
                true, true, true, "Cotovelo D", "Casa");

            _user.AddInfusion(infusion);

            Assert.NotEmpty(_user.Infusions);
            Assert.Equal(infusion, _user.Infusions.First());
        }
    }
}