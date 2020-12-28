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
        [Route("GetALLBookingConfirmedByFormNumberOrFormID")]
        [HttpGet]
        // GET: api/BookingConfirm
        [ResponseType(typeof(IQueryable<BookingConfirm>))]
        public IQueryable<BookingConfirm> GetALLBookingConfirmedByFormNumberOrFormID(string refNumber,string formNumber)
        {
            IQueryable<BookingConfirm> bookingConfirm;

            try
            {
                if(refNumber==null)
                {
                    bookingConfirm = db.BookingConfirms.Where(x=> x.Form_num == formNumber);

                }else if(formNumber==null)
                {
                    bookingConfirm = db.BookingConfirms.Where(x => x.Ref_num == refNumber);

                }
                else
                {
                    bookingConfirm = db.BookingConfirms.Where(x => x.Ref_num == refNumber || x.Form_num == formNumber);

                }
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
            if (existBookingConfirm != null)
            {
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
                existBookingConfirm.Book_Emp = BookingConfirm.Book_Emp;
                existBookingConfirm.Book_Dealer = BookingConfirm.Book_Dealer;
                existBookingConfirm.MS_amount = BookingConfirm.MS_amount;
                existBookingConfirm.Remarks = BookingConfirm.Remarks;
                existBookingConfirm.Confirmation_Date = BookingConfirm.Confirmation_Date;
                existBookingConfirm.Updated_By = BookingConfirm.Updated_By;
                existBookingConfirm.Updated_On = BookingConfirm.Updated_On;
                // db.Entry(BookingConfirm).State = EntityState.Modified;
                existBookingConfirm.Updated_On = DateTime.Now.ToString();

                try
                {
                    db.SaveChanges();
                    //BookingPaymentDetails(id, BookingConfirm);
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
            }else
            {
                var error = new { message = "Not Exist Entity against this" }; //<-- anonymous object
                return this.Content(HttpStatusCode.NotFound, error);
            }
        }

        // POST: api/BookingConfirm
        [ResponseType(typeof(BookingConfirm))]
        public IHttpActionResult PostBookingConf(BookingConfirm BookingConfirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var property = db.PropertyDefs.Where(x => x.ID == BookingConfirm.Property_ID).FirstOrDefault();
            BookingConfirm.Booking_Percent = property.Booking_percent;
            BookingConfirm.Booking_amount = ((property.Price / 100) * property.Booking_percent);
            BookingConfirm.Confirm_Percent = property.Confirm_percent;
            BookingConfirm.Confirm_amount = ((property.Price / 100) * property.Confirm_percent);
            BookingConfirm.Tax_Percent = property.Tax_percent;
            BookingConfirm.Total_amount = property.Price;
            BookingConfirm.Rebate_Percent = property.Rebate_percent;
            int employeeID = Convert.ToInt32(BookingConfirm.Book_Emp);
            var employeRebate=db.Rebate_Details.Where(x => x.Reg_ID == employeeID).Select(x => x.Rebate).FirstOrDefault();
            BookingConfirm.Emp_Rebate = employeRebate;
            int dealerID = Convert.ToInt32(BookingConfirm.Book_Dealer);
            var dealerRebate = db.Rebate_Details.Where(x => x.Reg_ID == dealerID).Select(x => x.Rebate).FirstOrDefault();
            BookingConfirm.Dealer_Rebate = dealerRebate;
            BookingConfirm.Emp_B_RAmt = 0;
            BookingConfirm.Emp_C_RAmt = 0;
            BookingConfirm.Dealer_B_RAmt = 0;
            BookingConfirm.Dealer_C_RAmt = 0;
            BookingConfirm.Com_B_RAmt = 0;
            BookingConfirm.Com_C_RAmt = 0;
            BookingConfirm.Flex_1 = "true";
            BookingConfirm.Flex_2 = "true";
            BookingConfirm.Created_By = "1";
            BookingConfirm.Payment_B_Status = "unPaid";
            BookingConfirm.Payment_C_Status = "unPaid";
            BookingConfirm.Payment_MSFee_Status = "unPaid";
            BookingConfirm.Flex_1 = "1";
            BookingConfirm.Flex_2 = "1";
            BookingConfirm.Created_By = "1";
            BookingConfirm.Created_ON = DateTime.Now;            
            //BookingConfirm.Emp_B_RAmt = (((property.Price / 100) * employeRebate) / 2);
            //BookingConfirm.Emp_C_RAmt = (((property.Price / 100) * employeRebate) / 2);
            //BookingConfirm.Dealer_B_RAmt = (((property.Price / 100) * dealerRebate) / 2);
            //BookingConfirm.Dealer_C_RAmt = (((property.Price / 100) * dealerRebate) / 2);
            //BookingConfirm.Com_B_RAmt = (((property.Price / 100) * employeRebate) / 2);
            //BookingConfirm.Com_C_RAmt = ((property.Price / 100) * property.Rebate_percent);
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
        //private IQueryable<BookingPayment> BookingPaymentDetails(int BookingID, BookingConfirm BookingConfirm)
        //{


        //    foreach (var item in BookingConfirm.BookingPayments)
        //    {
        //        if (item.ID > 0)
        //        {
        //            var existBookingPaymentExisted = db.BookingPayments.Where(x => x.ID == item.ID).FirstOrDefault();
        //            existBookingPaymentExisted.Payment_mode = item.Payment_mode;
        //            existBookingPaymentExisted.Instrument_Type = item.Instrument_Type;
        //            existBookingPaymentExisted.Ins_Type = item.Ins_Type;
        //            existBookingPaymentExisted.Payment_amount = item.Payment_amount;
        //            existBookingPaymentExisted.instrument_number = item.instrument_number;
        //            existBookingPaymentExisted.instrument_bank = item.instrument_bank;
        //            existBookingPaymentExisted.instrument_bank_Branch = item.instrument_bank_Branch;
        //            existBookingPaymentExisted.instrument_date = item.instrument_date;
        //            existBookingPaymentExisted.instrument_remarks = item.instrument_remarks;
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            item.ID = BookingID;
        //            db.BookingPayments.Add(item);
        //            db.SaveChanges();
        //        }

        //    }
        //    return db.BookingPayments.Where(x => x.ID == BookingID);
        //}
    }
}

