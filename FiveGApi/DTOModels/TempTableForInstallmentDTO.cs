using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class TempTableForInstallmentDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string InstallmentType { get; set; }
        public string Installment { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> Percentage { get; set; }
       
        public int Tax { get; set; }
        public int amount { get; set; }
        public int TaxAmount { get; set; }
        public int latesurchargeAmount { get; set; }
        public int totalAmount { get; set; }
        public string dueDate { get; set; }
        public string paymentStatus { get; set; }
        public double balance { get; set; }
        public decimal OtherTaxAmount { get; set; }
        public int Payment_Account { get; set; }
        //public int Updated_By { get; set; }
        public PaymentDetailDTO paymentDetailDTOs { get; set; }
    }
}