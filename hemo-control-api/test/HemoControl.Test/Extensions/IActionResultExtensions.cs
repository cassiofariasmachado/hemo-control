using System;
using HemoControl.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HemoControl.Test.Extensions
{
    public static class IActionResultExtensions
    {
        public static void AssertIsBadRequestObjectResult<T>(this IActionResult actionResult, Action<T> assert)
            where T : class
            => actionResult.AssertIsObjectResult<BadRequestObjectResult, T>(assert);

        public static void AssertIsNotFoundObjectResult<T>(this IActionResult actionResult, Action<T> assert)
            where T : class
            => actionResult.AssertIsObjectResult<NotFoundObjectResult, T>(assert);

        public static void AssertIsOkObjectResult<T>(this IActionResult actionResult, Action<T> assert)
            where T : class
            => actionResult.AssertIsObjectResult<OkObjectResult, T>(assert);

        public static void AssertIsOkResult(this IActionResult actionResult)
            => actionResult.AssertIsResult<OkResult>();

        public static void AssertIsCreatedResult(this IActionResult actionResult)
            => actionResult.AssertIsResult<CreatedResult>();

        private static void AssertIsObjectResult<TObjectResult, TResult>(this IActionResult actionResult, Action<TResult> assert)
            where TObjectResult : ObjectResult
            where TResult : class
        {
            Assert.NotNull(actionResult);
            var result = Assert.IsType<TObjectResult>(actionResult);

            var resultObject = result.Value as TResult;
            assert?.Invoke(resultObject);
        }

        private static void AssertIsResult<TObjectResult>(this IActionResult actionResult)
            where TObjectResult : ActionResult
        {
            Assert.NotNull(actionResult);
            Assert.IsType<TObjectResult>(actionResult);
        }
    }
}