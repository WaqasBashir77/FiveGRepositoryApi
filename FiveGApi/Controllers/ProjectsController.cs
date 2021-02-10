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
using FiveGApi.DTOModels;
using FiveGApi.Helper;
using FiveGApi.Models;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        //private FiveG_DBEntities db = new FiveG_DBEntities();
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        // GET: api/Projects
        public IQueryable<Project> GetProjects()
        {
            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            IQueryable<Project> projects ;

            try
            {
                if (!SecurityGroupDTO.CheckSuperAdmin(groupId))
                    projects = db.Projects.Where(x => x.SecurityGroupId == groupId);
                else
                    projects = db.Projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return projects;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(ProjectDto))]
        public IHttpActionResult GetProject(int id)
        {

            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            ProjectDto projectDto = new ProjectDto();
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }
            bool isAdmin = SecurityGroupDTO.CheckSuperAdmin(groupId);
            if (project.SecurityGroupId != groupId && !isAdmin)
            {
                return Ok(projectDto);
            }
            else
            {
                
                projectDto.address = project.address;
                projectDto.city = project.city;
                projectDto.description = project.description;
                projectDto.Id = project.Id;
                projectDto.location = project.location;
                projectDto.noc = project.noc;
                projectDto.PaymentPlanStatus = project.PaymentPlanStatus;
                projectDto.projectCode = project.projectCode;
                projectDto.projectName = project.projectName;
                projectDto.projectType = project.projectType;
                projectDto.status = project.status;
                projectDto.totalArea = project.totalArea;
                projectDto.unit = project.unit;

                foreach (var item in project.ProjectDetails)
                {
                    ProjectDetailDto projectDetail = new ProjectDetailDto();
                    projectDetail.building = item.building;
                    projectDetail.childArea = item.childArea;
                    projectDetail.childDescription = item.childDescription;
                    projectDetail.childStatus = item.childStatus;
                    projectDetail.featurePrice = item.featurePrice;
                    projectDetail.floor = item.floor;
                    projectDetail.floorName = db.Lookup_Values.Where(x => x.Ref_ID == 6 && x.Value_Status == true && x.Value_ID == item.floor).Select(x => x.Value_Description).FirstOrDefault();
                    projectDetail.buildingName = db.Lookup_Values.Where(x => x.Ref_ID == 5 && x.Value_Status == true && x.Value_ID == item.building).Select(x => x.Value_Description).FirstOrDefault();
                    projectDetail.building = item.building;
                    projectDetail.Id = item.Id;
                    projectDetail.otherFeatures = item.otherFeatures;
                    projectDetail.unitNumber = item.unitNumber;
                    projectDetail.unitPrice = item.unitPrice;
                    projectDetail.unitType = item.unitType;
                    projectDetail.SqFrPrice = item.SqFrPrice;
                    projectDetail.noOfBedrooms = item.NoOfBedRooms;

                    projectDto.ProjectDetails.Add(projectDetail);
                }
            }
            return Ok(projectDto);
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
            existProject.Update_Date = DateTime.Now;
            existProject.Update_By = project.Update_By;

            
            if (existProject.ProjectDetails.Count > 0)
            {
                foreach (var item in existProject.ProjectDetails.ToList())
                {
                    existProject.ProjectDetails.Remove(item);
                }
            }

            //foreach (var item in project.ProjectDetails.ToList())
            //{
            //    item.Update_By = project.Update_By;
            //    item.Update_Date = DateTime.Now;
            //}
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                project.PaymentPlanStatus = false;
                db.Projects.Add(project);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

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

        [HttpGet]
        [Route("getallbuildings")]
        public IHttpActionResult GetAllBuildings()
        {
            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();
            try
            {
                //Order by Value_Orderno ASC
                lookup_Values = db.Lookup_Values.Where(x => x.Ref_ID == 5 && x.Value_Status == true).OrderBy(x => x.Value_orderNo).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok(lookup_Values);
        }

        [HttpGet]
        [Route("getallfloor")]
        public IHttpActionResult GetAllFloor()
        {
            List<Lookup_Values> lookup_Values = new List<Lookup_Values>();
            try
            {
                //Order by Value_Orderno ASC
                lookup_Values = db.Lookup_Values.Where(x => x.Ref_ID == 6 && x.Value_Status == true).OrderBy(x => x.Value_orderNo).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok(lookup_Values);
        }
    }
}