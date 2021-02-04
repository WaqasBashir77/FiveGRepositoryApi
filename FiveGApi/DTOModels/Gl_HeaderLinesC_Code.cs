using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class Gl_HeaderLinesC_Code
    {
        public int H_ID { get; set; }
        public Nullable<System.DateTime> J_Date { get; set; }
        public Nullable<System.DateTime> Doc_Date { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Source { get; set; }
        public Nullable<int> Source_Tran_Id { get; set; }
        public string Pay_Mode { get; set; }
        public string Pay_Bank { get; set; }
        public string Pay_To { get; set; }
        public string Pay_Company { get; set; }
        public string Pay_Project { get; set; }
        public string Pay_Location { get; set; }
        public string Dep_Mode { get; set; }
        public string Dep_Bank { get; set; }
        public string Dep_By { get; set; }
        public string Dep_Company { get; set; }
        public string Dep_Project { get; set; }
        public string Dep_Location { get; set; }
        public string Trans_Status { get; set; }
        public string Posted_date { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
        public int L_ID { get; set; }       
        public Nullable<int> C_CODE { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public int C_ID { get; set; }
        public string C_Code { get; set; }
        public string Company { get; set; }
        public string Project { get; set; }
        public string Location { get; set; }
        public string Account { get; set; }
        public string Party { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        

    }
}