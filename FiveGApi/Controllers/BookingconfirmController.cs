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


        private FiveG_DBEntities db = new FiveG_DBEntities();

        // GET: api/BookingConfirm
        [ResponseType(typeof(IQueryable<BookingConfirm>))]
        public IQueryable<BookingConfirm> GetBookingConfAll()
        {
            return db.BookingConfirms;
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
            BookingPaymentDetails(id, BookingConfirm);


            // db.Entry(BookingConfirm).State = EntityState.Modified;
            existBookingConfirm.Updated_On = DateTime.Now.ToString();

            try
            {
                db.SaveChanges();
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
            return StatusCode(HttpStatusCode.NoContent);
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

