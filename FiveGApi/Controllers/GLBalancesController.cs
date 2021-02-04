
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

    [RoutePrefix("api/GLBalances")]
    public class GLBalancesController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        // GET: api/GL_Balances
        [ResponseType(typeof(IQueryable<GL_Balances>))]
        [Route("GetALLGL_Balances")]
        [HttpGet]
        public IQueryable<GL_Balances> GetALLGL_Balances()
        {
            return db.GL_Balances;
        }

        // GET: api/GL_Balances/5
        [ResponseType(typeof(GL_Balances))]
        public IHttpActionResult GetGL_BalancesByID(int id)
        {
            GL_Balances GL_Balances = db.GL_Balances.Find(id);

            if (GL_Balances == null)
            {
                return NotFound();
            }

            return Ok(GL_Balances);
        }

        // PUT: api/GL_Balances/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGL_Balances(int id, GL_Balances GL_Balances)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existGL_Balances = db.GL_Balances.Where(x => x.ID == id).FirstOrDefault();
            existGL_Balances.C_CODE = GL_Balances.C_CODE;
            existGL_Balances.Bal_Date = GL_Balances.Bal_Date;
            existGL_Balances.Amount = GL_Balances.Amount;
            existGL_Balances.Description = GL_Balances.Description;
            existGL_Balances.Effect_Trans_ID = GL_Balances.Effect_Trans_ID;
            existGL_Balances.Updated_By = "Admin";
            existGL_Balances.Updated_On = DateTime.Now;
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GL_BalancesExists((int)GL_Balances.ID))
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


        // POST: api/GL_Balances
        [ResponseType(typeof(GL_Balances))]
        public IHttpActionResult PostGL_Balances(GL_Balances GL_Balances)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            GL_Balances.Created_By = "Admin";
            GL_Balances.Created_On = DateTime.Now;
            db.GL_Balances.Add(GL_Balances);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = GL_Balances.ID }, GL_Balances);
        }

        // DELETE: api/GL_Balances/5
        [ResponseType(typeof(GL_Balances))]
        public IHttpActionResult DeleteGL_Balances(int id)
        {
            GL_Balances GL_Balances = db.GL_Balances.Find(id);
            if (GL_Balances == null)
            {
                return NotFound();
            }

            db.GL_Balances.Remove(GL_Balances);
            db.SaveChanges();

            return Ok(GL_Balances);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GL_BalancesExists(int id)
        {
            return db.GL_Balances.Count(e => e.ID == id) > 0;
        }
    }
}