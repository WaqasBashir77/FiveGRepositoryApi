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
    public class BookingPropertyDefController : ApiController
    {
        private FiveG_DBEntities db = new FiveG_DBEntities();

        // GET: api/PropertyDef
        // [ResponseType(typeof(IQueryable<PropertyDef>))]
        public IQueryable<PropertyDef> GetPropertyDefALL()
        {
            return db.PropertyDefs;
        }

        // GET: api/PropertyDef/5
        [ResponseType(typeof(PropertyDef))]
        public IHttpActionResult GetPropertyDef(int id)
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
            ////existPropertyDef.PropertyDefDetails
            //if (existPropertyDef.PropertyDefDetails.Count > 0)
            //{
            //    foreach (var item in existPropertyDef.PropertyDefDetails.ToList())
            //    {
            //        existPropertyDef.PropertyDefDetails.Remove(item);
            //    }
            //}

            //existPropertyDef.PropertyDefDetails = PropertyDef.PropertyDefDetails;



            // db.Entry(PropertyDef).State = EntityState.Modified;
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PropertyDef
        [ResponseType(typeof(PropertyDef))]
        public IHttpActionResult PostPropertyDef(PropertyDef PropertyDef)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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