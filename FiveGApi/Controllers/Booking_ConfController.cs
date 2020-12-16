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
    public class Booking_ConfController : ApiController
    {
        private FiveG_DBEntities db = new FiveG_DBEntities();

        // GET: api/Booking_Confirm
        [ResponseType(typeof(IQueryable<Booking_Confirm>))]
        public IQueryable<Booking_Confirm> AllBooking_Confirm(int RID)
        {
            return db.Booking_Confirm;
        }

        // GET: api/Booking_Confirm/5
        [ResponseType(typeof(Booking_Confirm))]
        public IHttpActionResult GetBooking_Confirm(int id)
        {
            Booking_Confirm Booking_Confirm = db.Booking_Confirm.Find(id);

            if (Booking_Confirm == null)
            {
                return NotFound();
            }

            return Ok(Booking_Confirm);
        }

        // PUT: api/Booking_Confirm/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooking_Confirm(int id, Booking_Confirm Booking_Confirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existBooking_Confirm = db.Booking_Confirm.Where(x => x.ID == id).FirstOrDefault();
            ////existBooking_Confirm.Booking_ConfirmDetails
            //if (existBooking_Confirm.Booking_ConfirmDetails.Count > 0)
            //{
            //    foreach (var item in existBooking_Confirm.Booking_ConfirmDetails.ToList())
            //    {
            //        existBooking_Confirm.Booking_ConfirmDetails.Remove(item);
            //    }
            //}

            //existBooking_Confirm.Booking_ConfirmDetails = Booking_Confirm.Booking_ConfirmDetails;



            // db.Entry(Booking_Confirm).State = EntityState.Modified;
            existBooking_Confirm.Updated_On = DateTime.Now.ToString();
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Booking_ConfirmExists((int)Booking_Confirm.ID))
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

        // POST: api/Booking_Confirm
        [ResponseType(typeof(Booking_Confirm))]
        public IHttpActionResult PostBooking_Confirm(Booking_Confirm Booking_Confirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Booking_Confirm.Add(Booking_Confirm);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Booking_Confirm.ID }, Booking_Confirm);
        }

        // DELETE: api/Booking_Confirm/5
        [ResponseType(typeof(Booking_Confirm))]
        public IHttpActionResult DeleteBooking_Confirm(int id)
        {
            Booking_Confirm Booking_Confirm = db.Booking_Confirm.Find(id);
            if (Booking_Confirm == null)
            {
                return NotFound();
            }

            db.Booking_Confirm.Remove(Booking_Confirm);
            db.SaveChanges();

            return Ok(Booking_Confirm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Booking_ConfirmExists(int id)
        {
            return db.Booking_Confirm.Count(e => e.ID == id) > 0;
        }
    }
}
