using FiveGApi.DTOModels;
using FiveGApi.Helper;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace FiveGApi.Controllers
{
  [Authorize]
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        private string UserId;
        public ValuesController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
        //private FiveG_DBEntities db = new FiveG_DBEntities();
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        [HttpGet]
        public IHttpActionResult GetPropertySaleLsit()
        {
            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            List<PropertySale> propertySale = new List<PropertySale>();
            if (!SecurityGroupDTO.CheckSuperAdmin(groupId))
                propertySale = db.PropertySales.Where(x => x.SecurityGroupId == groupId).ToList();
            else
                propertySale = db.PropertySales.ToList(); 
          
            //if (propertySale == null)
            //{
                //return NotFound();
            //}
            return Ok(propertySale);
        }

        // GET api/values
        [HttpGet]
        public IHttpActionResult GetPaymentMilestoneByProjectId(string id)
        {

            PaymentMilestone paymentMilestone = db.PaymentMilestones.Where(x => x.PaymentScheduleCode == id).FirstOrDefault();
            List<TempTableForInstallment> tempTableFors = db.TempTableForInstallments.Where(x => x.parentId == id).ToList();
            List<TempTableForInstallmentDTO> tempTableForsDTO = new List<TempTableForInstallmentDTO>();
            int i = 0;
            DateTime dateTime = DateTime.Now;
            foreach (var item in tempTableFors)
            {
                TempTableForInstallmentDTO temp = new TempTableForInstallmentDTO();
                temp.Id = item.Id;
                temp.Installment = item.Installment;
                temp.InstallmentType = item.InstallmentType;
                temp.Percentage = item.Percentage;
               
                temp.paymentDetailDTOs = new PaymentDetailDTO();
                if (i != 0 )
                {
                    if (item.Frequency == "Monthly")
                    {
                        dateTime = dateTime.AddMonths(1);
                    }
                    else if (item.Frequency == "Quarterly")
                    {
                        dateTime = dateTime.AddMonths(4);
                    }
                    else if (item.Frequency == "bianually")
                    {
                        dateTime = dateTime.AddMonths(6);
                    }
                    if (item.Frequency == "One Time")
                    {
                        dateTime = dateTime.AddMonths(1);
                    }
                    
                    string chngeformate = dateTime.ToString("dd/MM/yyyy");
                    temp.dueDate = chngeformate;// dateTime.Date;
                    
                }
                else
                {
                    DateTime date = new DateTime();
                    date = DateTime.Now;
                    string chngeformate = date.ToString("dd/MM/yyyy");
                    temp.dueDate = chngeformate;
                }
                
                i++;
                tempTableForsDTO.Add(temp);
            }
            if (paymentMilestone == null)
            {
                return NotFound();
            }

            var viewModel = new
            {
                paymentMilestone,
                tempTableForsDTO
            };

            return Ok(viewModel);
        }



        // POST api/values
        [HttpPost]
        public IHttpActionResult addPropertySale(PropertySaleDTO propertySale)
        {
            ResponseModel response = new ResponseModel();
            PropertySale OriginalPropertySale = new PropertySale();
            try
            {
                if (propertySale != null)
                {

                    
                    OriginalPropertySale.Project_ID = propertySale.projectId;
                    OriginalPropertySale.Unit_ID = propertySale.unitId;
                    OriginalPropertySale.Buyer_Name = propertySale.buyerName;
                    OriginalPropertySale.Buyer_Father_Name = propertySale.buyerFatherName;
                    OriginalPropertySale.Mobile_1 = propertySale.mobile_1.ToString();
                    OriginalPropertySale.Mobile_2 = propertySale.mobile_2.ToString();
                    OriginalPropertySale.Member_Reg_No = propertySale.memberRegNo.ToString();
                    OriginalPropertySale.Dealer_Comm = (double?)propertySale.dealerCommission;
                    OriginalPropertySale.Dealer_ID = propertySale.dealerId;
                    OriginalPropertySale.Address = propertySale.address;
                    OriginalPropertySale.Email = propertySale.email;
                    OriginalPropertySale.Relationship_With_Nominee = propertySale.relationWithNomine;
                    OriginalPropertySale.Sale_Status = propertySale.saleStatus;
                    OriginalPropertySale.Nominee_Name = propertySale.nomineeName;
                    OriginalPropertySale.Nominee_CNIC = propertySale.nomineeCnic;
                    OriginalPropertySale.Discount_Amount = propertySale.discountAmount;
                    OriginalPropertySale.Nominee_G_Number = propertySale.nomineeGNumber;
                    OriginalPropertySale.CNIC = propertySale.cnic;
                    OriginalPropertySale.Employee = propertySale.employeeId;
                    OriginalPropertySale.PaymentCode = propertySale.PaymentCode;
                    OriginalPropertySale.Employee_Com = (double?)propertySale.employeeCommission;
                    OriginalPropertySale.Created_ON = DateTime.Now;
                    OriginalPropertySale.Created_By = propertySale.Created_By;
                    OriginalPropertySale.SecurityGroupId = propertySale.SecurityGroupId;
                    OriginalPropertySale.differentiableAmount = propertySale.differentiableAmount;
                    //OriginalPropertySale.Nominee_Picture = propertySale.Nominee_Picture;
                    if (propertySale.Purchaser_Picture != "")
                    {
                        string[] image = propertySale.Purchaser_Picture.Split(',');
                        OriginalPropertySale.Purchaser_Picture = Convert.FromBase64String(image[1]);

                    }
                    if (propertySale.Nominee_Picture != "")
                    {
                        string[] image = propertySale.Nominee_Picture.Split(',');
                        OriginalPropertySale.Nominee_Picture = Convert.FromBase64String(image[1]);

                    }

                    List<SaleInstallment> saleInstallmentList = new List<SaleInstallment>();
                    if (propertySale.propertySaleDetails != null)
                    {

                        foreach (var item in propertySale.propertySaleDetails.ToList())
                        {
                            SaleInstallment saleInstallment = new SaleInstallment();

                            saleInstallment.Project_ID = propertySale.projectId;
                            saleInstallment.Unit_ID = propertySale.unitId;
                            saleInstallment.ins_milestone = item.Installment;
                            saleInstallment.ins_milestone_percentage = item.Percentage;
                            saleInstallment.ins_latesurcharge_amount = item.latesurchargeAmount;
                            saleInstallment.ins_due_date = item.dueDate.ToString();
                            saleInstallment.ins_payment_status = item.paymentStatus;
                            saleInstallment.ins_balance = item.balance;
                            saleInstallment.Booking_ID = OriginalPropertySale.Booking_ID;
                            saleInstallment.ins_amount = item.amount;
                            saleInstallment.ins_amount_tax = item.TaxAmount;
                            saleInstallment.ins_total_amount = item.totalAmount;
                            saleInstallment.Created_By = propertySale.Created_By;
                            saleInstallment.Created_ON = DateTime.Now;
                            saleInstallment.SecurityGroupId = propertySale.SecurityGroupId;
                            saleInstallment.OtherTaxAmount = item.OtherTaxAmount;
                            saleInstallment.Payment_Account = Convert.ToInt32(item.paymentDetailDTOs.Payment_Account);

                            if (item.balance == 0 && item.paymentStatus != "Paid")
                            {
                                saleInstallment.ins_payment_status = "UnPaid";
                            }
                            else
                            {
                                saleInstallment.ins_payment_status = item.paymentStatus;
                            }
                            saleInstallmentList.Add(saleInstallment);

                            if (item.paymentDetailDTOs.paymentMethod != null)
                            {
                                PaymentInstallment paymentInstallment = new PaymentInstallment();
                                paymentInstallment.Project_ID = propertySale.projectId;
                                paymentInstallment.Unit_ID = propertySale.unitId;
                                paymentInstallment.Payment_amount = item.paymentDetailDTOs.recievedAmount;
                                paymentInstallment.Instrument_Type = item.paymentDetailDTOs.paymentMethod;
                                paymentInstallment.Ins_ID = saleInstallment.Ins_ID;
                                paymentInstallment.Booking_ID = OriginalPropertySale.Booking_ID;
                                paymentInstallment.instrument_bank = item.paymentDetailDTOs.InstrumentBank;
                                paymentInstallment.instrument_bank_Branch = item.paymentDetailDTOs.InsturmentBankBranch;
                                paymentInstallment.instrument_date = Convert.ToDateTime( item.paymentDetailDTOs.InsturmentDate);
                                paymentInstallment.instrument_remarks = item.paymentDetailDTOs.paymentDescription;
                                paymentInstallment.instrument_number = item.paymentDetailDTOs.InstrumentNumber;
                                paymentInstallment.Created_By = propertySale.Created_By;
                                paymentInstallment.Created_ON = DateTime.Now;
                                paymentInstallment.SecurityGroupId = propertySale.SecurityGroupId;
                                paymentInstallment.Payment_Account = Convert.ToInt32(item.paymentDetailDTOs.Payment_Account);
                                saleInstallment.PaymentInstallments.Add(paymentInstallment);
                            }
                        }
                        OriginalPropertySale.SaleInstallments = saleInstallmentList;
                        
                    }
                    db.PropertySales.Add(OriginalPropertySale);
                    db.SaveChanges();
                    response.Code = 1;
                }
            }
            catch (Exception ex)
            {
                response.Code = 0;
            }
            return Ok(OriginalPropertySale);
        }
        [HttpPost]
        [Route("paymentInstallmentAuthorise")]
        public IHttpActionResult paymentInstallmentAuthorise(int paymentID)
        {
            var paymentInstallment = db.PaymentInstallments.Where(x => x.Payment_ID == paymentID).FirstOrDefault();
            if(paymentInstallment!=null)
            {
                var saleinstallment = db.SaleInstallments.Where(x => x.Ins_ID == paymentInstallment.Ins_ID).FirstOrDefault();
                
                    var pro = paymentInstallment;
                    Project_Entries booking_EntriesforROS = new Project_Entries();
                    booking_EntriesforROS.Transaction_ID = pro.Booking_ID;
                    booking_EntriesforROS.Entry_Date = DateTime.Now;
                    booking_EntriesforROS.Entry_Type = "Rebate";
                    booking_EntriesforROS.Created_By = "Admin";
                    booking_EntriesforROS.Created_On = DateTime.Now;
                    booking_EntriesforROS.Status = "Draft";
                // var c_Code = pro.Company + "." + pro.ProjectSeg + "." + pro.LocationSeg;
                #region Project Sale Price / Dealer Commision / Employee Commision
                var DebitCCode = "";
                var CreditCCode = "";
                int[] arrOfProjectEntriesID= { };
                //if (saleinstallment.ins_milestone == "Booking")
                //{
                    var account = db.Bank_Accounts.Where(x => x.ID == paymentInstallment.Payment_Account).FirstOrDefault();
                    DebitCCode = account.GL_Mapping.ToString();
                    var project = db.Projects.Where(x => x.Id == paymentInstallment.Project_ID).FirstOrDefault();
                    var c_Code = project.Company + "." + project.ProjectSeg + "." + project.LocationSeg;
                    var coa_Segment = db.COA_Segments.Where(x => x.Name == "Receivable from members").FirstOrDefault();
                    CreditCCode = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + ".0000").ToString();

                //}
                //else if(saleinstallment.ins_milestone == "Confirmation")
                //{
                //    var account = db.Bank_Accounts.Where(x => x.ID == saleinstallment.Payment_Account).FirstOrDefault();
                //    DebitCCode = account.GL_Mapping.ToString();
                //    var project = db.Projects.Where(x => x.Id == paymentInstallment.Project_ID).FirstOrDefault();
                //    var c_Code = project.Company + "." + project.ProjectSeg + "." + project.LocationSeg;
                //    var coa_Segment = db.COA_Segments.Where(x => x.Name == "Receivable from members").FirstOrDefault();
                //    CreditCCode = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + ".0000").ToString();

                //}
                //else if (saleinstallment.ins_milestone == "")
                //{

                //}
                ///-------------------Receivable from members------------------------------///////////
                //var coa_Segment = db.COA_Segments.Where(x => x.Name == "Receivable from members").FirstOrDefault();
                    booking_EntriesforROS.Transaction_ID = paymentID;
                    booking_EntriesforROS.C_CODE = DebitCCode;
                    booking_EntriesforROS.Debit = (decimal)paymentInstallment.Payment_amount;
                    booking_EntriesforROS.Credit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                    //arrOfProjectEntriesID[0] = booking_EntriesforROS.E_ID;
                    booking_EntriesforROS.C_CODE = CreditCCode;
                    booking_EntriesforROS.Credit = (decimal)paymentInstallment.Payment_amount;
                    booking_EntriesforROS.Debit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                //arrOfProjectEntriesID[1] = booking_EntriesforROS.E_ID;

                #endregion
                #region Gl Header and GL Lines
                var projectSale = db.PropertySales.Where(x => x.Booking_ID == saleinstallment.Booking_ID).FirstOrDefault();
                var glheader = db.GL_Headers.Where(x => x.Source_Tran_Id == projectSale.Booking_ID && x.Source == "Projects").FirstOrDefault();
                //var bookingConfirm = db.BookingConfirms.Where(x => x.ID == existBooking_Payments.ID).FirstOrDefault();
                // var propertydef = db.PropertyDefs.Where(x => x.ID == bookingConfirm.Property_ID).FirstOrDefault();
                GL_Headers gL_Headers = new GL_Headers();
                var projects = db.Projects.Where(x => x.Id == pro.Project_ID).FirstOrDefault();
                var projectDetail = db.ProjectDetails.Where(x => x.projectId == pro.Project_ID).FirstOrDefault();

                if (glheader == null)
                {
                    gL_Headers.J_Date = DateTime.Now;
                    gL_Headers.Doc_Date = DateTime.Now;
                    gL_Headers.Currency = "PKR";
                    gL_Headers.Description = projects.projectCode + "-" + projectDetail.unitNumber + "-" + projectDetail.unitType +"-"+projectSale.Buyer_Name;
                    gL_Headers.Remarks = "";
                    gL_Headers.Source = "Projects";
                    gL_Headers.Trans_Status = "UnPosted";
                    gL_Headers.Source_Tran_Id = projectSale.Booking_ID;
                    db.GL_Headers.Add(gL_Headers);
                    db.SaveChanges();
                }
                int glhv = 0;
                if (gL_Headers.H_ID > 0)
                {
                    glhv = gL_Headers.H_ID;
                }
                else
                {
                    glhv = glheader.H_ID;
                }
                var EntriesProject = db.Project_Entries.Where(x => x.Transaction_ID == paymentID && x.Entry_Type == "Rebate" && x.Status != "Transferred").ToList();
                if (EntriesProject != null)
                {
                    foreach (var item in EntriesProject)
                    {
                        GL_Lines gL_Lines = new GL_Lines();
                        gL_Lines.H_ID = glhv;
                        gL_Lines.C_CODE = Convert.ToInt32(item.C_CODE);
                        gL_Lines.Debit = item.Debit;
                        gL_Lines.Credit = item.Credit;
                        gL_Lines.Created_By = "1";
                        gL_Lines.Created_On = DateTime.Now;
                        // gL_Lines.Description = glhv;              

                        db.GL_Lines.Add(gL_Lines);
                        db.SaveChanges();
                        var be = db.Project_Entries.Where(x => x.E_ID == item.E_ID).FirstOrDefault();
                        be.Status = "Transferred";
                        be.Updated_By = "1";
                        be.Updated_On = DateTime.Now;
                        db.SaveChanges();
                    }

                }
                saleinstallment.AuthorizeStatus = true;
                db.SaveChanges();
                paymentInstallment.Payment_amount = paymentInstallment.Payment_amount;
                paymentInstallment.AuthorizeStatus = true;
                db.SaveChanges();
                return Ok();
                #endregion
            }
            else
            {
                return NotFound();
            }
        }
      
        [HttpGet]
        [Route("getdetail")]
        public IHttpActionResult getdetail()
        {
            
            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();
          
                var re = Request;
                //HttpRequestMessage re = new HttpRequestMessage();
                var headers = re.Headers;
               int tempId = Convert.ToInt32( headers.GetValues("Id").First());
                //Order by Value_Orderno ASC
                var masterobj = db.PropertySales.Where(x => x.Booking_ID == tempId).FirstOrDefault();


            return Ok(masterobj);
        }
        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("getallbank")]
        public IHttpActionResult GetAllBank()
        {
            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();
            try
            {
                //Order by Value_Orderno ASC
                lookup_Values = db.Lookup_Values.Where(x => x.Ref_ID == 4 && x.Value_Status == true).OrderBy(x => x.Value_orderNo).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok(lookup_Values);
        }

        [HttpGet]
        [Route("getallcity")]
        public IHttpActionResult GetAllCity()
        {
            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();
            try
            {
                //Order by Value_Orderno ASC
                lookup_Values = db.Lookup_Values.Where(x => x.Ref_ID == 1 && x.Value_Status == true).OrderBy(x => x.Value_orderNo).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok(lookup_Values);
        }

        [HttpGet]
        [Route("getallUOM")]
        public IHttpActionResult GetallUOM()
        {
            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();
            try
            {
                //Order by Value_Orderno ASC
                lookup_Values = db.Lookup_Values.Where(x => x.Ref_ID == 2 && x.Value_Status == true).OrderBy(x => x.Value_orderNo).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok(lookup_Values);
        }

        [HttpPost]
        [Route("getPropertySaleDataForUpdate")]
        public IHttpActionResult getPropertySaleDataForUpdate(GeneralDTO general)
        {
            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            PropertySale propertySale = new PropertySale();
            var getPropertyMaster = db.PropertySales.Where(x => x.Booking_ID == general.Id).FirstOrDefault();
            if (getPropertyMaster != null && getPropertyMaster.SecurityGroupId == groupId)
            {
                propertySale = getPropertyMaster;
                return Ok(propertySale);
            }
            if (getPropertyMaster != null)
            {
                propertySale = getPropertyMaster;
            }
            return Ok(propertySale);

        }

        [HttpPost]
        [Route("updatePropertySale")]
        public IHttpActionResult updatePropertySale(PropertySaleDTO propertySale)
        {

            try
            {
                var MasterObj = db.PropertySales.Where(x => x.Booking_ID == propertySale.ID).FirstOrDefault();
                if (MasterObj != null)
                {

                    MasterObj.Project_ID = propertySale.projectId;
                    MasterObj.Unit_ID = propertySale.unitId;
                    MasterObj.Buyer_Name = propertySale.buyerName;
                    MasterObj.Buyer_Father_Name = propertySale.buyerFatherName;
                    MasterObj.Mobile_1 = propertySale.mobile_1.ToString();
                    MasterObj.Mobile_2 = propertySale.mobile_2.ToString();
                    MasterObj.Member_Reg_No = propertySale.memberRegNo.ToString();
                    MasterObj.Dealer_Comm = (double?)propertySale.dealerCommission;
                    MasterObj.Dealer_ID = propertySale.dealerId;
                    MasterObj.Address = propertySale.address;
                    MasterObj.Email = propertySale.email;
                    MasterObj.Relationship_With_Nominee = propertySale.relationWithNomine;
                    MasterObj.Sale_Status = propertySale.saleStatus;
                    MasterObj.Nominee_Name = propertySale.nomineeName;
                    MasterObj.Nominee_CNIC = propertySale.nomineeCnic;
                    MasterObj.Discount_Amount = propertySale.discountAmount;
                    MasterObj.Nominee_G_Number = propertySale.nomineeGNumber;
                    MasterObj.CNIC = propertySale.cnic;
                    MasterObj.Employee = propertySale.employeeId;
                    MasterObj.PaymentCode = propertySale.PaymentCode;
                    MasterObj.Employee_Com = (double?)propertySale.employeeCommission;
                    MasterObj.Created_ON = DateTime.Now;
                    MasterObj.Created_By = propertySale.Created_By;
                    MasterObj.SecurityGroupId = propertySale.SecurityGroupId;
                    MasterObj.differentiableAmount = propertySale.differentiableAmount;
                    //OriginalPropertySale.Nominee_Picture = propertySale.Nominee_Picture;
                    if (propertySale.Purchaser_Picture != "")
                    {
                        string[] image = propertySale.Purchaser_Picture.Split(',');
                        MasterObj.Purchaser_Picture = Convert.FromBase64String(image[1]);

                    }
                    if (propertySale.Nominee_Picture != "")
                    {
                        string[] image = propertySale.Nominee_Picture.Split(',');
                        MasterObj.Nominee_Picture = Convert.FromBase64String(image[1]);

                    }
                    foreach (var saleitem in propertySale.propertySaleDetails.ToList())
                    {
                        SaleInstallment saleInstallment = new SaleInstallment();
                        if (saleitem.Id > 0)
                        {
                            saleInstallment = db.SaleInstallments.Where(x => x.Booking_ID == saleitem.BookingId && x.Ins_ID == saleitem.Id && x.Project_ID == saleitem.ProjectId).FirstOrDefault();

                            saleInstallment.ins_due_date = saleitem.dueDate;
                            saleInstallment.ins_payment_status = saleitem.paymentStatus;
                            saleInstallment.ins_latesurcharge_amount = saleitem.latesurchargeAmount;
                            saleInstallment.ins_balance = saleitem.balance;
                            saleInstallment.OtherTaxAmount = saleitem.OtherTaxAmount;
                            saleInstallment.Updated_By = propertySale.Update_By;
                            saleInstallment.Updated_On = DateTime.Now;
                            saleInstallment.Payment_Account = saleitem.Payment_Account;
                        }
                        else
                        {
                            
                            saleInstallment.Booking_ID = saleInstallment.Booking_ID;
                            saleInstallment.Unit_ID = saleInstallment.Unit_ID;
                            saleInstallment.Project_ID = saleInstallment.Project_ID;
                            saleInstallment.ins_total_amount = saleitem.totalAmount;
                            saleInstallment.ins_remaining = saleitem.balance;
                            saleInstallment.ins_milestone_percentage = saleitem.Percentage;
                            saleInstallment.ins_latesurcharge_amount = saleitem.latesurchargeAmount;
                            saleInstallment.ins_due_date = saleitem.dueDate;
                            saleInstallment.ins_balance = saleitem.balance;
                            saleInstallment.OtherTaxAmount = saleitem.OtherTaxAmount;
                            saleInstallment.ins_payment_status = saleitem.paymentStatus;
                            saleInstallment.Updated_By = propertySale.Update_By;
                            saleInstallment.Updated_On = DateTime.Now;
                            saleInstallment.Payment_Account = saleitem.Payment_Account;
                            db.SaleInstallments.Add(saleInstallment);

                        }


                        if (saleitem.paymentDetailDTOs != null)
                        {
                            if (saleitem.paymentDetailDTOs.Payment_ID == 0)
                            {
                                PaymentInstallment paymentInstallment = new PaymentInstallment();
                                paymentInstallment.Project_ID = saleitem.ProjectId;
                                paymentInstallment.Unit_ID = propertySale.unitId;
                                paymentInstallment.Payment_amount = saleitem.amount;
                                paymentInstallment.Instrument_Type = saleitem.InstallmentType;
                                paymentInstallment.Ins_ID = saleInstallment.Ins_ID;
                                paymentInstallment.Booking_ID = saleitem.BookingId;
                                paymentInstallment.instrument_bank =saleitem.paymentDetailDTOs.InstrumentBank;
                                paymentInstallment.instrument_bank_Branch = saleitem.paymentDetailDTOs.InsturmentBankBranch;
                                paymentInstallment.instrument_date = Convert.ToDateTime(saleitem.paymentDetailDTOs.InsturmentDate);
                                paymentInstallment.instrument_number = saleitem.paymentDetailDTOs.InstrumentNumber;
                                paymentInstallment.instrument_remarks = saleitem.paymentDetailDTOs.paymentDescription;
                                paymentInstallment.Created_By = 1;
                                paymentInstallment.Created_ON = DateTime.Now;
                                paymentInstallment.Payment_Account = Convert.ToInt32(saleitem.paymentDetailDTOs.Payment_Account);
                                db.PaymentInstallments.Add(paymentInstallment);
                            }
                            else
                            {

                                // PaymentInstallment paymentInstallment = new PaymentInstallment();
                                var paymentInstallment = db.PaymentInstallments.Where(x => x.Payment_ID == saleitem.paymentDetailDTOs.Payment_ID).FirstOrDefault();
                                if (paymentInstallment != null)
                                {
                                    paymentInstallment.Project_ID = saleitem.ProjectId;
                                    paymentInstallment.Unit_ID = propertySale.unitId;
                                    paymentInstallment.Payment_amount = saleitem.amount;
                                    paymentInstallment.Instrument_Type = saleitem.InstallmentType;
                                    paymentInstallment.Ins_ID = saleInstallment.Ins_ID;
                                    paymentInstallment.Booking_ID = saleitem.BookingId;
                                    paymentInstallment.instrument_bank = saleitem.paymentDetailDTOs.InstrumentBank;
                                    paymentInstallment.instrument_bank_Branch = saleitem.paymentDetailDTOs.InsturmentBankBranch;
                                    paymentInstallment.instrument_date = Convert.ToDateTime(saleitem.paymentDetailDTOs.InsturmentDate);
                                    paymentInstallment.instrument_number = saleitem.paymentDetailDTOs.InstrumentNumber;
                                    paymentInstallment.instrument_remarks = saleitem.paymentDetailDTOs.paymentDescription;
                                    paymentInstallment.Created_By = 1;
                                    paymentInstallment.Created_ON = DateTime.Now;
                                    paymentInstallment.Payment_Account = Convert.ToInt32(saleitem.paymentDetailDTOs.Payment_Account);
                                    db.SaveChanges();
                                }
                            }
                        }
                            
                        
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return Ok();

        }

        [HttpPost]
        [Route("getPaymentchildData")]
        public IHttpActionResult getPaymentchildData(GeneralDTO general)
        {

            var paymentInstallmentList = db.PaymentInstallments.Where(x => x.Ins_ID == general.insId).ToList();

            return Ok(paymentInstallmentList);

        }


        [HttpGet]
        [Route("getemployees")]
        public IHttpActionResult getEmployees()
        {

            var employeesList = db.Registrations.Where(x => x.Type == "Staff").ToList();

            return Ok(employeesList);

        }

        [HttpGet]
        [Route("getdealers")]
        public IHttpActionResult getDealers()
        {

            var dealerList = db.Registrations.Where(x => x.Type == "Dealer").ToList();

            return Ok(dealerList);

        }
        [HttpGet]
        [Route("GetallSocieties")]
        public IHttpActionResult GetallSocieties()
        {
            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();
            try
            {
                //Get All Societies on the basic of refID==3
                lookup_Values = db.Lookup_Values.Where(x => x.Ref_ID == 3 && x.Value_Status == true).OrderBy(x => x.Value_orderNo).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok(lookup_Values);
        }
        [Route("UploadAttachments")]
        [HttpPost]
        public IHttpActionResult UploadAttachments()
        {
            if (UserId != null)
            {
                var userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().Select(x => x.SecurityGroupId).FirstOrDefault();

                //Create the Directory.
                string path = HttpContext.Current.Server.MapPath("~/Attachments/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                List<string> stringList = new List<string>();
                var url = HttpContext.Current.Request.Form["url"];
                var tableID = HttpContext.Current.Request.Form["tableID"];
                var form = db.Forms.Where(x => x.FormUrl == url).FirstOrDefault();
                if (url == null || tableID == null || form == null)
                {
                    string value = "Url=" + url + "-TableId=" + tableID + "-form=" + form;
                    return Ok(value);
                }

                //Fetch the File.
                var postedFile = HttpContext.Current.Request.Files;
                if (postedFile.Count > 0)
                {
                    for (int i = 0; i < postedFile.Count; i++)
                    {
                        HttpPostedFile upload = postedFile[i];
                        if (upload.ContentLength == 0) continue;
                        //Fetch the File Name.
                        var key = postedFile.AllKeys[i];
                        string fileName = upload.FileName + DateTime.Now.ToFileTime() + Path.GetExtension(upload.FileName);
                        //Save the File.
                        upload.SaveAs(path + fileName);
                        stringList.Add(fileName);
                        Attachment attachment = new Attachment();
                        attachment.FileName = fileName;
                        attachment.Description = key;
                        attachment.FileExtension = Path.GetExtension(upload.FileName);
                        attachment.FileType = Path.GetExtension(upload.FileName);
                        attachment.FilePath = "~/Attachments/";
                        attachment.FileParentRootFolder = "Attachments";
                        attachment.FileParentRootFolder = "Attachments";
                        attachment.FileSize = upload.ContentLength;
                        attachment.IsCompleted = true;
                        attachment.CreatedBy = 1;
                        attachment.CreatedDate = DateTime.Now;
                        attachment.SecurityGroup = userSecurityGroup;
                        db.Attachments.Add(attachment);
                        db.SaveChanges();
                        Attatchment_Relation attatchment_Relation = new Attatchment_Relation();
                        attatchment_Relation.FormId = form.Id;
                        attatchment_Relation.FileID = Convert.ToInt32(attachment.ID);
                        attatchment_Relation.TableRowId = Convert.ToInt32(tableID);
                        db.Attatchment_Relation.Add(attatchment_Relation);
                        db.SaveChanges();
                    }
                    //Send OK Response to Client.
                    return Ok(stringList);
                }
                else
                {

                    return Ok("File Not found");
                }
            }
            else
            {
                return NotFound();
            }
            
        }
        [HttpDelete]     
        [Route("DeleteAttachment")]
        [ResponseType(typeof(Attachment))]
        public IHttpActionResult DeleteAttachment(int attachmentID)
        {
            //get Attachment
            Attachment attachment = db.Attachments.Where(x=>x.ID==attachmentID).FirstOrDefault();
            if (attachment == null)
            {
                return NotFound();
            }
            //Remove Attachment
            db.Attachments.Remove(attachment);
            db.SaveChanges();
            //get attatchment_Relation
            Attatchment_Relation attatchment_Relation = db.Attatchment_Relation.Where(x => x.FileID == attachmentID).FirstOrDefault();
            //Remove Attachment
            if (attatchment_Relation != null)
            {
                db.Attatchment_Relation.Remove(attatchment_Relation);
                db.SaveChanges();
            }           
            //return attachment with ok message
            return Ok(attachment);
        }
        [HttpGet]
        [Route("GetAttachmetsByFormIDAndTableID")]
        public IHttpActionResult GetAttachmetsByFormIDAndTableID(string FormURl, int tableID)
        {
            if (UserId != null)
            {
                var userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().Select(x => x.SecurityGroupId).FirstOrDefault();

                var formsUrl = db.Forms.Where(x => x.FormUrl == FormURl).FirstOrDefault();
                if (formsUrl == null)
                {
                    return NotFound();
                }
                var attachments = new List<Attachment>();
                if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup))
                     attachments = (from a in db.Attachments
                                       join ar in db.Attatchment_Relation on a.ID equals ar.FileID
                                       where ar.FormId == formsUrl.Id && ar.TableRowId == tableID &&a.SecurityGroup==userSecurityGroup
                                       select a).AsQueryable().ToList();

                else
                     attachments = (from a in db.Attachments
                                       join ar in db.Attatchment_Relation on a.ID equals ar.FileID
                                       where ar.FormId == formsUrl.Id && ar.TableRowId == tableID
                                       select a).AsQueryable().ToList();

                if (attachments == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(attachments);
                }
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Route("DownloadPdfFile")]
        public HttpResponseMessage DownloadPdfFile(int id)
        {
            //var UserName = User.Identity.Name;
            if (UserId != null)
            {
                var userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().Select(x => x.SecurityGroupId).FirstOrDefault();

                HttpResponseMessage response = null;

                try
                {
                    var file = new Attachment();
                    if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup))
                          file = db.Attachments.Where(b => b.ID == id && b.SecurityGroup==userSecurityGroup).SingleOrDefault();

                    else
                        file = db.Attachments.Where(b => b.ID == id ).SingleOrDefault();


                    if (file == null)
                    {
                        return response;
                    }
                    //Create HTTP Response.
                    response = Request.CreateResponse(HttpStatusCode.OK);

                    //Set the File Path.
                    string filePath = HttpContext.Current.Server.MapPath("~/Attachments/") + file.FileName;

                    //Check whether File exists.
                    if (!File.Exists(filePath))
                    {
                        //Throw 404 (Not Found) exception if File not found.
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.ReasonPhrase = string.Format("File not found: {0} .", file.FileName);
                        throw new HttpResponseException(response);
                    }

                    //Read the File into a Byte Array.
                    byte[] bytes = File.ReadAllBytes(filePath);

                    //Set the Response Content.
                    response.Content = new ByteArrayContent(bytes);

                    //Set the Response Content Length.
                    response.Content.Headers.ContentLength = bytes.LongLength;

                    //Set the Content Disposition Header Value and FileName.
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = file.FileName;

                    //Set the File Content Type.
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(file.FileName));

                    return response;
                    // return response;

                }
                catch (Exception ex)
                {
                    return response;
                }
            }
            else
            {
                return null;
            }
        }
        [NonAction]
        private int GenerateCOACombinations(string CCode)
        {
            var ExistedCOA_Combinations = db.COA_Combinations.Where(x => x.C_Code == CCode).FirstOrDefault();
            if (ExistedCOA_Combinations != null)
            {
                return ExistedCOA_Combinations.C_ID;
            }
            else
            {
                string[] codeParts = CCode.Split('.');
                COA_Combinations cOA_Combinations = new COA_Combinations();
                cOA_Combinations.C_Code = CCode;
                cOA_Combinations.Company = codeParts[0];
                cOA_Combinations.Project = codeParts[1];
                cOA_Combinations.Location = codeParts[2];
                cOA_Combinations.Account = codeParts[3];
                cOA_Combinations.Party = codeParts[4];
                cOA_Combinations.Created_By = "1";
                cOA_Combinations.Created_ON = DateTime.Now;
                db.COA_Combinations.Add(cOA_Combinations);
                db.SaveChanges();
                return cOA_Combinations.C_ID;
            }

        }
    }
}
