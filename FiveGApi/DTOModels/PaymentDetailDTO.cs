using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class PaymentDetailDTO
    {
        public int Payment_ID { get; set; }     
        public string paymentMethod { get; set; }
        public int recievedAmount { get; set; }
        public string InstrumentBank { get; set; }
        public string InstrumentNumber { get; set; }
        public string InsturmentBankBranch { get; set; }
        public string InsturmentDate { get; set; }
        public string paymentDescription { get; set; }
        public string Payment_Account { get; set; }

        public static implicit operator List<object>(PaymentDetailDTO v)
        {
            throw new NotImplementedException();
        }
    }
}