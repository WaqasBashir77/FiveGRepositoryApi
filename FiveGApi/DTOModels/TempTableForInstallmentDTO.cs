using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class TempTableForInstallmentDTO
    {
        public int Id { get; set; }
        public string InstallmentType { get; set; }
        public string Installment { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> Percentage { get; set; }
       
        public double Tax { get; set; }
        public double amount { get; set; }
        public double TaxAmount { get; set; }
        public double latesurchargeAmount { get; set; }
        public double totalAmount { get; set; }
        public string dueDate { get; set; }
        public string paymentStatus { get; set; }
        public double balance { get; set; }
        public PaymentDetailDTO paymentDetailDTOs { get; set; }
    }
}