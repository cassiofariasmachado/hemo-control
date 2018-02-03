using HemoControl.Database;
using HemoControl.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HemoControl.Models.Users;
using System;

namespace HemoControl.Controllers
{

    [Route("api/[Controller]")]
    public class UsersController : Controller
    {

        private readonly HemoControlContext _context;

        public UsersController(HemoControlContext dbContext)
        {
            this._context = dbContext;
        }

        [HttpPut]
        [Route("{email}")]
        public IActionResult Update(string email, [FromBody]UpdateUserModel model)
        {
            // To-do: Get e-mail from context when to implement authentication
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return NotFound();
            }

            user.ChangeEmail(model.Email);
            user.ChangeWeight(model.Weigth);
            user.ChangeBirthday(model.Birthday);

            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok();
        }
    }

}