using Microsoft.AspNetCore.Mvc;
using HemoControl.Entities;
using HemoControl.Database;
using HemoControl.Models.Infusions;
using System.Linq;

namespace HemoControl.Controllers
{
    [Route("api/[Controller]")]
    public class InfusionsController : Controller
    {
        private readonly HemoControlContext _context;

        public InfusionsController(HemoControlContext dbContext)
        {
            this._context = dbContext;
        }

        [HttpPost]
        [Route("{email}")]
        public IActionResult AddInfusion(string email, [FromBody]AddInfusionModel model) 
        {
            var user = this._context.Users.FirstOrDefault(u => u.Email == email);
            var infusion = new Infusion(model.Date, model.UserWeigth, model.FactorUnity, model.FactorBrand, 
                model.FactorLot, model.IsHemarthrosis, model.IsBleeding, model.IsTreatmentContinuation, model.BleedingLocal, model.TreatmentLocal);
            user.AddInfusion(infusion);

            return Ok();
        }


    }
}