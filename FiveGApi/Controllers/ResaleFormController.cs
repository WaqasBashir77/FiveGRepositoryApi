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
using System.Security.Claims;
using FiveGApi.Helper;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/ResaleForm")]
    public class ResaleFormController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        private string UserId;
        private User userSecurityGroup = new User();

        public ResaleFormController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;
            userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().FirstOrDefault();

        }
        // GET: api/Resale_Form

        [ResponseType(typeof(IQueryable<Resale_Form>))]
        public IQueryable<Resale_Form> GetAllResale_Form(int RDID)
        {
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                return db.Resale_Form.Where(x => x.SecurityGroupId == userSecurityGroup.SecurityGroupId).OrderByDescending(x => x.Created_On).AsQueryable();
            else
                return db.Resale_Form.OrderByDescending(x => x.Created_On).AsQueryable();
        }
        [Route("GetResale_FormByID")]
        // GET: api/Resale_Form/5
        [ResponseType(typeof(Resale_Form))]
        public IHttpActionResult GetResale_FormByID(int id)
        {
            Resale_Form Resale_Form = new Resale_Form();
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                Resale_Form= db.Resale_Form.Where(x =>x.ID==id&& x.SecurityGroupId == userSecurityGroup.SecurityGroupId).FirstOrDefault();
            else
                Resale_Form= db.Resale_Form.Find(id);
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
            Resale_Form.SecurityGroupId = userSecurityGroup.SecurityGroupId;
            Resale_Form.Updated_By = userSecurityGroup.UserName;
            Resale_Form.Updated_On = DateTime.Now;
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
            Resale_Form.SecurityGroupId = userSecurityGroup.SecurityGroupId;
            Resale_Form.Created_By = userSecurityGroup.UserName;
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
            var result = new List<Resale_Form>();
           
                result= db.Resale_Form.Where(x => x.StaffName.Contains(staffName) || x.EmployeeCode.Contains(partyCode)).ToList();
            if (result == null)
            {

                return NotFound();
            }
            else
            {
                if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                    return Ok(result.Where(x => x.SecurityGroupId == userSecurityGroup.SecurityGroupId));
                else
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

