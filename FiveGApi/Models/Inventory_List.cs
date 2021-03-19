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
    
    public partial class Inventory_List
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> PurchasingDate { get; set; }
        public string FileName { get; set; }
        public string Project { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public string MS_RegNo { get; set; }
        public string Plot_FileNo { get; set; }
        public string FileStatus { get; set; }
        public string InventoryStatus { get; set; }
        public Nullable<decimal> TotalPlotValue { get; set; }
        public Nullable<decimal> PaidtoSocietyProjectAmount { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> PaidtoSocietyProjectMSOther { get; set; }
        public Nullable<decimal> TotalPaidtoSocietyProject { get; set; }
        public Nullable<decimal> ProfitPaid { get; set; }
        public Nullable<int> StaffPurchasing { get; set; }
        public Nullable<decimal> StaffCommissionPurchasing { get; set; }
        public Nullable<int> AgentPurchasing { get; set; }
        public Nullable<decimal> AgentCommissionPurchasing { get; set; }
        public Nullable<decimal> TotalPaid { get; set; }
        public string PaidBy { get; set; }
        public Nullable<System.DateTime> SellingDate { get; set; }
        public Nullable<decimal> SellingAmount { get; set; }
        public Nullable<decimal> GrossProfit { get; set; }
        public Nullable<int> StaffSelling { get; set; }
        public Nullable<decimal> StaffCommissionSelling { get; set; }
        public Nullable<int> AgentSelling { get; set; }
        public Nullable<decimal> AgentCommissionSelling { get; set; }
        public Nullable<decimal> NetProfit { get; set; }
        public string Remarks { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
        public Nullable<int> SecurityGroupId { get; set; }
    }
}
