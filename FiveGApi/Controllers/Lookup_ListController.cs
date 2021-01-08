using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    public class Lookup_ListController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        // GET: api/Lookup_List
        public IQueryable<Lookup_List> GetLookup_ListALL()
        {
            return db.Lookup_List;
        }

        // GET: api/Lookup_List/5
        [ResponseType(typeof(Lookup_List))]
        public IHttpActionResult GetLookup_List(int id)
        {
            Lookup_List lookup_List = db.Lookup_List.Find(id);
            if (lookup_List == null)
            {
                return NotFound();
            }

            return Ok(lookup_List);
        }

        // PUT: api/Lookup_List/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLookup_List(int id, Lookup_List lookup_List)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lookup_List.REF_ID)
            {
                return BadRequest();
            }

            db.Entry(lookup_List).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Lookup_ListExists(id))
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

        // POST: api/Lookup_List
        [ResponseType(typeof(Lookup_List))]
        public IHttpActionResult PostLookup_List(Lookup_List lookup_List)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lookup_List.Add(lookup_List);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lookup_List.REF_ID }, lookup_List);
        }

        // DELETE: api/Lookup_List/5
        [ResponseType(typeof(Lookup_List))]
        public IHttpActionResult DeleteLookup_List(int id)
        {
            Lookup_List lookup_List = db.Lookup_List.Find(id);
            if (lookup_List == null)
            {
                return NotFound();
            }

            db.Lookup_List.Remove(lookup_List);
            db.SaveChanges();

            return Ok(lookup_List);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Lookup_ListExists(int id)
        {
            return db.Lookup_List.Count(e => e.REF_ID == id) > 0;
        }
    }
}