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
    
    public partial class Delivery_Sheet
    {
        public int ID { get; set; }
        public string Ref_num { get; set; }
        public string Form_num { get; set; }
        public string Delivery_Type { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public string Delivery_To { get; set; }
        public string Handover_Staff { get; set; }
        public string Handover_Date { get; set; }
        public Nullable<int> Payment_ID { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
        public string Remarks { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
    }
}
