using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class PropertySaleNewDTO
    {
        public string nomineeGContact { get; set; }

        public int Booking_ID { get; set; }
        public Nullable<int> Project_ID { get; set; }
        public string Project_Name { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public ProjectUnitDetailsDTO unit_Detail { get; set; }
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
        public byte[] Nominee_Picture { get; set; }
        public Nullable<bool> AuthorizeStatus { get; set; }
        public string Nominee_Father_Name { get; set; }
        public virtual ICollection<SaleInstallment> SaleInstallments { get; set; }
        public string BuyerMemberCode { get; set; }
    }
}