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
    
    public partial class PaymentInstallment
    {
        public int Payment_ID { get; set; }
        public Nullable<int> Ins_ID { get; set; }
        public Nullable<int> Booking_ID { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public Nullable<int> Project_ID { get; set; }
        public string Instrument_Type { get; set; }
        public Nullable<double> Payment_amount { get; set; }
        public string instrument_number { get; set; }
        public string instrument_bank { get; set; }
        public string instrument_bank_Branch { get; set; }
        public Nullable<System.DateTime> instrument_date { get; set; }
        public string instrument_remarks { get; set; }
    
        public virtual SaleInstallment SaleInstallment { get; set; }
    }
}
