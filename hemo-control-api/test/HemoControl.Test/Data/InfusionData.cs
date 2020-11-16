using System;
using System.Collections.Generic;
using HemoControl.Entities;

namespace HemoControl.Test.Data
{
    public static class InfusionData
    {
        public static List<Infusion> Infusions = new List<Infusion>
        {
            new Infusion(DateTime.Now, new Factor("Brand", 1000, "Lot"), true, false, false, "BleedingLocal", "TreatmentLocal", UserData.User),
            new Infusion(DateTime.Now.AddMonths(-10), new Factor("Brand", 2000, "Lot"), true, false, true, "BleedingLocal", "TreatmentLocal", UserData.User)
        };
    }
}