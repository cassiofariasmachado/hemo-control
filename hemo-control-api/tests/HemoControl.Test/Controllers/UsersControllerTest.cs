using Microsoft.EntityFrameworkCore;
using HemoControl.Controllers;
using HemoControl.Database;
using HemoControl.Models.Users;
using HemoControl.Entities;
using HemoControl.Test.Utils;
using Xunit;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace HemoControl.Test.Controllers
{
    public class UsersControllerTest
    {
        public UsersControllerTest()
        {
        }

        // To-do: Update when to implement authentication
        [Fact]
        public void ShouldUpdateUser()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>("Update");
            var model = new UpdateUserModel
            {
                Email = "cfariasm@gmail.com",
                Weigth = 100,
                Birthday = new DateTime(1996, 9, 18)
            };
            IActionResult result;

            using (var context = new HemoControlContext(options))
            {
                context.Users.Add(new User("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", 78));
                context.SaveChanges();
            }

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context);
                result = controller.Update("cassiofariasmachado@yahoo.com", model);
            }

            using (var context = new HemoControlContext(options))
            {
                var user = context.Users.FirstOrDefault(u => u.Id == 1);
                Assert.Equal("cfariasm@gmail.com", user.Email);
                Assert.Equal(new DateTime(1996, 9, 18), user.Birthday);
                Assert.Equal(100, user.Weigth);
                Assert.IsType<OkResult>(result);
            }
        }
    }
}