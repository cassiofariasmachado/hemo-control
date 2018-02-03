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
            this._user = new User("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", 76);
        }

        [Fact]
        public void ShouldCreate()
        {
            Assert.Equal(0, _user.Id);
            Assert.Equal("Cássio", _user.Name);
            Assert.Equal("Farias Machado", _user.LastName);
            Assert.Equal("cassiofariasmachado@yahoo.com", _user.Email);
            Assert.Equal(76, _user.Weigth);
            Assert.False(_user.Birthday.HasValue);
            Assert.Empty(_user.Infusions);
        }

        [Fact]
        public void ShouldChangeEmail()
        {
            _user.ChangeEmail("cfariasm@outlook.com");

            Assert.Equal("cfariasm@outlook.com", _user.Email);
        }

        [Fact]
        public void ShouldNotChangeEmailWhenIsEmpty()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangeEmail(""));

            Assert.Equal("Deve informar o novo e-mail", ex.Message);
        }

        [Fact]
        public void ShouldNotChangeEmailWhenIsNull()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangeEmail(null));

            Assert.Equal("Deve informar o novo e-mail", ex.Message);
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

            Assert.Equal("Peso não pode ser menor ou igual a zero", ex.Message);
        }

        [Fact]
        public void ShouldNotChangeWeightWhenIsZero()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => _user.ChangeWeight(0));

            Assert.Equal("Peso não pode ser menor ou igual a zero", ex.Message);
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
            var infusion = new Infusion(new DateTime(), _user.Weigth, 2000, "Baxter", "XXX-123",
                true, true, true, "Cotovelo D", "Casa");

            _user.AddInfusion(infusion);

            Assert.NotEmpty(_user.Infusions);
            Assert.Equal(infusion, _user.Infusions.First());
        }
    }
}