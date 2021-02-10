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
    
    public partial class PropertySale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertySale()
        {
            this.SaleInstallments = new HashSet<SaleInstallment>();
        }
    
        public int Booking_ID { get; set; }
        public Nullable<int> Project_ID { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public string Buyer_Name { get; set; }
        public string Buyer_Father_Name { get; set; }
        public string Mobile_1 { get; set; }
        public string Mobile_2 { get; set; }
        public string Member_Reg_No { get; set; }
        public byte[] Purchaser_Picture { get; set; }
        public Nullable<double> Dealer_Comm { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Relationship_With_Nominee { get; set; }
        public string Sale_Status { get; set; }
        public string Nominee_Name { get; set; }
        public string Nominee_CNIC { get; set; }
        public string CNIC { get; set; }
        public string Nominee_G_Number { get; set; }
        public Nullable<int> Discount_Amount { get; set; }
        public string Description { get; set; }
        public Nullable<int> Employee { get; set; }
        public Nullable<double> Employee_Com { get; set; }
        public Nullable<int> Dealer_ID { get; set; }
        public Nullable<System.DateTime> Created_ON { get; set; }
        public Nullable<int> Updated_By { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
        public string PaymentCode { get; set; }
        public Nullable<int> SecurityGroupId { get; set; }
        public Nullable<decimal> differentiableAmount { get; set; }
        public string Nominee_Picture { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleInstallment> SaleInstallments { get; set; }
    }
}
