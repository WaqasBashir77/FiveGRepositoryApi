using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class ProjectUnitDetailsDTO
    {
        public int unit_ID { get; set; }
        public string unitNumber { get; set; }
        public string  UnitType { get; set; }
        public int? unitBuilding { get; set; }
        public int? Unitfloor { get; set; }
        public string UnitStatus { get; set; }

    }
}