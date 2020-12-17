using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class GeneralDTO
    {
        public int Id { get; set; }
        public int projectId { get; set; }
        public int unitId { get; set; }
        public int insId { get; set; }
        public int bookingId { get; set; }
    }
}