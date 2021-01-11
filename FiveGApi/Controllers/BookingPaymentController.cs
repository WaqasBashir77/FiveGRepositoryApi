using FiveGApi.DTOModels;
using FiveGApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    [RoutePrefix("api/BookingPayment")]
    public class BookingPaymentController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/Booking_Payments
        [ResponseType(typeof(IQueryable<BookingPayment>))]
        public IQueryable<BookingPayment> GetBookingPaymentsAll()//[FromUri] PagingParameterModel pagingparametermodel)
        {
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
            BookingPayment Booking_Payments = db.BookingPayments.Find(id);

            if (Booking_Payments == null)
            {
                return NotFound();
            }

            return Ok(Booking_Payments);
        }
        [Route("GetBooking_PaymentsByConfirmationID")]
        [ResponseType(typeof(BookingPayment))]
        public IHttpActionResult GetBooking_PaymentsByConfirmationID(int id)
        {
            var Booking_Payments = db.BookingPayments.Where(x=>x.ID==id).ToList();

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
            var Booking_Payments = db.BookingPayments.Where(x => x.ID == id && x.Ins_Type== instype).ToList();

            if (Booking_Payments == null)
            {
                return NotFound();
            }

            return Ok(Booking_Payments);
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
                try
                {
                    db.SaveChanges();
                    var existBookingConfirm = db.BookingConfirms.Where(x => x.ID == Booking_Payments.ID).FirstOrDefault();
                    var property = db.PropertyDefs.Where(x => x.ID == existBookingConfirm.Property_ID).FirstOrDefault();
                    if (Booking_Payments.Ins_Type == "Booking")
                    {
                        existBookingConfirm.Emp_B_RAmt = (((property.Price / 100) * existBookingConfirm.Emp_Rebate) / 2);
                        existBookingConfirm.Dealer_B_RAmt = (((property.Price / 100) * existBookingConfirm.Dealer_Rebate) / 2);
                        existBookingConfirm.Com_B_RAmt = (((property.Price / 100) * existBookingConfirm.Rebate_Percent));
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
                        existBookingConfirm.Emp_C_RAmt = (((property.Price / 100) * existBookingConfirm.Emp_Rebate) / 2);
                        existBookingConfirm.Dealer_C_RAmt = (((property.Price / 100) * existBookingConfirm.Dealer_C_RAmt) / 2);
                        existBookingConfirm.Com_C_RAmt = (((property.Price / 100) * existBookingConfirm.Com_C_RAmt));
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
            db.BookingPayments.Add(Booking_Payments);
            db.SaveChanges();
            var existBookingConfirm = db.BookingConfirms.Where(x => x.ID == Booking_Payments.ID).FirstOrDefault();
            var property = db.PropertyDefs.Where(x => x.ID == existBookingConfirm.Property_ID).FirstOrDefault();
            if (Booking_Payments.Ins_Type== "Booking")
            {
                existBookingConfirm.Emp_B_RAmt = (((property.Price / 100) * existBookingConfirm.Emp_Rebate)/2);
                existBookingConfirm.Dealer_B_RAmt = (((property.Price / 100) * existBookingConfirm.Dealer_Rebate)/2);
                existBookingConfirm.Com_B_RAmt = (((property.Price / 100) * existBookingConfirm.Rebate_Percent) );
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
                existBookingConfirm.Emp_C_RAmt = (((property.Price / 100) * existBookingConfirm.Emp_Rebate)/2);
                existBookingConfirm.Dealer_C_RAmt = (((property.Price / 100) * existBookingConfirm.Dealer_Rebate)/2);
                existBookingConfirm.Com_C_RAmt = (((property.Price / 100) * existBookingConfirm.Rebate_Percent));               
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
            return CreatedAtRoute("DefaultApi", new { id = Booking_Payments.ID }, Booking_Payments);
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