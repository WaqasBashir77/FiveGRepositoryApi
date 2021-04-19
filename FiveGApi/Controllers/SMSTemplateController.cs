using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using FiveGApi.Models;

namespace FiveGApi.Controllers
{
    [Authorize]

    public class SMSTemplateController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        private string UserId;
        public SMSTemplateController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
        // GET: api/SMS_Template
        public IQueryable<SMS_Template> GetSMS_TemplateAll()
        {
            return db.SMS_Template.OrderByDescending(x => x.Created_On).AsQueryable();
        }

        // GET: api/SMS_Template/5
        [ResponseType(typeof(SMS_Template))]
        public IHttpActionResult GetSMS_Template(int id)
        {
            List<SMS_Template> SMS_Template = db.SMS_Template.Where(x => x.ID == id ).OrderByDescending(x => x.Created_On).AsQueryable().ToList();
            if (SMS_Template == null)
            {
                return NotFound();
            }

            return Ok(SMS_Template);
        }

        // PUT: api/SMS_Template/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSMS_Template(int id, SMS_Template SMS_Template)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SMS_Template.ID)
            {
                return BadRequest();
            }

            db.Entry(SMS_Template).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SMS_TemplateExists(id))
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

        // POST: api/SMS_Template
        [ResponseType(typeof(SMS_Template))]
        public IHttpActionResult PostSMS_Template(SMS_Template SMS_Template)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SMS_Template.Add(SMS_Template);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = SMS_Template.ID }, SMS_Template);
        }

        // DELETE: api/SMS_Template/5
        [ResponseType(typeof(SMS_Template))]
        public IHttpActionResult DeleteSMS_Template(int id)
        {
            SMS_Template SMS_Template = db.SMS_Template.Find(id);
            if (SMS_Template == null)
            {
                return NotFound();
            }

            db.SMS_Template.Remove(SMS_Template);
            db.SaveChanges();

            return Ok(SMS_Template);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SMS_TemplateExists(int id)
        {
            return db.SMS_Template.Count(e => e.ID == id) > 0;
        }
    }
}
