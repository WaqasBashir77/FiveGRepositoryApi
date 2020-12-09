using System;
using System.Collections.Generic;
using System.Linq;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;

namespace FiveGApi.Controllers
{
    public class RebateDetailsController : ApiController
    {
        private MIS_DBEntities db = new MIS_DBEntities();

        // GET: api/Rebate_Details

        [ResponseType(typeof(IQueryable<Rebate_Details>))]
        public IQueryable<Rebate_Details> AllRebate_Details(int RDID)
        {
            return db.Rebate_Details;
        }

        // GET: api/Rebate_Details/5
        [ResponseType(typeof(Rebate_Details))]
        public IHttpActionResult GetRebate_Detail(int id)
        {
            Rebate_Details Rebate_Detail = db.Rebate_Details.Find(id);
            if (Rebate_Detail == null)
            {
                return NotFound();
            }

            return Ok(Rebate_Detail);
        }

        // PUT: api/Rebate_Details/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRebate_Detail(int id, Rebate_Details Rebate_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Rebate_Detail.R_ID)
            {
                return BadRequest();
            }

            db.Entry(Rebate_Detail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Rebate_DetailExists(id))
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

        // POST: api/Rebate_Details
        [ResponseType(typeof(Rebate_Details))]
        public IHttpActionResult PostRebate_Detail(Rebate_Details Rebate_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rebate_Details.Add(Rebate_Detail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Rebate_Detail.R_ID }, Rebate_Detail);
        }

        // DELETE: api/Rebate_Details/5
        [ResponseType(typeof(Rebate_Details))]
        public IHttpActionResult DeleteRebate_Detail(int id)
        {
            Rebate_Details Rebate_Detail = db.Rebate_Details.Find(id);
            if (Rebate_Detail == null)
            {
                return NotFound();
            }

            db.Rebate_Details.Remove(Rebate_Detail);
            db.SaveChanges();

            return Ok(Rebate_Detail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Rebate_DetailExists(int id)
        {
            return db.Rebate_Details.Count(e => e.R_ID == id) > 0;
        }
    }
}
