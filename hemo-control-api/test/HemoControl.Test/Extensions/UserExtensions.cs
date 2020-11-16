using HemoControl.Entities;
using HemoControl.Models.Users;
using Xunit;

namespace HemoControl.Test.Extensions
{
    public static class UserExtensions
    {
        public static void AssertResponse(this User actual, UserResponse expected)
        {
            if (expected == default && actual == default)
                return;

            Assert.NotNull(expected);
            Assert.NotNull(actual);

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Birthday, actual.Birthday);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Weigth, actual.Weigth);
        }

        public static void AssertRequest(this User actual, RegisterUserRequest expected)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Username, actual.Username);
            Assert.NotEqual(expected.Password, actual.Password);
        }
    }
}