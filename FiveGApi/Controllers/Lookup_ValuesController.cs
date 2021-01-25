using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FiveGApi.Models;

namespace FiveGApi.Controllers
{
    [Authorize]

    public class Lookup_ValuesController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/Lookup_Values
        public IQueryable<Lookup_Values> GetLookup_ValuesAll()
        {
            return db.Lookup_Values;
        }

        // GET: api/Lookup_Values/5
        [ResponseType(typeof(Lookup_Values))]
        public IHttpActionResult GetLookup_Values(int id)
        {
            List<Lookup_Values> lookup_Values = db.Lookup_Values.Where(x => x.Ref_ID == id && x.Value_Status == true).ToList();
            if (lookup_Values == null)
            {
                return NotFound();
            }

            return Ok(lookup_Values);
        }

        // PUT: api/Lookup_Values/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLookup_Values(int id, Lookup_Values lookup_Values)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lookup_Values.Value_ID)
            {
                return BadRequest();
            }

            db.Entry(lookup_Values).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Lookup_ValuesExists(id))
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

        // POST: api/Lookup_Values
        [ResponseType(typeof(Lookup_Values))]
        public IHttpActionResult PostLookup_Values(Lookup_Values lookup_Values)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lookup_Values.Add(lookup_Values);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lookup_Values.Value_ID }, lookup_Values);
        }

        // DELETE: api/Lookup_Values/5
        [ResponseType(typeof(Lookup_Values))]
        public IHttpActionResult DeleteLookup_Values(int id)
        {
            Lookup_Values lookup_Values = db.Lookup_Values.Find(id);
            if (lookup_Values == null)
            {
                return NotFound();
            }

            db.Lookup_Values.Remove(lookup_Values);
            db.SaveChanges();

            return Ok(lookup_Values);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Lookup_ValuesExists(int id)
        {
            return db.Lookup_Values.Count(e => e.Value_ID == id) > 0;
        }
    }
}