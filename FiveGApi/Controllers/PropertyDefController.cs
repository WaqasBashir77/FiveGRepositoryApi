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

    [RoutePrefix("api/PropertyDef")]

    public class PropertyDefController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/PropertyDef
        [ResponseType(typeof(IQueryable<PropertyDef>))]
        [Route("GetALLPropertyDef")]
        [HttpGet]
        public IQueryable<PropertyDef> GetALLPropertyDef()
        {
            return db.PropertyDefs;
        }

        // GET: api/PropertyDef/5
        [ResponseType(typeof(PropertyDef))]
        public IHttpActionResult GetPropertyDefByID(int id)
        {
            PropertyDef PropertyDef = db.PropertyDefs.Find(id);

            if (PropertyDef == null)
            {
                return NotFound();
            }

            return Ok(PropertyDef);
        }

        // PUT: api/PropertyDef/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPropertyDef(int id, PropertyDef PropertyDef)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                
            var existPropertyDef = db.PropertyDefs.Where(x => x.ID == id).FirstOrDefault();          
            existPropertyDef.Name       = PropertyDef.Name;
            existPropertyDef.Society = PropertyDef.Society;
            existPropertyDef.Category = PropertyDef.Category;
            existPropertyDef.Plot_Size = PropertyDef.Plot_Size;
            existPropertyDef.Dimensions = PropertyDef.Dimensions;
            existPropertyDef.Price = PropertyDef.Price;
            existPropertyDef.Block = PropertyDef.Block;
            existPropertyDef.Type = PropertyDef.Type;
            existPropertyDef.Status = PropertyDef.Status;
            existPropertyDef.Booking_percent = PropertyDef.Booking_percent;
            existPropertyDef.Confirm_percent = PropertyDef.Confirm_percent;
            existPropertyDef.Rebate_percent = PropertyDef.Rebate_percent;
            existPropertyDef.Tax_percent = PropertyDef.Tax_percent;
            existPropertyDef.Total_Files = PropertyDef.Total_Files;
            existPropertyDef.Booking_Start = PropertyDef.Booking_Start;
            existPropertyDef.Booking_End = PropertyDef.Booking_End;
            existPropertyDef.GL_Mapping_ID = PropertyDef.GL_Mapping_ID;
            existPropertyDef.Flex_1 = "1";
            existPropertyDef.Flex_2 ="1";           
            existPropertyDef.Updated_By = PropertyDef.Updated_By;
            existPropertyDef.Updated_On = DateTime.Now.ToString();
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyDefExists((int)PropertyDef.ID))
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

        // POST: api/PropertyDef
        [ResponseType(typeof(PropertyDef))]
        public IHttpActionResult PostPropertyDef(PropertyDef PropertyDef)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PropertyDef.Flex_1 = "1";
            PropertyDef.Flex_2 = "1";
            PropertyDef.Created_By = "1";
            PropertyDef.Created_ON = DateTime.Now;
            db.PropertyDefs.Add(PropertyDef);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = PropertyDef.ID }, PropertyDef);
        }

        // DELETE: api/PropertyDef/5
        [ResponseType(typeof(PropertyDef))]
        public IHttpActionResult DeletePropertyDef(int id)
        {
            PropertyDef PropertyDef = db.PropertyDefs.Find(id);
            if (PropertyDef == null)
            {
                return NotFound();
            }

            db.PropertyDefs.Remove(PropertyDef);
            db.SaveChanges();

            return Ok(PropertyDef);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyDefExists(int id)
        {
            return db.PropertyDefs.Count(e => e.ID == id) > 0;
        }
    }
}