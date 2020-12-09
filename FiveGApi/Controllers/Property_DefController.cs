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
    public class Property_DefController : ApiController
    {
        private MIS_DBEntities db = new MIS_DBEntities();

        // GET: api/Property_def
        [ResponseType(typeof(IQueryable<Property_def>))]
        public IQueryable<Property_def> AllProperty_def(int RID)
        {
            return db.Property_def;
        }

        // GET: api/Property_def/5
        [ResponseType(typeof(Property_def))]
        public IHttpActionResult GetProperty_def(int id)
        {
            Property_def Property_def = db.Property_def.Find(id);

            if (Property_def == null)
            {
                return NotFound();
            }

            return Ok(Property_def);
        }

        // PUT: api/Property_def/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProperty_def(int id, Property_def Property_def)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existProperty_def = db.Property_def.Where(x => x.ID == id).FirstOrDefault();
            ////existProperty_def.Property_defDetails
            //if (existProperty_def.Property_defDetails.Count > 0)
            //{
            //    foreach (var item in existProperty_def.Property_defDetails.ToList())
            //    {
            //        existProperty_def.Property_defDetails.Remove(item);
            //    }
            //}

            //existProperty_def.Property_defDetails = Property_def.Property_defDetails;



            // db.Entry(Property_def).State = EntityState.Modified;
            existProperty_def.Updated_On = DateTime.Now.ToString();
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Property_defExists((int)Property_def.ID))
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

        // POST: api/Property_def
        [ResponseType(typeof(Property_def))]
        public IHttpActionResult PostProperty_def(Property_def Property_def)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Property_def.Add(Property_def);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Property_def.ID }, Property_def);
        }

        // DELETE: api/Property_def/5
        [ResponseType(typeof(Property_def))]
        public IHttpActionResult DeleteProperty_def(int id)
        {
            Property_def Property_def = db.Property_def.Find(id);
            if (Property_def == null)
            {
                return NotFound();
            }

            db.Property_def.Remove(Property_def);
            db.SaveChanges();

            return Ok(Property_def);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Property_defExists(int id)
        {
            return db.Property_def.Count(e => e.ID == id) > 0;
        }
    }
}
