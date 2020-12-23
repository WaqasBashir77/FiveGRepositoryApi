using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    [RoutePrefix("api/Bookingconfirm")]
    public class BookingconfirmController : ApiController
    {


        private MIS_DBEntities1 db = new MIS_DBEntities1();
        [Route("GetALLBookingConfirmed")]
        [HttpGet]       
        // GET: api/BookingConfirm
        [ResponseType(typeof(IQueryable<BookingConfirm>))]
        public IQueryable<BookingConfirm> GetALLBookingConfirmed()
        {
            IQueryable<BookingConfirm> bookingConfirm;

            try
            {
                bookingConfirm = db.BookingConfirms;
            }
            catch (Exception)
            {
                throw;
            }
            return bookingConfirm;
           
        }

        // GET: api/BookingConfirm/5
        [ResponseType(typeof(BookingConfirm))]
        public IHttpActionResult GetBookingConf(int id)
        {
            BookingConfirm BookingConfirm = db.BookingConfirms.Find(id);

            if (BookingConfirm == null)
            {
                return NotFound();
            }

            return Ok(BookingConfirm);
        }

        // PUT: api/BookingConfirm/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBookingConf(int id, BookingConfirm BookingConfirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existBookingConfirm = db.BookingConfirms.Where(x => x.ID == id).FirstOrDefault();
            
            existBookingConfirm.Ref_num = BookingConfirm.Ref_num;
            existBookingConfirm.Form_num = BookingConfirm.Form_num;
            existBookingConfirm.Authorize_Status = BookingConfirm.Authorize_Status;
            existBookingConfirm.File_Status = BookingConfirm.File_Status;
            existBookingConfirm.Replaced_Form = BookingConfirm.Replaced_Form;
            existBookingConfirm.Applicant_name = BookingConfirm.Applicant_name;
            existBookingConfirm.CNIC = BookingConfirm.CNIC;
            existBookingConfirm.Form_Rec_Date = BookingConfirm.Form_Rec_Date;
            existBookingConfirm.Contact_Num = BookingConfirm.Contact_Num;
            existBookingConfirm.Property_ID = BookingConfirm.Property_ID;
            existBookingConfirm.Member_Num = BookingConfirm.Member_Num;
            existBookingConfirm.Book_Emp  = BookingConfirm.Book_Emp;
            existBookingConfirm.Book_Dealer = BookingConfirm.Book_Emp;
            existBookingConfirm.Booking_Percent = BookingConfirm.Booking_Percent;
            existBookingConfirm.Booking_amount = BookingConfirm.Booking_amount;
            existBookingConfirm.Confirm_Percent = BookingConfirm.Confirm_Percent;
            existBookingConfirm.Confirm_amount = BookingConfirm.Confirm_amount;
            existBookingConfirm.MS_amount = BookingConfirm.MS_amount;
            existBookingConfirm.Total_amount = BookingConfirm.Total_amount;
            existBookingConfirm.Tax_Percent = BookingConfirm.Tax_Percent;
            existBookingConfirm.Rebate_Percent = BookingConfirm.Rebate_Percent;
            existBookingConfirm.Emp_Rebate = BookingConfirm.Emp_Rebate;
            existBookingConfirm.Dealer_Rebate = BookingConfirm.Dealer_Rebate;
            existBookingConfirm.Emp_B_RAmt = BookingConfirm.Emp_B_RAmt;
            existBookingConfirm.Emp_C_RAmt = BookingConfirm.Emp_C_RAmt;
            existBookingConfirm.Dealer_B_RAmt = BookingConfirm.Dealer_B_RAmt;
            existBookingConfirm.Dealer_C_RAmt = BookingConfirm.Dealer_C_RAmt;
            existBookingConfirm.Com_B_RAmt = BookingConfirm.Com_B_RAmt;
            existBookingConfirm.Com_C_RAmt = BookingConfirm.Com_C_RAmt;
            existBookingConfirm.Payment_B_Status = BookingConfirm.Payment_B_Status;
            existBookingConfirm.Payment_C_Status = BookingConfirm.Payment_C_Status;
            existBookingConfirm.Payment_MSFee_Status = BookingConfirm.Payment_B_Status;
            existBookingConfirm.Booking_Date = BookingConfirm.Booking_Date;
            existBookingConfirm.Confirmation_Date = BookingConfirm.Confirmation_Date;
            existBookingConfirm.MSFee_Date = BookingConfirm.MSFee_Date;
            existBookingConfirm.Remarks = BookingConfirm.Remarks;
            existBookingConfirm.Confirmation_Date = BookingConfirm.Confirmation_Date;            
            existBookingConfirm.Updated_By = BookingConfirm.Updated_By;
            existBookingConfirm.Updated_On = BookingConfirm.Updated_On;

            // db.Entry(BookingConfirm).State = EntityState.Modified;
            existBookingConfirm.Updated_On = DateTime.Now.ToString();

            try
            {
                db.SaveChanges();
                BookingPaymentDetails(id, BookingConfirm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingConfExists((int)BookingConfirm.ID))
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

        // POST: api/BookingConfirm
        [ResponseType(typeof(BookingConfirm))]
        public IHttpActionResult PostBookingConf(BookingConfirm BookingConfirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BookingConfirms.Add(BookingConfirm);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = BookingConfirm.ID }, BookingConfirm);
        }

        // DELETE: api/BookingConfirm/5
        [ResponseType(typeof(BookingConfirm))]
        public IHttpActionResult DeleteBookingConf(int id)
        {
            BookingConfirm BookingConfirm = db.BookingConfirms.Find(id);
            if (BookingConfirm == null)
            {
                return NotFound();
            }

            db.BookingConfirms.Remove(BookingConfirm);
            db.SaveChanges();

            return Ok(BookingConfirm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingConfExists(int id)
        {
            return db.BookingConfirms.Count(e => e.ID == id) > 0;
        }
        private IQueryable<BookingPayment> BookingPaymentDetails(int BookingID, BookingConfirm BookingConfirm)
        {


            foreach (var item in BookingConfirm.BookingPayments)
            {
                if (item.ID > 0)
                {
                    var existBookingPaymentExisted = db.BookingPayments.Where(x => x.ID == item.ID).FirstOrDefault();
                    existBookingPaymentExisted.Payment_mode = item.Payment_mode;
                    existBookingPaymentExisted.Instrument_Type = item.Instrument_Type;
                    existBookingPaymentExisted.Ins_Type = item.Ins_Type;
                    existBookingPaymentExisted.Payment_amount = item.Payment_amount;
                    existBookingPaymentExisted.instrument_number = item.instrument_number;
                    existBookingPaymentExisted.instrument_bank = item.instrument_bank;
                    existBookingPaymentExisted.instrument_bank_Branch = item.instrument_bank_Branch;
                    existBookingPaymentExisted.instrument_date = item.instrument_date;
                    existBookingPaymentExisted.instrument_remarks = item.instrument_remarks;
                    db.SaveChanges();
                }
                else
                {
                    item.ID = BookingID;
                    db.BookingPayments.Add(item);
                    db.SaveChanges();
                }

            }
            return db.BookingPayments.Where(x => x.ID == BookingID);
        }
    }
}

