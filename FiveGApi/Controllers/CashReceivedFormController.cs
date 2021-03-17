using FiveGApi.Helper;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/CashReceivedForm")]
    public class CashReceivedFormController : ApiController
    {
        private string UserId;
        public CashReceivedFormController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        // GET: api/CashreceivedForm
        public IQueryable<CashreceivedForm> GetCashreceivedFormALL()
        {
            if (UserId != null)
            {
                var userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().Select(x => x.SecurityGroupId).FirstOrDefault();
                if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup))
                {
                    return db.CashreceivedForms.Where(x=>x.Security_Group_ID==userSecurityGroup.ToString()).AsQueryable();
                }
                else
                {
                    return db.CashreceivedForms;

                }


            }
            else
            {
                return null;
            }
                
        }

        // GET: api/CashreceivedForm/5
        [ResponseType(typeof(CashreceivedForm))]
        public IHttpActionResult GetCashreceivedForm(int id)
        {
            if (UserId != null)
            {
                CashreceivedForm CashreceivedForm = new CashreceivedForm();
                var userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().Select(x => x.SecurityGroupId).FirstOrDefault();
                if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup))
                {
                    CashreceivedForm = db.CashreceivedForms.Where(x => x.Security_Group_ID == userSecurityGroup.ToString() && x.ID==id).AsQueryable().FirstOrDefault();

                   
                }
                else
                {
                    CashreceivedForm=db.CashreceivedForms.Find(id);

                }

                if(CashreceivedForm==null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(CashreceivedForm);
                }
            }
            else
            {
                return Unauthorized();
            }

            
        }

        // PUT: api/CashreceivedForm/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCashreceivedForm(int id, CashreceivedForm CashreceivedForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != CashreceivedForm.ID)
            {
                return BadRequest();
            }

            db.Entry(CashreceivedForm).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CashreceivedFormExists(id))
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

        // POST: api/CashreceivedForm
        [ResponseType(typeof(CashreceivedForm))]
        public IHttpActionResult PostCashreceivedForm(CashreceivedForm CashreceivedForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (UserId != null)
            {
                var userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().FirstOrDefault();

                CashreceivedForm.Security_Group_ID = userSecurityGroup.SecurityGroupId.ToString();
                CashreceivedForm.Created_By = userSecurityGroup.UserId.ToString();
                CashreceivedForm.Receipt_Number = "5G-"+DateTime.Now.ToFileTimeUtc()+"-"+DateTime.Now.Year;
                db.CashreceivedForms.Add(CashreceivedForm);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = CashreceivedForm.ID }, CashreceivedForm);
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE: api/CashreceivedForm/5
        [ResponseType(typeof(CashreceivedForm))]
        public IHttpActionResult DeleteCashreceivedForm(int id)
        {
            CashreceivedForm CashreceivedForm = db.CashreceivedForms.Find(id);
            if (CashreceivedForm == null)
            {
                return NotFound();
            }

            db.CashreceivedForms.Remove(CashreceivedForm);
            db.SaveChanges();

            return Ok(CashreceivedForm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CashreceivedFormExists(int id)
        {
            return db.CashreceivedForms.Count(e => e.ID == id) > 0;
        }
    }
}