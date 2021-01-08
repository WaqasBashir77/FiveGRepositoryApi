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
    [RoutePrefix("api/DeliverySheet")]
    public class DeliverySheetController : ApiController
    {
        

       
            private MIS_DBEntities1 db = new MIS_DBEntities1();

            // GET: api/Delivery_Sheet
            [ResponseType(typeof(IQueryable<Delivery_Sheet>))]
            [Route("GetALLDelivery_Sheet")]
            [HttpGet]
            public IQueryable<Delivery_Sheet> GetALLDeliverySheet()
            {
                return db.Delivery_Sheet;
            }

            // GET: api/Delivery_Sheet/5
            [ResponseType(typeof(Delivery_Sheet))]
            public IHttpActionResult GetDelivery_SheetByID(int id)
            {
                Delivery_Sheet Delivery_Sheet = db.Delivery_Sheet.Find(id);

                if (Delivery_Sheet == null)
                {
                    return NotFound();
                }

                return Ok(Delivery_Sheet);
            }
        // GET: api/Delivery_Sheet/5
        [Route("GetListDeliverySheetByRef_numOrForm_num")]
        [ResponseType(typeof(IEnumerable<Delivery_Sheet>))]
        public IHttpActionResult GetListDeliverySheetByRef_numOrForm_num(string Ref_num,string Form_num)
        {
            IEnumerable<Delivery_Sheet> Delivery_Sheet;
            if (Ref_num==null)
            {
                Delivery_Sheet = db.Delivery_Sheet.Where(x => x.Form_num == Form_num).ToList();
            }
            else if(Form_num==null)
            {
                Delivery_Sheet = db.Delivery_Sheet.Where(x => x.Ref_num == Ref_num).ToList();

            }
            else
            {
                Delivery_Sheet = db.Delivery_Sheet.Where(x => x.Ref_num == Ref_num && x.Form_num == Form_num).ToList();

            }
            if (Delivery_Sheet == null)
            {
                return NotFound();
            }

            return Ok(Delivery_Sheet);
        }
        // PUT: api/Delivery_Sheet/5
        [ResponseType(typeof(void))]
            public IHttpActionResult PutDelivery_Sheet(int id, Delivery_Sheet Delivery_Sheet)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existDelivery_Sheet = db.Delivery_Sheet.Where(x => x.ID == id).FirstOrDefault();
            existDelivery_Sheet.Ref_num = Delivery_Sheet.Ref_num;
            existDelivery_Sheet.Form_num = Delivery_Sheet.Form_num;
            existDelivery_Sheet.Delivery_Type   = Delivery_Sheet.Delivery_Type;
            existDelivery_Sheet.Delivery_Date = Delivery_Sheet.Delivery_Date;
            existDelivery_Sheet.Delivery_To = Delivery_Sheet.Delivery_To;
            existDelivery_Sheet.Handover_Staff = Delivery_Sheet.Handover_Staff;
            existDelivery_Sheet.Handover_Date = Delivery_Sheet.Handover_Date;
            existDelivery_Sheet.Payment_ID = Delivery_Sheet.Payment_ID;
            existDelivery_Sheet.Remarks = Delivery_Sheet.Remarks;
            existDelivery_Sheet.Updated_By = "1";
            existDelivery_Sheet.Updated_On = DateTime.Now ;
            try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Delivery_SheetExists((int)Delivery_Sheet.ID))
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

            // POST: api/Delivery_Sheet
            [ResponseType(typeof(Delivery_Sheet))]
            public IHttpActionResult PostDelivery_Sheet(Delivery_Sheet Delivery_Sheet)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Delivery_Sheet.Flex_1 = "1";
                Delivery_Sheet.Flex_2 = "1";
                Delivery_Sheet.Created_By = "1";
                Delivery_Sheet.Created_On = DateTime.Now;
                db.Delivery_Sheet.Add(Delivery_Sheet);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = Delivery_Sheet.ID }, Delivery_Sheet);
            }

            // DELETE: api/Delivery_Sheet/5
            [ResponseType(typeof(Delivery_Sheet))]
            public IHttpActionResult DeleteDelivery_Sheet(int id)
            {
                Delivery_Sheet Delivery_Sheet = db.Delivery_Sheet.Find(id);
                if (Delivery_Sheet == null)
                {
                    return NotFound();
                }

                db.Delivery_Sheet.Remove(Delivery_Sheet);
                db.SaveChanges();

                return Ok(Delivery_Sheet);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool Delivery_SheetExists(int id)
            {
                return db.Delivery_Sheet.Count(e => e.ID == id) > 0;
            }
        }
    }