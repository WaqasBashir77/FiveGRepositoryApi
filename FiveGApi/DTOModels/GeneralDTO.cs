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
        public DateTime Created_Date { get; set; }
        public DateTime Update_Date { get; set; }
        public int? Created_By { get; set; }
        public int? Update_By { get; set; }
        public int UserId { get; set; }
        public int? SecurityGroupId { get; set; }
    }
}