using FiveGApi.DTOModels;
using FiveGApi.Helper;
using FiveGApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/BookingPayment")]
    public class BookingPaymentController : ApiController
    {

        private MIS_DBEntities1 db = new MIS_DBEntities1();
        private string UserId;
        private User userSecurityGroup = new User();

        public BookingPaymentController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;
            userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().FirstOrDefault();

        }
        // GET: api/Booking_Payments
        [ResponseType(typeof(IQueryable<BookingPayment>))]
        public IQueryable<BookingPayment> GetBookingPaymentsAll()//[FromUri] PagingParameterModel pagingparametermodel)
        {
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                return db.BookingPayments.Where(x => x.SecurityGroupId == userSecurityGroup.SecurityGroupId);
            else
                return db.BookingPayments;
            //// Get's No of Rows Count   
            //int count = source.Count();

            //// Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            //int CurrentPage = pagingparametermodel.pageNumber;

            //// Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            //int PageSize = pagingparametermodel.pageSize;

            //// Display TotalCount to Records to User  
            //int TotalCount = count;

            //// Calculating Totalpage by Dividing (No of Records / Pagesize)  
            //int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            //// Returns List of Customer after applying Paging   
            //var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            //// if CurrentPage is greater than 1 means it has previousPage  
            //var previousPage = CurrentPage > 1 ? "Yes" : "No";

            //// if TotalPages is greater than CurrentPage means it has nextPage  
            //var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            //// Object which we are going to send in header   
            //var paginationMetadata = new
            //{
            //    totalCount = TotalCount,
            //    pageSize = PageSize,
            //    currentPage = CurrentPage,
            //    totalPages = TotalPages,
            //    previousPage,
            //    nextPage
            //};

            //// Setting Header  
            //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            //// Returing List of Customers Collections  
            //return (IQueryable<BookingPayment>)items;
        }

        // GET: api/Booking_Payments/5
        [Route("GetBooking_Payments")]
        [ResponseType(typeof(BookingPayment))]
        public IHttpActionResult GetBooking_Payments(int id)
        {
            BookingPayment bookingPayment = new BookingPayment();
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                bookingPayment = db.BookingPayments.Where(x=>x.ID==id&&x.SecurityGroupId==userSecurityGroup.SecurityGroupId).FirstOrDefault();
            else
                bookingPayment = db.BookingPayments.Find(id);

            if (bookingPayment == null)
            {
                return NotFound();
            }

            return Ok(bookingPayment);
        }
        [Route("GetBooking_PaymentsByConfirmationID")]
        [ResponseType(typeof(BookingPayment))]
        public IHttpActionResult GetBooking_PaymentsByConfirmationID(int id)
        {
            List<BookingPayment> Booking_Payments = new List<BookingPayment>();
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                Booking_Payments = db.BookingPayments.Where(x => x.ID == id && x.SecurityGroupId == userSecurityGroup.SecurityGroupId).ToList();
            else
                Booking_Payments=db.BookingPayments.Where(x => x.ID == id).ToList();

            //db.BookingPayments.Where(x=>x.ID==id).ToList();

            if (Booking_Payments == null)
            {
                return NotFound();
            }

            return Ok(Booking_Payments);
        }
        [Route("GetBooking_PaymentsByConfirmationIDandInsType")]
        [ResponseType(typeof(BookingPayment))]
        public IHttpActionResult GetBooking_PaymentsByConfirmationIDandInsType(int id,string instype)
        {
            List<BookingPayment> Booking_Payments = new List<BookingPayment>();
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                Booking_Payments = db.BookingPayments.Where(x => x.ID == id && x.Ins_Type == instype && x.SecurityGroupId == userSecurityGroup.SecurityGroupId).ToList();
            else
                Booking_Payments = db.BookingPayments.Where(x => x.ID == id && x.Ins_Type == instype).ToList();


            if (Booking_Payments == null)
            {
                return NotFound();
            }

            return Ok(Booking_Payments);
            //var Booking_Payments = db.BookingPayments.Where(x => x.ID == id && x.Ins_Type== instype).ToList();

           
        }
        [Route("AuthorizeBooking_Payments")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AuthorizeBooking_Payments(int PaymentID, BookingPayment Booking_Payments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existBooking_Payments = db.BookingPayments.Where(x => x.Payment_ID == PaymentID).FirstOrDefault();
            existBooking_Payments.Authorize_By = Booking_Payments.Authorize_By;
            existBooking_Payments.Authorize_Status = Booking_Payments.Authorize_Status;
            existBooking_Payments.Authorize_Date = Booking_Payments.Authorize_Date;
            if (existBooking_Payments != null)
            {
                try
                {
                    db.SaveChanges();
                    //BookingPaymentDetails(id, BookingConfirm);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingConfExists((int)existBooking_Payments.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var glheader = db.GL_Headers.Where(x => x.Source_Tran_Id ==existBooking_Payments.ID).FirstOrDefault();
                var bookingConfirm = db.BookingConfirms.Where(x => x.ID == existBooking_Payments.ID).FirstOrDefault();
                var propertydef = db.PropertyDefs.Where(x => x.ID == bookingConfirm.Property_ID).FirstOrDefault();
                GL_Headers gL_Headers = new GL_Headers();
                
                if (glheader == null)
                {                    
                    gL_Headers.J_Date = DateTime.Now;
                    gL_Headers.Doc_Date = DateTime.Now;
                    gL_Headers.Currency ="PKR";
                    gL_Headers.Description =existBooking_Payments.ID.ToString()+"-"+bookingConfirm.Applicant_name+"-"+ propertydef.Name;
                    gL_Headers.Remarks = "";
                    gL_Headers.Source =existBooking_Payments.Ins_Type;
                    gL_Headers.Trans_Status = "UnPosted";
                    gL_Headers.Source_Tran_Id = existBooking_Payments.ID;
                    gL_Headers.SecurityGroupId = userSecurityGroup.SecurityGroupId;
                    db.GL_Headers.Add(gL_Headers);

                    db.SaveChanges();
                }
                int glhv = 0;
                if(gL_Headers.H_ID>0)
                {
                    glhv = gL_Headers.H_ID;
                }else
                {
                    glhv = glheader.H_ID;
                }
                var BookingEntriesPayment = db.Booking_Entries.Where(x => x.Flex_1 == existBooking_Payments.Payment_ID.ToString() && x.Entry_Type == "Payments" && x.Transaction_ID == existBooking_Payments.ID &&x.Status!= "Transferred").ToList();
                if (BookingEntriesPayment != null)
                {
                    foreach (var item in BookingEntriesPayment)
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
                        var be = db.Booking_Entries.Where(x => x.E_ID == item.E_ID).FirstOrDefault();
                        be.Status = "Transferred";
                        be.Updated_By = "1";
                        be.Updated_On = DateTime.Now;
                        db.SaveChanges();
                    } 
                    
                }
                return StatusCode(HttpStatusCode.OK);
            }
            else
            {
                var error = new { message = "Not Exist Entity against this" }; //<-- anonymous object
                return this.Content(HttpStatusCode.NotFound, error);
            }
        }
            // PUT: api/Booking_Payments/5
            [Route("PutBooking_Payments")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooking_Payments(int id, BookingPayment Booking_Payments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existBooking_Payments = db.BookingPayments.Where(x => x.Payment_ID == id).FirstOrDefault();
            if (existBooking_Payments != null)
            {
                existBooking_Payments.Ins_Type = Booking_Payments.Ins_Type;
                existBooking_Payments.Payment_mode = Booking_Payments.Payment_mode;
                existBooking_Payments.Payment_amount = Booking_Payments.Payment_amount;
                existBooking_Payments.Instrument_Type = Booking_Payments.Instrument_Type;
                existBooking_Payments.instrument_number = Booking_Payments.instrument_number;
                existBooking_Payments.instrument_bank = Booking_Payments.instrument_bank;
                existBooking_Payments.instrument_bank_Branch = Booking_Payments.instrument_bank_Branch;
                existBooking_Payments.instrument_date = Booking_Payments.instrument_date;
                existBooking_Payments.instrument_remarks = Booking_Payments.instrument_remarks;
                existBooking_Payments.Authorize_Status = Booking_Payments.Authorize_Status;
                existBooking_Payments.Authorize_By = Booking_Payments.Authorize_By;
                existBooking_Payments.Authorize_Date = Booking_Payments.Authorize_Date;
                existBooking_Payments.SecurityGroupId = userSecurityGroup.SecurityGroupId;
                try
                {
                    db.SaveChanges();
                    var existBookingConfirm = db.BookingConfirms.Where(x => x.ID == Booking_Payments.ID).FirstOrDefault();
                    var property = db.PropertyDefs.Where(x => x.ID == existBookingConfirm.Property_ID).FirstOrDefault();
                    if (Booking_Payments.Ins_Type == "Booking")
                    {
                         existBookingConfirm.Booking_Date = DateTime.Now;
                        var totalPayments = db.BookingPayments.Where(xx => xx.ID == Booking_Payments.ID && xx.Ins_Type == "Booking");
                        decimal totalPaymentsClculation = 0;
                        foreach (var item in totalPayments)
                        {
                            totalPaymentsClculation = (decimal)(totalPaymentsClculation + item.Payment_amount);

                        }
                        var PaymentPercentage = (totalPaymentsClculation / existBookingConfirm.Total_amount) * 100;
                        if (PaymentPercentage == 0)
                        {
                            existBookingConfirm.Payment_B_Status = "unPaid";
                        }
                        else if (existBookingConfirm.Booking_Percent > PaymentPercentage)
                        {
                            existBookingConfirm.Payment_B_Status = "partialPaid";

                        }
                        else
                        {
                            existBookingConfirm.Payment_B_Status = "paid";
                            existBookingConfirm.Booking_Date = DateTime.Now;

                        }
                    }
                    else if (Booking_Payments.Ins_Type == "Confirmation")
                    {
                        var totalPayments = db.BookingPayments.Where(xx => xx.ID == Booking_Payments.ID && xx.Ins_Type == "Confirmation");
                        decimal totalPaymentsClculation = 0;
                        foreach (var item in totalPayments)
                        {
                            totalPaymentsClculation = (decimal)(totalPaymentsClculation + item.Payment_amount);
                        }
                        var PaymentPercentage = (totalPaymentsClculation / existBookingConfirm.Total_amount) * 100;

                        if (PaymentPercentage == 0)
                        {
                            existBookingConfirm.Payment_C_Status = "unPaid";
                        }
                        else if (existBookingConfirm.Confirm_Percent > PaymentPercentage)
                        {
                            existBookingConfirm.Payment_C_Status = "partialPaid";
                        }
                        else
                        {
                            existBookingConfirm.Payment_C_Status = "paid";
                            existBookingConfirm.Confirmation_Date = DateTime.Now;

                        }

                    }
                    else
                    {
                       
                        var totalPayments = db.BookingPayments.Where(xx => xx.ID == Booking_Payments.ID && xx.Ins_Type == "MsFee");
                        decimal totalPaymentsClculation = 0;
                        foreach (var item in totalPayments)
                        {
                            totalPaymentsClculation = (decimal)(totalPaymentsClculation + item.Payment_amount);
                        }
                       
                        if (totalPaymentsClculation == 0)
                        {
                            existBookingConfirm.Payment_MSFee_Status = "unPaid";
                        }
                        else if (existBookingConfirm.MS_amount > totalPaymentsClculation)
                        {
                            existBookingConfirm.Payment_MSFee_Status = "partialPaid";
                        }
                        else
                        {
                            existBookingConfirm.MSFee_Date = DateTime.Now;
                            existBookingConfirm.Payment_MSFee_Status = "paid";

                        }
                       
                    }
                    try
                    {
                        db.SaveChanges();
                        //BookingPaymentDetails(id, BookingConfirm);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BookingConfExists((int)existBookingConfirm.ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingPaymentsExists((int)Booking_Payments.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                var error = new { message = "Not Exist Entity against this" }; //<-- anonymous object
                return this.Content(HttpStatusCode.NotFound, error);
            }
        }

        // POST: api/Booking_Payments
        [ResponseType(typeof(BookingPayment))]
        public IHttpActionResult PostBooking_Payments(BookingPayment Booking_Payments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Booking_Payments.Authorize_Status = "No";
            Booking_Payments.SecurityGroupId =userSecurityGroup.SecurityGroupId;
            db.BookingPayments.Add(Booking_Payments);
            db.SaveChanges();
            var existBookingConfirm = db.BookingConfirms.Where(x => x.ID == Booking_Payments.ID).FirstOrDefault();
            var property = db.PropertyDefs.Where(x => x.ID == existBookingConfirm.Property_ID).FirstOrDefault();
            if (Booking_Payments.Ins_Type== "Booking")
            {
                existBookingConfirm.Booking_Date = DateTime.Now;
                var totalPayments = db.BookingPayments.Where(xx => xx.ID == Booking_Payments.ID && xx.Ins_Type == "Booking");
                decimal totalPaymentsClculation = 0;
                foreach (var item in totalPayments)
                {
                    totalPaymentsClculation = (decimal)(totalPaymentsClculation + item.Payment_amount);

                }
                var PaymentPercentage = (totalPaymentsClculation / existBookingConfirm.Total_amount)*100;
                if(PaymentPercentage == 0)
                {
                    existBookingConfirm.Payment_B_Status = "unPaid";
                }
                else if(existBookingConfirm.Booking_Percent> PaymentPercentage)
                {
                    existBookingConfirm.Payment_B_Status = "partialPaid";

                }
                else
                {
                    existBookingConfirm.Payment_B_Status = "paid";
                    existBookingConfirm.Booking_Date = DateTime.Now;

                }
            }
            else if (Booking_Payments.Ins_Type == "Confirmation")
            {
                var totalPayments = db.BookingPayments.Where(xx => xx.ID == Booking_Payments.ID && xx.Ins_Type == "Confirmation");
                decimal totalPaymentsClculation = 0;
                foreach (var item in totalPayments)
                {
                    totalPaymentsClculation = (decimal)(totalPaymentsClculation + item.Payment_amount);
                }
                var PaymentPercentage = (totalPaymentsClculation / existBookingConfirm.Total_amount) * 100;

                if (PaymentPercentage == 0)
                {
                    existBookingConfirm.Payment_C_Status = "unPaid";
                }
                else if (existBookingConfirm.Confirm_Percent > PaymentPercentage)
                {
                    existBookingConfirm.Payment_C_Status = "partialPaid";
                }
                else
                {
                    existBookingConfirm.Payment_C_Status = "paid";
                    existBookingConfirm.Confirmation_Date = DateTime.Now;

                }

            }
            else
            {
                var totalPayments = db.BookingPayments.Where(xx => xx.ID == Booking_Payments.ID && xx.Ins_Type == "MsFee");
                decimal totalPaymentsClculation = 0;
                foreach (var item in totalPayments)
                {
                    totalPaymentsClculation = (decimal)(totalPaymentsClculation + item.Payment_amount);
                }

                if (totalPaymentsClculation == 0)
                {
                    existBookingConfirm.Payment_MSFee_Status = "unPaid";
                }
                else if (existBookingConfirm.MS_amount > totalPaymentsClculation)
                {
                    existBookingConfirm.Payment_MSFee_Status = "partialPaid";
                }
                else
                {
                    existBookingConfirm.MSFee_Date = DateTime.Now;
                    existBookingConfirm.Payment_MSFee_Status = "paid";

                }
            }
            try
            {
                db.SaveChanges();
                //BookingPaymentDetails(id, BookingConfirm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingConfExists((int)existBookingConfirm.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }            
                var bankAccount = db.Bank_Accounts.Where(x => x.ID == Booking_Payments.Payment_Account).FirstOrDefault();
                var combination = db.COA_Combinations.Where(x => x.C_ID == bankAccount.GL_Mapping).FirstOrDefault();
                Booking_Entries booking_EntriesforROS = new Booking_Entries();
                booking_EntriesforROS.Transaction_ID = Booking_Payments.ID;
                booking_EntriesforROS.Entry_Date = DateTime.Now;
                booking_EntriesforROS.Entry_Type = "Payments";
                booking_EntriesforROS.Created_By = "Admin";
                booking_EntriesforROS.Created_On = DateTime.Now;
                booking_EntriesforROS.Status = "Draft";
                booking_EntriesforROS.C_CODE = combination.C_ID.ToString();
                booking_EntriesforROS.Debit = Booking_Payments.Payment_amount;
                booking_EntriesforROS.Credit = 0;
                booking_EntriesforROS.Flex_1 = Booking_Payments.Payment_ID.ToString();
                db.Booking_Entries.Add(booking_EntriesforROS);
                db.SaveChanges();
                var c = property.Company + "." + property.Project + "." + property.Location + "." + property.PayableToSociet + ".0000";
                Booking_Entries booking_EntriesforROS2 = new Booking_Entries();
                booking_EntriesforROS2.Transaction_ID = Booking_Payments.ID;
                booking_EntriesforROS2.Entry_Date = DateTime.Now;
                booking_EntriesforROS2.Entry_Type = "Payments";
                booking_EntriesforROS2.Created_By = "Admin";
                booking_EntriesforROS2.Created_On = DateTime.Now;
                booking_EntriesforROS2.Status = "Draft";
                booking_EntriesforROS2.C_CODE = getCoaID(c).ToString();
                booking_EntriesforROS2.Credit = Booking_Payments.Payment_amount;
                booking_EntriesforROS2.Debit = 0;
                booking_EntriesforROS2.Flex_1 = Booking_Payments.Payment_ID.ToString();
                db.Booking_Entries.Add(booking_EntriesforROS2);
                db.SaveChanges();
           
            return CreatedAtRoute("DefaultApi", new { id = Booking_Payments.ID }, Booking_Payments);
        }
        [NonAction]
        private int getCoaID(string CCode)
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
                cOA_Combinations.Created_By = userSecurityGroup.UserName;
                cOA_Combinations.Created_ON = DateTime.Now;
                db.COA_Combinations.Add(cOA_Combinations);
                db.SaveChanges();
                return cOA_Combinations.C_ID;
            }
}
// DELETE: api/Booking_Payments/5
[ResponseType(typeof(BookingPayment))]
        public IHttpActionResult DeleteBooking_Payments(int id)
        {
            BookingPayment Booking_Payments = db.BookingPayments.Find(id);
            if (Booking_Payments == null)
            {
                return NotFound();
            }
            db.BookingPayments.Remove(Booking_Payments);
            db.SaveChanges();
            return Ok(Booking_Payments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingPaymentsExists(int id)
        {
            return db.BookingPayments.Count(e => e.ID == id) > 0;
        }
        private bool BookingConfExists(int id)
        {
            return db.BookingConfirms.Count(e => e.ID == id) > 0;
        }
    }
}