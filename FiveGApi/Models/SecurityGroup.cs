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
    
    public partial class SecurityGroup
    {
        public int Id { get; set; }
        public string Group_Name { get; set; }
        public Nullable<int> Parent_Id { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Update_Date { get; set; }
        public Nullable<int> Update_By { get; set; }
    }
}