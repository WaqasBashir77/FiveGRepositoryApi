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
        public class SocietySlipController : ApiController
        {
            private MIS_DBEntities1 db = new MIS_DBEntities1();

            // GET: api/Society_Slip
            [ResponseType(typeof(IQueryable<Society_Slip>))]
            [Route("GetALLSociety_Slip")]
            [HttpGet]
            public IQueryable<Society_Slip> GetALLSociety_Slip()
            {
                return db.Society_Slip;
            }

            // GET: api/Society_Slip/5
            [ResponseType(typeof(Society_Slip))]
            public IHttpActionResult GetSociety_SlipByID(int id)
            {
                Society_Slip Society_Slip = db.Society_Slip.Find(id);

                if (Society_Slip == null)
                {
                    return NotFound();
                }

                return Ok(Society_Slip);
            }
        [Route("GetSocietyValidation")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult GetSocietyValidation(string slipNum)
        {
            var Reg = db.Society_Slip.Where(x => x.Slip_num == slipNum).FirstOrDefault();
            if (Reg != null)
            {
                var error = new { message = "Slip number must be unique" }; //<-- anonymous object
                return this.Content(HttpStatusCode.Conflict, error);
            }

            return Ok(slipNum);
        }

        // PUT: api/Society_Slip/5
        [ResponseType(typeof(void))]
            public IHttpActionResult PutSociety_Slip(int id, Society_Slip Society_Slip)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existSociety_Slip = db.Society_Slip.Where(x => x.ID == id).FirstOrDefault();
                existSociety_Slip.Ref_num = Society_Slip.Ref_num;
                existSociety_Slip.Form_num = Society_Slip.Form_num;
                existSociety_Slip.Slip_Type = Society_Slip.Slip_Type;
                existSociety_Slip.Slip_Status = Society_Slip.Slip_Status;
                existSociety_Slip.Slip_num = Society_Slip.Slip_num;
                existSociety_Slip.Slip_Date = Society_Slip.Slip_Date;
                existSociety_Slip.Deliver_Date = Society_Slip.Deliver_Date;
                existSociety_Slip.Deliver_Name = Society_Slip.Deliver_Name;
                existSociety_Slip.Deliver_Contact = Society_Slip.Deliver_Contact;
                existSociety_Slip.MS_number = Society_Slip.MS_number;
                existSociety_Slip.Receipt_Amount = Society_Slip.Receipt_Amount;
                existSociety_Slip.Letter_Status = Society_Slip.Letter_Status;
            existSociety_Slip.Remarks = Society_Slip.Remarks;
                existSociety_Slip.Flex_1 = Society_Slip.Flex_1;
                existSociety_Slip.Flex_2 = Society_Slip.Flex_2;                
                existSociety_Slip.Updated_ON = DateTime.Now;
                existSociety_Slip.Delivered_Date = Society_Slip.Delivered_Date;
            existSociety_Slip.Updated_By = "1";
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Society_SlipExists((int)Society_Slip.ID))
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

            // POST: api/Society_Slip
            [ResponseType(typeof(Society_Slip))]
            public IHttpActionResult PostSociety_Slip(Society_Slip Society_Slip)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            Society_Slip.Created_By = "1";
            Society_Slip.Created_ON = DateTime.Now;
            Society_Slip.Flex_1 = "1";
            Society_Slip.Flex_2 = "1";


            db.Society_Slip.Add(Society_Slip);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = Society_Slip.ID }, Society_Slip);
            }

            // DELETE: api/Society_Slip/5
            [ResponseType(typeof(Society_Slip))]
            public IHttpActionResult DeleteSociety_Slip(int id)
            {
                Society_Slip Society_Slip = db.Society_Slip.Find(id);
                if (Society_Slip == null)
                {
                    return NotFound();
                }

                db.Society_Slip.Remove(Society_Slip);
                db.SaveChanges();

                return Ok(Society_Slip);
            }
        [Route("GetListSocietySlipByRef_numOrForm_num")]
        [ResponseType(typeof(IEnumerable<Society_Slip>))]
        public IHttpActionResult GetListSocietySlipByRef_numOrForm_num(string Ref_num, string Form_num)
        {
            IEnumerable<Society_Slip> Society_Slip;
            if (Ref_num == null)
            {
                Society_Slip = db.Society_Slip.Where(x => x.Form_num == Form_num).ToList();
            }
            else if (Form_num == null)
            {
                Society_Slip = db.Society_Slip.Where(x => x.Ref_num == Ref_num).ToList();

            }
            else
            {
                Society_Slip = db.Society_Slip.Where(x => x.Ref_num == Ref_num && x.Form_num == Form_num).ToList();

            }
            if (Society_Slip == null)
            {
                return NotFound();
            }

            return Ok(Society_Slip);
        }
        protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool Society_SlipExists(int id)
            {
                return db.Society_Slip.Count(e => e.ID == id) > 0;
            }
        }
    }
