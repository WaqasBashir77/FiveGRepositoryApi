using FiveGApi.DTOModels;
using FiveGApi.Helper;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FiveGApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
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
            try
            {
                if (propertySale != null)
                {

                    PropertySale OriginalPropertySale = new PropertySale();
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
                    OriginalPropertySale.Nominee_Picture = propertySale.Nominee_Picture;
                    if (propertySale.Purchaser_Picture != "")
                    {
                        string[] image = propertySale.Purchaser_Picture.Split(',');
                        OriginalPropertySale.Purchaser_Picture = Convert.FromBase64String(image[1]);

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
                        db.PropertySales.Add(OriginalPropertySale);
                    }
                    db.SaveChanges();
                    response.Code = 1;
                }
            }
            catch (Exception ex)
            {
                response.Code = 0;
            }
            return Ok(response);
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
                    gL_Headers.Description = projects.projectCode + "-" + projectDetail.unitNumber + "-" + projectDetail.unitType ;
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
        public IHttpActionResult updatePropertySale(PropertySale UpdatedObj)
        {

            try
            {
                var MasterObj = db.PropertySales.Where(x => x.Booking_ID == UpdatedObj.Booking_ID).FirstOrDefault();
                if (MasterObj != null)
                {
                    MasterObj.Buyer_Name = UpdatedObj.Buyer_Name;
                    MasterObj.Buyer_Father_Name = UpdatedObj.Buyer_Father_Name;
                    MasterObj.CNIC = UpdatedObj.CNIC;
                    MasterObj.Dealer_Comm = UpdatedObj.Dealer_Comm;
                    MasterObj.Dealer_ID = UpdatedObj.Dealer_ID;
                    MasterObj.Discount_Amount = UpdatedObj.Discount_Amount;
                    MasterObj.Email = UpdatedObj.Email;
                    MasterObj.Employee = UpdatedObj.Employee;
                    MasterObj.Employee_Com = UpdatedObj.Employee_Com;
                    MasterObj.Member_Reg_No = UpdatedObj.Member_Reg_No;
                    MasterObj.Mobile_1 = UpdatedObj.Mobile_1;
                    MasterObj.Mobile_2 = UpdatedObj.Mobile_2;
                    MasterObj.Nominee_CNIC = UpdatedObj.Nominee_CNIC;
                    MasterObj.Nominee_G_Number = UpdatedObj.Nominee_G_Number;
                    MasterObj.Nominee_Name = UpdatedObj.Nominee_Name;
                    MasterObj.Purchaser_Picture = UpdatedObj.Purchaser_Picture;
                    MasterObj.Relationship_With_Nominee = UpdatedObj.Relationship_With_Nominee;
                    MasterObj.Address = UpdatedObj.Address;
                    MasterObj.Sale_Status = UpdatedObj.Sale_Status;
                    MasterObj.Description = UpdatedObj.Description;
                    MasterObj.Updated_By = UpdatedObj.Updated_By;
                    MasterObj.Updated_On = DateTime.Now;
                    MasterObj.differentiableAmount = UpdatedObj.differentiableAmount;
                    MasterObj.Nominee_Picture = UpdatedObj.Nominee_Picture;
                    foreach (var saleitem in UpdatedObj.SaleInstallments.ToList())
                    {
                        SaleInstallment saleInstallment = new SaleInstallment();
                        if (saleitem.Booking_ID > 0)
                        {
                            saleInstallment = db.SaleInstallments.Where(x => x.Booking_ID == saleitem.Booking_ID && x.Ins_ID == saleitem.Ins_ID && x.Project_ID == saleitem.Project_ID).FirstOrDefault();

                            saleInstallment.ins_due_date = saleitem.ins_due_date;
                            saleInstallment.ins_payment_status = saleitem.ins_payment_status;
                            saleInstallment.ins_latesurcharge_amount = saleitem.ins_latesurcharge_amount;
                            saleInstallment.ins_balance = saleitem.ins_balance;
                            saleInstallment.OtherTaxAmount = saleitem.OtherTaxAmount;
                            saleInstallment.Updated_By = UpdatedObj.Updated_By;
                            saleInstallment.Updated_On = DateTime.Now;
                        }
                        else
                        {
                            
                            saleInstallment.Booking_ID = UpdatedObj.Booking_ID;
                            saleInstallment.Unit_ID = UpdatedObj.Unit_ID;
                            saleInstallment.Project_ID = UpdatedObj.Project_ID;
                            saleInstallment.ins_total_amount = saleitem.ins_total_amount;
                            saleInstallment.ins_remaining = saleitem.ins_remaining;
                            saleInstallment.ins_milestone_percentage = saleitem.ins_milestone_percentage;
                            saleInstallment.ins_latesurcharge_amount = saleitem.ins_latesurcharge_amount;
                            saleInstallment.ins_due_date = saleitem.ins_due_date;
                            saleInstallment.ins_balance = saleitem.ins_balance;
                            saleInstallment.OtherTaxAmount = saleitem.OtherTaxAmount;
                            saleInstallment.ins_payment_status = saleitem.ins_payment_status;
                            saleInstallment.Updated_By = UpdatedObj.Updated_By;
                            saleInstallment.Updated_On = DateTime.Now;
                            db.SaleInstallments.Add(saleInstallment);

                        }
                        

                        foreach (var item in saleitem.PaymentInstallments.ToList())
                        {
                            if (item.Payment_ID == 0)
                            {
                                PaymentInstallment paymentInstallment = new PaymentInstallment();
                                paymentInstallment.Project_ID = UpdatedObj.Project_ID;
                                paymentInstallment.Unit_ID = UpdatedObj.Unit_ID;
                                paymentInstallment.Payment_amount = item.Payment_amount;
                                paymentInstallment.Instrument_Type = item.Instrument_Type;
                                paymentInstallment.Ins_ID = saleInstallment.Ins_ID;
                                paymentInstallment.Booking_ID = UpdatedObj.Booking_ID;
                                paymentInstallment.instrument_bank = item.instrument_bank;
                                paymentInstallment.instrument_bank_Branch = item.instrument_bank_Branch;
                                paymentInstallment.instrument_date = Convert.ToDateTime(item.instrument_date);
                                paymentInstallment.instrument_number = item.instrument_number;
                                paymentInstallment.instrument_remarks = item.instrument_remarks;
                                paymentInstallment.Created_By = UpdatedObj.Updated_By;
                                paymentInstallment.Created_ON = DateTime.Now; 

                                db.PaymentInstallments.Add(paymentInstallment);
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
