using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class PropertySaleDashboardDto
    {
        public Nullable<bool> AuthorizeStatus { get; set; }
        public string Sale_Status { get; set; }
    }
}