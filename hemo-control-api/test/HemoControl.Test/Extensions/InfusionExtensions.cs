using System;
using System.Collections.Generic;
using System.Linq;
using HemoControl.Entities;
using HemoControl.Models.Infusions;
using Xunit;

namespace HemoControl.Test.Extensions
{
    public static class InfusionExtensions
    {
        public static void AssertRequest(this Infusion actual, AddInfusionRequest expected)
        {
            Assert.NotNull(actual);
            Assert.NotNull(expected);

            Assert.Equal(expected.Date, actual.Date);
            actual.Factor.AssertRequest(expected.Factor);
            Assert.Equal(expected.IsHemarthrosis, actual.IsHemarthrosis);
            Assert.Equal(expected.IsBleeding, actual.IsBleeding);
            Assert.Equal(expected.IsTreatmentContinuation, actual.IsTreatmentContinuation);
            Assert.Equal(expected.BleedingLocal, actual.BleedingLocal);
            Assert.Equal(expected.TreatmentLocal, actual.TreatmentLocal);
        }

        public static void AssertRequest(this Infusion actual, UpdateInfusionRequest expected)
        {
            Assert.NotNull(actual);
            Assert.NotNull(expected);

            Assert.Equal(expected.Date, actual.Date);
            actual.Factor.AssertRequest(expected.Factor);
            Assert.Equal(expected.IsHemarthrosis, actual.IsHemarthrosis);
            Assert.Equal(expected.IsBleeding, actual.IsBleeding);
            Assert.Equal(expected.IsTreatmentContinuation, actual.IsTreatmentContinuation);
            Assert.Equal(expected.BleedingLocal, actual.BleedingLocal);
            Assert.Equal(expected.TreatmentLocal, actual.TreatmentLocal);
        }

        public static void AssertResponse(this Infusion actual, InfusionResponse expected)
        {
            Assert.NotNull(actual);
            Assert.NotNull(expected);

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Date, actual.Date);
            Assert.Equal(expected.UserWeigth, actual.UserWeigth);
            actual.Factor.AssertResponse(expected.Factor);
            Assert.Equal(expected.IsHemarthrosis, actual.IsHemarthrosis);
            Assert.Equal(expected.IsBleeding, actual.IsBleeding);
            Assert.Equal(expected.IsTreatmentContinuation, actual.IsTreatmentContinuation);
            Assert.Equal(expected.BleedingLocal, actual.BleedingLocal);
            Assert.Equal(expected.TreatmentLocal, actual.TreatmentLocal);
            actual.User.AssertResponse(expected.User);
        }

        public static void AssertResponse(this IEnumerable<Infusion> actualInfusions, IEnumerable<InfusionResponse> expectedInfusions)
        {
            Assert.Collection(actualInfusions,
                expectedInfusions
                    .Select<InfusionResponse, Action<Infusion>>(
                        infusionResponse =>
                            infusion => infusion.AssertResponse(infusionResponse))
                    .ToArray()
            );
        }
    }
}