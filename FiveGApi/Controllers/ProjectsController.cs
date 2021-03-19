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
using FiveGApi.DTOModels;
using FiveGApi.Helper;
using FiveGApi.Models;

namespace FiveGApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private string UserId;
        public ProjectsController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
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
            IQueryable<Project> projects;

            try
            {
                if (!SecurityGroupDTO.CheckSuperAdmin(groupId))
                    projects = db.Projects.Where(x => x.SecurityGroupId == groupId).AsQueryable();
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
        [HttpGet]
        [Route("GetProjectsListDTO")]
        public IHttpActionResult GetProjectsListDTO()
        {
            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            IQueryable<ProjectListDTO> projects;

            try
            {
                if (!SecurityGroupDTO.CheckSuperAdmin(groupId))
                    projects = db.Projects.Where(x => x.SecurityGroupId == groupId).Select(x=> new ProjectListDTO
                    {
                        Id=x.Id,
                        projectCode = x.projectCode,
                        location=x.location,
                        projectName=x.projectName,
                        projectType = x.projectType,
                        totalArea = x.totalArea,
                        address = x.address,
                        city = x.city,
                        status = x.status,
                        description = x.description,
                        unit = x.unit,
                        noc = x.noc,
                        projectCurrency = x.projectCurrency,
                        PaymentPlanStatus = x.PaymentPlanStatus,
                        SecurityGroupId = x.SecurityGroupId,
                        Created_By = x.Created_By,
                        Created_Date = x.Created_Date,
                        Update_By = x.Update_By,
                        Update_Date = x.Update_Date,
                        LocationSeg = x.location,
                        Company = x.Company,
                        ProjectSeg = x.ProjectSeg,
                        PlotAres = x.PlotAres
                       
                    }).OrderByDescending(x=>x.Created_By).AsQueryable();
                else
                    projects = db.Projects.Select(x => new ProjectListDTO
                    {
                        Id = x.Id,
                        projectCode = x.projectCode,
                        location = x.location,
                        projectName = x.projectName,
                        projectType = x.projectType,
                        totalArea = x.totalArea,
                        address = x.address,
                        city = x.city,
                        status = x.status,
                        description = x.description,
                        unit = x.unit,
                        noc = x.noc,
                        projectCurrency = x.projectCurrency,
                        PaymentPlanStatus = x.PaymentPlanStatus,
                        SecurityGroupId = x.SecurityGroupId,
                        Created_By = x.Created_By,
                        Created_Date = x.Created_Date,
                        Update_By = x.Update_By,
                        Update_Date = x.Update_Date,
                        LocationSeg = x.location,
                        Company = x.Company,
                        ProjectSeg = x.ProjectSeg,
                        PlotAres = x.PlotAres

                    }).OrderByDescending(x => x.Created_By).AsQueryable();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw;
            }
            return Ok(projects);
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
                projectDto.PlotAres= project.PlotAres;
                projectDto.Company= project.Company;
                projectDto.LocationSeg= project.LocationSeg;
                projectDto.ProjectSeg= project.ProjectSeg;
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
            existProject.PlotAres = project.PlotAres;

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
                project.Created_By = 1;
                project.Created_Date = DateTime.Now;
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
        //[ResponseType(typeof(Project))]
        [HttpPost]
        [Route("AuthorizePropertySalesSale")]
        public IHttpActionResult AuthorizePropertySales(int id)
        {
            var propertySales = db.PropertySales.Where(x=>x.Booking_ID==id).FirstOrDefault();
            if (propertySales == null && propertySales.AuthorizeStatus==true)
            {
                return NotFound();
            }
            else
            {
                propertySales.AuthorizeStatus = true;
                db.SaveChanges();
                var projectDetail = db.ProjectDetails.Where(x => x.projectId == propertySales.Project_ID).FirstOrDefault();
                if (projectDetail != null)
                {
                    var EmployeeCommision = ((projectDetail.unitPrice / 100) * propertySales.Employee_Com);
                    var DealerCommision = ((projectDetail.unitPrice / 100) * propertySales.Dealer_Comm);
                    var pro = db.Projects.Where(x => x.Id == propertySales.Project_ID).FirstOrDefault();
                    Project_Entries booking_EntriesforROS = new Project_Entries();
                    booking_EntriesforROS.Transaction_ID = id;
                    booking_EntriesforROS.Entry_Date = DateTime.Now;
                    booking_EntriesforROS.Entry_Type = "Rebate";
                    booking_EntriesforROS.Created_By = "Admin";
                    booking_EntriesforROS.Created_On = DateTime.Now;
                    booking_EntriesforROS.Status = "Transfered";
                    var c_Code = pro.Company + "." + pro.ProjectSeg + "." + pro.LocationSeg;
                    #region Project Sale Price / Dealer Commision / Employee Commision
                    ///-------------------Receivable from members------------------------------///////////
                    var coa_Segment = db.COA_Segments.Where(x => x.Name == "Receivable from members").FirstOrDefault();
                    booking_EntriesforROS.C_CODE = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + ".0000").ToString();
                    booking_EntriesforROS.Debit = (decimal)projectDetail.unitPrice;
                    booking_EntriesforROS.Credit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                    ///-------------------To Unearned revenue------------------------------///////////            
                    coa_Segment = db.COA_Segments.Where(x => x.Name == "Unearned revenue").FirstOrDefault();
                    booking_EntriesforROS.C_CODE = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + ".0000").ToString();
                    booking_EntriesforROS.Credit = (decimal)projectDetail.unitPrice;
                    booking_EntriesforROS.Debit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                    ///-------------------Commission to staff------------------------------///////////
                    coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission to staff").FirstOrDefault();
                    booking_EntriesforROS.C_CODE = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + ".0000").ToString();
                    booking_EntriesforROS.Debit = (decimal)EmployeeCommision;
                    booking_EntriesforROS.Credit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                    ///-------------------To Commission payable to Staff------------------------------///////////            
                    var partCode = db.Registrations.Where(x => x.ID == propertySales.Employee).FirstOrDefault();
                    coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission payable to Staff").FirstOrDefault();
                    booking_EntriesforROS.C_CODE = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + "."+ partCode.Code).ToString();
                    booking_EntriesforROS.Credit = (decimal)EmployeeCommision;
                    booking_EntriesforROS.Debit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                    ////-------------------Commission to agent------------------------------///////////
                    coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission to agent").FirstOrDefault();
                    booking_EntriesforROS.C_CODE = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + ".000").ToString();
                    booking_EntriesforROS.Debit = (decimal)DealerCommision;
                    booking_EntriesforROS.Credit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                    ////-------------------To Commission payable to Agent------------------------------///////////
                    partCode = db.Registrations.Where(x => x.ID == propertySales.Dealer_ID).FirstOrDefault();
                    coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission payable to Agent").FirstOrDefault();
                    booking_EntriesforROS.C_CODE = GenerateCOACombinations(c_Code + "." + coa_Segment.Segment_Value + "."+partCode.Code).ToString();
                    booking_EntriesforROS.Credit = (decimal)DealerCommision;
                    booking_EntriesforROS.Debit = 0;
                    db.Project_Entries.Add(booking_EntriesforROS);
                    db.SaveChanges();
                    #endregion Project Sale Price
                    #region Gl Header and GL Lines
                    var glheader = db.GL_Headers.Where(x => x.Source_Tran_Id == propertySales.Booking_ID && x.Source == "Projects").FirstOrDefault();
                    //var bookingConfirm = db.BookingConfirms.Where(x => x.ID == existBooking_Payments.ID).FirstOrDefault();
                    // var propertydef = db.PropertyDefs.Where(x => x.ID == bookingConfirm.Property_ID).FirstOrDefault();
                    GL_Headers gL_Headers = new GL_Headers();

                    if (glheader == null)
                    {
                        gL_Headers.J_Date = DateTime.Now;
                        gL_Headers.Doc_Date = DateTime.Now;
                        gL_Headers.Currency = "PKR";
                        gL_Headers.Description = pro.projectCode + "-" + projectDetail.unitNumber + "-" + projectDetail.unitType + "-" + propertySales.Buyer_Name;
                        gL_Headers.Remarks = "";
                        gL_Headers.Source = "Projects";
                        gL_Headers.Trans_Status = "UnPosted";
                        gL_Headers.Source_Tran_Id = id;
                        db.GL_Headers.Add(gL_Headers);
                        db.SaveChanges();
                    }
                    int glhv = 0;
                    if (gL_Headers.H_ID > 0)
                    {
                        glhv = gL_Headers.H_ID;
                    }
                    else
                    {
                        glhv = glheader.H_ID;
                    }
                    var EntriesProject = db.Project_Entries.Where(x => x.Transaction_ID == id && x.Entry_Type == "Rebate" && x.Status != "Transferred").ToList();
                    if (EntriesProject != null)
                    {
                        foreach (var item in EntriesProject)
                        {
                            GL_Lines gL_Lines = new GL_Lines();
                            gL_Lines.H_ID = glhv;
                            gL_Lines.C_CODE = Convert.ToInt32(item.C_CODE);
                            gL_Lines.Debit = item.Debit;
                            gL_Lines.Credit = item.Credit;
                            gL_Lines.Created_By = "1";
                            gL_Lines.Created_On = DateTime.Now;
                            // gL_Lines.Description = glhv;              

                            db.GL_Lines.Add(gL_Lines);
                            db.SaveChanges();
                            var be = db.Project_Entries.Where(x => x.E_ID == item.E_ID).FirstOrDefault();
                            be.Status = "Transferred";
                            be.Updated_By = "1";
                            be.Updated_On = DateTime.Now;
                            db.SaveChanges();
                        }

                    }
                    #endregion
                }
                else
                {
                    propertySales.AuthorizeStatus = false;
                    db.SaveChanges();
                    return NotFound();
                }

            }
            return Ok();
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
        [NonAction]
        private int GenerateCOACombinations(string CCode)
        {
            var ExistedCOA_Combinations = db.COA_Combinations.Where(x => x.C_Code == CCode).FirstOrDefault();
            if (ExistedCOA_Combinations != null)
            {
                return ExistedCOA_Combinations.C_ID;
            }
            else
            {
                string[] codeParts = CCode.Split('.');
                COA_Combinations cOA_Combinations = new COA_Combinations();
                cOA_Combinations.C_Code = CCode;
                cOA_Combinations.Company = codeParts[0];
                cOA_Combinations.Project = codeParts[1];
                cOA_Combinations.Location = codeParts[2];
                cOA_Combinations.Account = codeParts[3];
                cOA_Combinations.Party = codeParts[4];
                cOA_Combinations.Created_By = "1";
                cOA_Combinations.Created_ON = DateTime.Now;
                db.COA_Combinations.Add(cOA_Combinations);
                db.SaveChanges();
                return cOA_Combinations.C_ID;
            }

        }
    }
}