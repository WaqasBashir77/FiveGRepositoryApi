//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiveGApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Form
    {
        public int Id { get; set; }
        public string FormUrl { get; set; }
        public Nullable<int> ModuleId { get; set; }
        public string Alias { get; set; }
        public string Icon { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> OrderBy { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Update_Date { get; set; }
        public Nullable<int> Update_By { get; set; }
    
        public virtual Module Module { get; set; }
    }
}