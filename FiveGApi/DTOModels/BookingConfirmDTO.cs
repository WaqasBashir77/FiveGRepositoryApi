using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class BookingConfirmDTO
    {
        public int ID { get; set; }
        public string Ref_num { get; set; }
        public string Form_num { get; set; }
        public string Authorize_Status { get; set; }
        public string File_Status { get; set; }
        public string Replaced_Form { get; set; }
        public string Applicant_name { get; set; }
        public string CNIC { get; set; }
        public Nullable<System.DateTime> Form_Rec_Date { get; set; }
        public string Contact_Num { get; set; }
        public Nullable<int> Property_ID { get; set; }
        public string Member_Num { get; set; }
        public string Book_Emp { get; set; }
        public string Book_Dealer { get; set; }
        public Nullable<decimal> Booking_Percent { get; set; }
        public Nullable<decimal> Booking_amount { get; set; }
        public Nullable<decimal> Confirm_Percent { get; set; }
        public Nullable<decimal> Confirm_amount { get; set; }
        public Nullable<decimal> MS_amount { get; set; }
        public Nullable<decimal> Total_amount { get; set; }
        public Nullable<decimal> Tax_Percent { get; set; }
        public Nullable<decimal> Rebate_Percent { get; set; }
        public Nullable<decimal> Emp_Rebate { get; set; }
        public Nullable<decimal> Dealer_Rebate { get; set; }
        public Nullable<decimal> Emp_B_RAmt { get; set; }
        public Nullable<decimal> Emp_C_RAmt { get; set; }
        public Nullable<decimal> Dealer_B_RAmt { get; set; }
        public Nullable<decimal> Dealer_C_RAmt { get; set; }
        public Nullable<decimal> Com_B_RAmt { get; set; }
        public Nullable<decimal> Com_C_RAmt { get; set; }
        public string Payment_B_Status { get; set; }
        public string Payment_C_Status { get; set; }
        public string Payment_MSFee_Status { get; set; }
        public Nullable<System.DateTime> Booking_Date { get; set; }
        public Nullable<System.DateTime> Confirmation_Date { get; set; }
        public Nullable<System.DateTime> MSFee_Date { get; set; }
        public string Remarks { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_ON { get; set; }
        public string Updated_By { get; set; }
        public string Updated_On { get; set; }
        public string Authorize_By { get; set; }
        public Nullable<System.DateTime> Authorize_Date { get; set; }
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }

    }
}