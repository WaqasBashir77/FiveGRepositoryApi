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
    
    public partial class GL_Lines
    {
        public int L_ID { get; set; }
        public Nullable<int> H_ID { get; set; }
        public Nullable<int> C_CODE { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public string Description { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
    
        public virtual GL_Headers GL_Headers { get; set; }
    }
}