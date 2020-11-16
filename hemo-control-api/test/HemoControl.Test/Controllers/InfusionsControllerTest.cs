using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using HemoControl.Controllers;
using HemoControl.Database;
using HemoControl.Models.Infusions;
using HemoControl.Models.Shared;
using HemoControl.Services;
using HemoControl.Test.Data;
using HemoControl.Test.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.EntityFrameworkCore;
using HemoControl.Models.Errors;
using HemoControl.Test.Extensions;

namespace HemoControl.Test.Controllers
{
    public class InfusionsControllerTest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InfusionsControllerTest()
        {
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();
            _httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            _httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();
        }

        [Fact]
        public async Task AddInfusionShouldAddInfusionCorrectly()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(AddInfusionShouldAddInfusionCorrectly));

            var identity = AccessTokenService.CreateIdentity("cassiofariasmachado");

            A.CallTo(() => _httpContextAccessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(identity));

            var request = new AddInfusionRequest
            {
                Date = DateTime.Now,
                Factor = new FactorRequest { Brand = "Brand", Lot = "1234", Unity = 1000 },
                IsHemarthrosis = true,
                IsBleeding = false,
                IsTreatmentContinuation = false,
                BleedingLocal = "BleedingLocal",
                TreatmentLocal = "TreatmentLocal"
            };

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.SaveChangesAsync();
            }

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new InfusionsController(context, _httpContextAccessor);

                response = await controller.AddInfusionAsync(request, default(CancellationToken));
            }

            response.AssertIsOkObjectResult<InfusionResponse>(async infusionResponse =>
            {
                using (var context = new HemoControlContext(options))
                {
                    var infusion = await context.Infusions
                        .Include(i => i.User)
                        .FirstOrDefaultAsync(i => i.Id == infusionResponse.Id);

                    infusion.AssertRequest(request);
                    infusion.AssertResponse(infusionResponse);
                }
            });
        }

        [Fact]
        public async Task UpdateInfusionShouldUpdateInfusionCorrectly()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(UpdateInfusionShouldUpdateInfusionCorrectly));

            var request = new UpdateInfusionRequest
            {
                Date = DateTime.Now.AddMonths(2),
                Factor = new FactorRequest { Brand = "NewBrand", Lot = "NewLot-1234", Unity = 1000 },
                IsHemarthrosis = false,
                IsBleeding = true,
                IsTreatmentContinuation = false,
                BleedingLocal = "NewBleedingLocal",
                TreatmentLocal = "NewTreatmentLocal"
            };

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.AddRangeAsync(InfusionData.Infusions);
                await context.SaveChangesAsync();
            }

            var infusionId = InfusionData.Infusions.Select(i => i.Id).FirstOrDefault();
            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new InfusionsController(context, _httpContextAccessor);

                response = await controller.UpdateInfusionAsync(infusionId, request, default(CancellationToken));
            }

            response.AssertIsOkObjectResult<InfusionResponse>(async infusionResponse =>
            {
                using (var context = new HemoControlContext(options))
                {
                    var infusion = await context.Infusions
                        .FirstOrDefaultAsync(i => i.Id == infusionId);

                    infusion.AssertRequest(request);
                    infusion.AssertResponse(infusionResponse);
                }
            });
        }

        [Fact]
        public async Task UpdateInfusionShouldReturnsBadRequestWhenInfusionNotExists()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(UpdateInfusionShouldReturnsBadRequestWhenInfusionNotExists));

            var request = new UpdateInfusionRequest
            {
                Date = DateTime.Now.AddMonths(2),
                Factor = new FactorRequest { Brand = "NewBrand", Lot = "NewLot-1234", Unity = 1000 },
                IsHemarthrosis = false,
                IsBleeding = true,
                IsTreatmentContinuation = false,
                BleedingLocal = "NewBleedingLocal",
                TreatmentLocal = "NewTreatmentLocal"
            };

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.AddRangeAsync(InfusionData.Infusions);
                await context.SaveChangesAsync();
            }

            using (var context = new HemoControlContext(options))
            {
                var controller = new InfusionsController(context, _httpContextAccessor);

                var response = await controller.UpdateInfusionAsync(999, request, default(CancellationToken));

                response.AssertIsBadRequestObjectResult<ErrorResponse>(errorResponse =>
                {
                    Assert.Equal("Infusion not found", errorResponse.Message);
                });
            }
        }

        [Fact]
        public async Task RemoveInfusionShouldRemoveInfusionCorrectly()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(RemoveInfusionShouldRemoveInfusionCorrectly));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.AddRangeAsync(InfusionData.Infusions);
                await context.SaveChangesAsync();
            }

            var infusionId = InfusionData.Infusions.Select(i => i.Id).FirstOrDefault();

            using (var context = new HemoControlContext(options))
            {
                var controller = new InfusionsController(context, _httpContextAccessor);

                var response = await controller.RemoveInfusionAsync(infusionId, default(CancellationToken));

                response.AssertIsOkResult();
            }

            using (var context = new HemoControlContext(options))
            {
                var existsInfusion = await context.Infusions.AnyAsync(i => i.Id == infusionId);

                Assert.False(existsInfusion);
            }
        }

        [Fact]
        public async Task RemoveInfusionReturnsBadRequestWhenInfusionNotExists()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(RemoveInfusionReturnsBadRequestWhenInfusionNotExists));

            using (var context = new HemoControlContext(options))
            {
                var controller = new InfusionsController(context, _httpContextAccessor);

                var response = await controller.RemoveInfusionAsync(999, default(CancellationToken));

                response.AssertIsBadRequestObjectResult<ErrorResponse>(response =>
                {
                    Assert.Equal("Infusion not found", response.Message);
                });
            }
        }
    }
}