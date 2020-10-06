using Microsoft.AspNetCore.Mvc;
using HemoControl.Entities;
using HemoControl.Database;
using HemoControl.Models.Infusions;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using HemoControl.Models.Errors;

namespace HemoControl.Controllers
{
    [Controller]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class InfusionsController : Controller
    {
        private readonly HemoControlContext _context;

        public InfusionsController(HemoControlContext dbContext)
        {
            _context = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddInfusionAsync([FromBody] AddInfusionRequest request, CancellationToken cancellationToken)
        {
            var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(c => c.Username == username);

            var factor = new Factor(request.Factor.Brand, request.Factor.Unity, request.Factor.Lot);
            var infusion = new Infusion(
                request.Date,
                factor,
                request.IsHemarthrosis,
                request.IsBleeding,
                request.IsTreatmentContinuation,
                request.BleedingLocal,
                request.TreatmentLocal,
                user
            );

            user.AddInfusion(infusion);

            await _context.AddAsync(infusion, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInfusionAsync([FromRoute] int id, [FromBody] AddInfusionRequest request, CancellationToken cancellationToken)
        {
            var infusion = _context.Infusions.FirstOrDefault(i => i.Id == id);
            if (infusion == default)
                return BadRequest(new ErrorResponse { Message = "Infusion not found" });

            infusion.ChangeDate(request.Date);
            infusion.ChangeFactor(new Factor(request.Factor.Brand, request.Factor.Unity, request.Factor.Lot));
            infusion.ChangeIsHemarthrosis(request.IsHemarthrosis);
            infusion.ChangeIsTreatmentContinuation(request.IsTreatmentContinuation);
            infusion.ChangeIsBleeding(request.IsBleeding);
            infusion.ChangeBleedingLocal(request.BleedingLocal);
            infusion.ChangeTreatmentLocal(request.TreatmentLocal);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveInfusionAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var infusion = _context.Infusions.FirstOrDefault(i => i.Id == id);

            if (infusion == default)
                return BadRequest(new ErrorResponse { Message = "Infusion not found" });

            _context.Remove(infusion);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}