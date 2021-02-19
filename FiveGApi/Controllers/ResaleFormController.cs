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
using FiveGApi.DTOModels;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/ResaleForm")]
    public class ResaleFormController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/Resale_Form

        [ResponseType(typeof(IQueryable<Resale_Form>))]
        public IQueryable<Resale_Form> GetAllResale_Form(int RDID)
        {
            return db.Resale_Form;
        }
        [Route("GetResale_FormByID")]
        // GET: api/Resale_Form/5
        [ResponseType(typeof(Resale_Form))]
        public IHttpActionResult GetResale_FormByID(int id)
        {
            Resale_Form Resale_Form = db.Resale_Form.Find(id);
            if (Resale_Form == null)
            {
                return NotFound();
            }

            return Ok(Resale_Form);
        }

        // PUT: api/Resale_Form/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutResale_Form(int id, Resale_Form Resale_Form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != Resale_Form.ID)
            {
                return BadRequest();
            }

            db.Entry(Resale_Form).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Resale_FormExists(id))
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

        // POST: api/Resale_Form
        [ResponseType(typeof(Resale_Form))]
        public IHttpActionResult PostResale_Form(Resale_Form Resale_Form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Resale_Form.Created_By = "Admin";
            Resale_Form.Created_On = DateTime.Now;
            db.Resale_Form.Add(Resale_Form);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Resale_Form.ID }, Resale_Form);
        }
        
        // DELETE: api/Resale_Form/5
        [ResponseType(typeof(Resale_Form))]
        public IHttpActionResult DeleteResale_Form(int id)
        {
            Resale_Form Resale_Form = db.Resale_Form.Find(id);
            if (Resale_Form == null)
            {
                return NotFound();
            }

            db.Resale_Form.Remove(Resale_Form);
            db.SaveChanges();

            return Ok(Resale_Form);
        }
        [Route("ResaleByStaffNameOrPartyCode")]
        [HttpGet]
        [ResponseType(typeof(FiveGApi.Models.Resale_Form))]
        public IHttpActionResult ResaleByStaffNameOrPartyCode(string staffName, string partyCode)
        {
            var result = db.Resale_Form.Where(x => x.StaffName.Contains(staffName) || x.EmployeeCode.Contains(partyCode)).ToList();
            if (result == null)
            {

                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Resale_FormExists(int id)
        {
            return db.Resale_Form.Count(e => e.ID == id) > 0;
        }
    }
}

