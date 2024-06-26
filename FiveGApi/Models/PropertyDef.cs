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
    
    public partial class PropertyDef
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Society { get; set; }
        public string Category { get; set; }
        public string Plot_Size { get; set; }
        public string Dimensions { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Block { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> Booking_percent { get; set; }
        public Nullable<decimal> Confirm_percent { get; set; }
        public Nullable<decimal> Rebate_percent { get; set; }
        public Nullable<decimal> Tax_percent { get; set; }
        public Nullable<int> Total_Files { get; set; }
        public Nullable<System.DateTime> Booking_Start { get; set; }
        public Nullable<System.DateTime> Booking_End { get; set; }
        public Nullable<int> GL_Mapping_ID { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_ON { get; set; }
        public string Updated_By { get; set; }
        public string Updated_On { get; set; }
    }
}
