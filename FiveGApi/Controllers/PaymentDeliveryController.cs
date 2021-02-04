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
    [Authorize]

    [RoutePrefix("api/PaymentDelivery")]
        public class PaymentDeliveryController : ApiController
        {
            private MIS_DBEntities1 db = new MIS_DBEntities1();
            // GET: api/Payment_Delivery
            [ResponseType(typeof(IQueryable<Payment_Delivery>))]
            [Route("GetALLPayment_Delivery")]
            [HttpGet]
            public IQueryable<Payment_Delivery> GetALLPayment_Delivery()
            {
                return db.Payment_Delivery;
            }

            // GET: api/Payment_Delivery/5
            [ResponseType(typeof(Payment_Delivery))]
            public IHttpActionResult GetPayment_DeliveryByID(int id)
            {
                Payment_Delivery Payment_Delivery = db.Payment_Delivery.Find(id);

                if (Payment_Delivery == null)
                {
                    return NotFound();
                }

                return Ok(Payment_Delivery);
            }

            // PUT: api/Payment_Delivery/5
            [ResponseType(typeof(void))]
            public IHttpActionResult PutPayment_Delivery(int id, Payment_Delivery Payment_Delivery)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existPayment_Delivery = db.Payment_Delivery.Where(x => x.Payment_ID == id).FirstOrDefault();
                existPayment_Delivery.Ins_ID = Payment_Delivery.Ins_ID;               
                existPayment_Delivery.Unit_ID = Payment_Delivery.Unit_ID;
                existPayment_Delivery.Payment_amount = Payment_Delivery.Payment_amount;
                existPayment_Delivery.Instrument_Type = Payment_Delivery.Instrument_Type;
                existPayment_Delivery.instrument_number = Payment_Delivery.instrument_number;
                existPayment_Delivery.instrument_bank = Payment_Delivery.instrument_bank;
                existPayment_Delivery.instrument_bank_Branch = Payment_Delivery.instrument_bank_Branch;
                existPayment_Delivery.instrument_date = Payment_Delivery.instrument_date;
                existPayment_Delivery.instrument_remarks = Payment_Delivery.instrument_remarks;              
                
            try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Payment_DeliveryExists((int)Payment_Delivery.Payment_ID))
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

            // POST: api/Payment_Delivery
            [ResponseType(typeof(Payment_Delivery))]
            public IHttpActionResult PostPayment_Delivery(Payment_Delivery Payment_Delivery)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            
                db.Payment_Delivery.Add(Payment_Delivery);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = Payment_Delivery.Payment_ID }, Payment_Delivery);
            }

            // DELETE: api/Payment_Delivery/5
            [ResponseType(typeof(Payment_Delivery))]
            public IHttpActionResult DeletePayment_Delivery(int id)
            {
                Payment_Delivery Payment_Delivery = db.Payment_Delivery.Find(id);
                if (Payment_Delivery == null)
                {
                    return NotFound();
                }

                db.Payment_Delivery.Remove(Payment_Delivery);
                db.SaveChanges();

                return Ok(Payment_Delivery);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool Payment_DeliveryExists(int id)
            {
                return db.Payment_Delivery.Count(e => e.Payment_ID == id) > 0;
            }
        }
    }