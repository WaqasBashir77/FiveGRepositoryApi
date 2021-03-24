using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class ProjectListDTO
    {
        public int Id { get; set; }
        public string projectCode { get; set; }
        public string projectName { get; set; }
        public string projectType { get; set; }
        public string address { get; set; }
        public Nullable<int> city { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public Nullable<double> totalArea { get; set; }
        public Nullable<int> unit { get; set; }
        public string noc { get; set; }
        public string projectCurrency { get; set; }
        public string location { get; set; }
        public Nullable<bool> PaymentPlanStatus { get; set; }
        public Nullable<int> SecurityGroupId { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Update_Date { get; set; }
        public Nullable<int> Update_By { get; set; }
        public string Company { get; set; }
        public string LocationSeg { get; set; }
        public string ProjectSeg { get; set; }
        public string PlotAres { get; set; }
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }

    }
}