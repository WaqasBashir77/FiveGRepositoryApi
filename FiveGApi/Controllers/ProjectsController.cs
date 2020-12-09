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
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private MIS_DBEntities db = new MIS_DBEntities();

        // GET: api/Projects
        public IQueryable<Project> GetProjects()
        {

            return db.Projects;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existProject = db.Projects.Where(x => x.Id == id).FirstOrDefault();
            existProject.address = project.address;
            existProject.city = project.city;
            existProject.description = project.description;
            existProject.location = project.location;
            existProject.noc = project.noc;
            existProject.projectCurrency = project.projectCurrency;
            existProject.projectName = project.projectName;
            existProject.projectType = project.projectType;
            existProject.status = project.status;
            existProject.totalArea = project.totalArea;
            existProject.unit = project.unit;

            if (existProject.ProjectDetails.Count > 0)
            {
                foreach (var item in existProject.ProjectDetails.ToList())
                {
                    existProject.ProjectDetails.Remove(item);
                }
            }

            existProject.ProjectDetails = project.ProjectDetails;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.Id == id) > 0;
        }

        [HttpGet]
        [Route("checkDuplicateCode")]
        public IHttpActionResult CheckDuplicateProjectCode(string code)
        {
            bool isExist = false;
            if (!string.IsNullOrWhiteSpace(code))
            {
                var exist = db.Projects.Where(x => x.projectCode == code).FirstOrDefault();
                if (exist != null)
                {
                    isExist = true;
                }
            }



            return Ok(isExist);
        }
    }
}