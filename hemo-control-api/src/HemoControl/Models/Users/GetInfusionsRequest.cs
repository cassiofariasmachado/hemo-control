using System;
using HemoControl.Models.Shared;

namespace HemoControl.Models.Users
{
    public class GetInfusionsRequest : PagedRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}