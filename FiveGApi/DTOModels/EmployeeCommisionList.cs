using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class EmployeeCommisionList
    {
        public string Name { get; set; }
        public decimal? totalBookingCommisson { get; set; }
        public decimal? totalConfirmCommisson { get; set; }
        public decimal? totalCommisson { get; set; }
        public int totalNoBookings { get; set; }
        public int totalNoCommision { get; set; }

    }
}