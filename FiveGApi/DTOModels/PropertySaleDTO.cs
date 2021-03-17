using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class PropertySaleDTO
    {
        public int ID { get; set; }
        public int unitId { get; set; }
        public int projectId { get; set; }
        public int mobile_1 { get; set; }
        public int mobile_2 { get; set; }
        public int memberRegNo { get; set; }
        public int discountAmount { get; set; }
        public int employeeId { get; set; }
        public decimal employeeCommission { get; set; }
        public decimal dealerCommission { get; set; }
        public int dealerId { get; set; }
        public string buyerName { get; set; }
        public string buyerFatherName { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string relationWithNomine { get; set; }
        public string saleStatus { get; set; }
        public string nomineeName { get; set; }
        public string nomineeCnic { get; set; }
        public int nomineeContact { get; set; }
        public string cnic { get; set; }
        public string nomineeGNumber{ get; set; }
        public string Purchaser_Picture { get; set; }
        public string PaymentCode { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Update_Date { get; set; }
        public short? Created_By { get; set; }
        public int? Update_By { get; set; }
        public int? SecurityGroupId { get; set; }
        public decimal differentiableAmount { get; set; }
        public string Nominee_Picture { get; set; }
        public List<TempTableForInstallmentDTO> propertySaleDetails { get; set; }
       
    }
}