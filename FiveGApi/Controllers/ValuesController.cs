using FiveGApi.DTOModels;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FiveGApi.Controllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        private FiveG_DBEntities db = new FiveG_DBEntities();

        [HttpGet]
        public IHttpActionResult GetPropertySaleLsit()
        {

            List<PropertySale> propertySale = db.PropertySales.ToList();

            if (propertySale == null)
            {
                return NotFound();
            }
            return Ok(propertySale);
        }

        // GET api/values
        [HttpGet]
        public IHttpActionResult GetPaymentMilestoneByProjectId(int id)
        {

            PaymentMilestone paymentMilestone = db.PaymentMilestones.Where(x => x.projectId == id).FirstOrDefault();
            List<TempTableForInstallment> tempTableFors = db.TempTableForInstallments.Where(x => x.ProjectId == id).ToList();
            List<TempTableForInstallmentDTO> tempTableForsDTO = new List<TempTableForInstallmentDTO>();
            foreach (var item in tempTableFors)
            {
                TempTableForInstallmentDTO temp = new TempTableForInstallmentDTO();
                temp.Id = item.Id;
                temp.Installment = item.Installment;
                temp.InstallmentType = item.InstallmentType;
                temp.Percentage = item.Percentage;
                temp.ProjectId = item.ProjectId;
                temp.paymentDetailDTOs = new PaymentDetailDTO();
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
                    OriginalPropertySale.Dealer_Comm = propertySale.dealerCommission;
                    OriginalPropertySale.Dealer_Name = propertySale.dealerName;
                    OriginalPropertySale.Address = propertySale.address;
                    OriginalPropertySale.Email = propertySale.address;
                    OriginalPropertySale.Relationship_With_Nominee = propertySale.relationWithNomine;
                    OriginalPropertySale.Sale_Status = propertySale.saleStatus;
                    OriginalPropertySale.Nominee_Name = propertySale.nomineeName;
                    OriginalPropertySale.Nominee_CNIC = propertySale.nomineeCnic;
                    OriginalPropertySale.Discount_Amount = propertySale.discountAmount;
                    OriginalPropertySale.Nominee_G_Number = propertySale.nomineeGNumber;
                    OriginalPropertySale.CNIC = propertySale.cnic;
                    OriginalPropertySale.Employee = propertySale.employeeId;
                    OriginalPropertySale.Employee_Com = propertySale.employeeCommission;
                    string[] image = propertySale.Purchaser_Picture.Split(',');
                    // OriginalPropertySale.Purchaser_Picture = Convert.FromBase64String(image[1]);

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
                            saleInstallment.ins_due_date = item.dueDate;
                            saleInstallment.ins_payment_status = item.paymentStatus;
                            saleInstallment.ins_balance = item.balance;
                            saleInstallment.Booking_ID = OriginalPropertySale.Booking_ID;
                            saleInstallment.ins_amount = item.amount;
                            saleInstallment.ins_amount_tax = item.TaxAmount;
                            saleInstallment.ins_total_amount = item.totalAmount;
                            saleInstallment.ins_payment_status = "Paid";
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
                                paymentInstallment.instrument_date = Convert.ToDateTime(item.paymentDetailDTOs.InsturmentDate);
                                paymentInstallment.instrument_remarks = item.paymentDetailDTOs.paymentDescription;

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


        [HttpGet]
        [Route("getdetail")]
        public IHttpActionResult getdetail()
        {

            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();

            var re = Request;
            //HttpRequestMessage re = new HttpRequestMessage();
            var headers = re.Headers;
            int tempId = Convert.ToInt32(headers.GetValues("Id").First());
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
    }
}