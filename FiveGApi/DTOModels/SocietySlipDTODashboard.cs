using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class SocietySlipDTODashboard
    {
        public string Name { get; set; }
        public int totalSlips { get; set; }
        public int totalPendingSlips { get; set; }
        public int totalDeliveredSlips { get; set; }
        public int totalBookingSlips { get; set; }
        public int totalConfirmSlips { get; set; }
        public decimal? totalBookingSlipsAmount { get; set; }
        public decimal? totalConfirmationSlipsAmount { get; set; }
    }
}