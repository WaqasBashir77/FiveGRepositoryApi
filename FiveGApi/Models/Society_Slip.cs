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
    
    public partial class Society_Slip
    {
        public int ID { get; set; }
        public string Ref_num { get; set; }
        public string Form_num { get; set; }
        public string Slip_Type { get; set; }
        public string Slip_Status { get; set; }
        public string Slip_num { get; set; }
        public Nullable<System.DateTime> Slip_Date { get; set; }
        public string Letter_Status { get; set; }
        public Nullable<System.DateTime> Deliver_Date { get; set; }
        public string Deliver_Name { get; set; }
        public string Deliver_Contact { get; set; }
        public string MS_number { get; set; }
        public Nullable<decimal> Receipt_Amount { get; set; }
        public string Remarks { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_ON { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_ON { get; set; }
    }
}