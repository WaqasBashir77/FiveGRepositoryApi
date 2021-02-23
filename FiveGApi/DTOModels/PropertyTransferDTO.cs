﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class PropertyTransferDTO
    {
        public int ID { get; set; }
        public int Booking_ID { get; set; }
        public Nullable<int> Project_ID { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public string Buyer_Name { get; set; }
        public string Buyer_FatherName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerCNIC { get; set; }
        public string BuyerMobile_1 { get; set; }
        public string BuyerMobile_2 { get; set; }
        public string BuyerMember_Reg_No { get; set; }
        public string Buyer_Picture { get; set; }
        public string Seller_Name { get; set; }
        public string Seller_FatherName { get; set; }
        public string SellerAddress { get; set; }
        public string SellerEmail { get; set; }
        public string SellerCNIC { get; set; }
        public string SellerMobile_1 { get; set; }
        public string SellerMobile_2 { get; set; }
        public string SellerMember_Reg_No { get; set; }
        public string Seller_Picture { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<bool> TransferStatus { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
    }
}