using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class PropertyDashboardModel
    {
        public string PropertyName  { get; set; }
        public int PropertyTotalCount { get; set; }
        public decimal? PropertySaleVlaue { get; set; }
        public decimal? TotalPercentage { get; set; }


    }
    public class CommisionReciveableDashboard
    {
        public string SocietyName { get; set; }
        public int PropertyBooking { get; set; }
        public decimal? BookingCommision { get; set; }
        public int PropertyConfimr { get; set; }
        public decimal? ConfimrCommision { get; set; }
        public decimal? TotalCommision { get; set; }
    }
  
}