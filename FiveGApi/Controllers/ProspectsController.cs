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
using FiveGApi.Helper;
using FiveGApi.Models;

namespace FiveGApi.Controllers
{

    [Authorize]
    public class ProspectsController : ApiController
    {
        // private FiveG_DBEntities db = new FiveG_DBEntities();
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        // GET: api/Prospects
        private string UserId;
        public ProspectsController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
        public IQueryable<Prospect> GetProspects()
        {
            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            IQueryable<Prospect> prospects;
            try
            {
                if (!SecurityGroupDTO.CheckSuperAdmin(groupId))
                    prospects = db.Prospects.Where(x => x.SecurityGroupId == groupId).OrderByDescending(x => x.Created_Date).AsQueryable();
                else
                    prospects = db.Prospects.OrderByDescending(x => x.Created_Date).AsQueryable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return prospects;
        }


        // GET: api/Prospects/5
        [ResponseType(typeof(Prospect))]
        public IHttpActionResult GetProspect(int id)
        {
            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            Prospect Reponseprospect = new Prospect();
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return NotFound();
            }
            bool isAdmin = SecurityGroupDTO.CheckSuperAdmin(groupId);
            if (prospect.SecurityGroupId != groupId && !isAdmin)
            {
                return Ok(Reponseprospect);
            }
            else
            {
                Reponseprospect = prospect;
            }
            return Ok(Reponseprospect);
        }

        // PUT: api/Prospects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProspect(int id, Prospect prospect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            prospect.Id = id;
            Prospect Updateprospect = db.Prospects.Find(id);
            Updateprospect.address = prospect.address;
            Updateprospect.city = prospect.city;
            Updateprospect.cnic = prospect.cnic;
            Updateprospect.employeeId = prospect.employeeId;
            Updateprospect.knowUs = prospect.knowUs;
            Updateprospect.mobile = prospect.mobile;
            Updateprospect.name = prospect.name;
            Updateprospect.profession = prospect.profession;
            Updateprospect.projectId = prospect.projectId;
            Updateprospect.prospectType = prospect.prospectType;
            Updateprospect.Update_By = prospect.Update_By;
            Updateprospect.gender = prospect.gender;
            Updateprospect.Update_Date = DateTime.Now;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProspectExists(id))
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

        // POST: api/Prospects
        [ResponseType(typeof(Prospect))]
        public IHttpActionResult PostProspect(Prospect prospect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            prospect.Created_Date = DateTime.Now;
            db.Prospects.Add(prospect);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = prospect.Id }, prospect);
        }

        // DELETE: api/Prospects/5
        [ResponseType(typeof(Prospect))]
        public IHttpActionResult DeleteProspect(int id)
        {
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return NotFound();
            }

            db.Prospects.Remove(prospect);
            db.SaveChanges();

            return Ok(prospect);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProspectExists(int id)
        {
            return db.Prospects.Count(e => e.Id == id) > 0;
        }
    }
}