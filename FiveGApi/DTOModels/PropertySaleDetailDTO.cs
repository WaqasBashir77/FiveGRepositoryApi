using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class PropertySaleDetailDTO
    {
        public int Id { get; set; }
        public int Persentage { get; set; }
        public int Tax { get; set; }
        public int amount { get; set; }
        public int TaxAmount { get; set; }
        public int latesurchargeAmount { get; set; }
        public int totalAmount { get; set; }
        public string dueDate { get; set; }
        public string paymentStatus { get; set; }
        public string Installment { get; set; }
        public double balance { get; set; }

        public List<PaymentDetailDTO> paymentDetails { get; set; }
    }
}