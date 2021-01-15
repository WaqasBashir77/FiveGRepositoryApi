using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class ProjectDetailDto
    {
        public int Id { get; set; }
        public string unitNumber { get; set; }
        public string unitType { get; set; }
        public Nullable<int> childArea { get; set; }
        public string childStatus { get; set; }
        public Nullable<int> building { get; set; }
        public Nullable<int> floor { get; set; }
        public Nullable<double> unitPrice { get; set; }
        public string childDescription { get; set; }
        public string otherFeatures { get; set; }
        public Nullable<int> projectId { get; set; }
        public Nullable<int> SqFrPrice { get; set; }
        public Nullable<int> featurePrice { get; set; }
        public string floorName { get; set; }
        public string buildingName { get; set; }
    }
}