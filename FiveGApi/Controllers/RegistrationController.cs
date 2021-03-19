using FiveGApi.DTOModels;
using FiveGApi.Helper;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
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

    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        private string UserId;
        private User userSecurityGroup = new User();

        public RegistrationController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;
            userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().FirstOrDefault();

        }
        // GET: api/AllRegistrations
        [Route("GetALLRegistrations")]
        [HttpGet]
        [ResponseType(typeof(IQueryable<FiveGApi.Models.Registration>))]
        public IQueryable<FiveGApi.Models.Registration> GetALLRegistrations()//[FromUri] PagingParameterModel pagingparametermodel)
        {
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                return db.Registrations.Where(x => x.SecurityGroupId == userSecurityGroup.SecurityGroupId);
            else
                return db.Registrations;
            ////Get All Registration From DB
            //var source = db.Registrations.OrderBy(x=>x.ID);
            //// Get's No of Rows Count   
            //int count = source.Count();

            //// Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            //int CurrentPage = pagingparametermodel.pageNumber;

            //// Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            //int PageSize = pagingparametermodel.pageSize;

            //// Display TotalCount to Records to User  
            //int TotalCount = count;

            //// Calculating Totalpage by Dividing (No of Records / Pagesize)  
            //int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            //// Returns List of Customer after applying Paging   
            //var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            //// if CurrentPage is greater than 1 means it has previousPage  
            //var previousPage = CurrentPage > 1 ? "Yes" : "No";

            //// if TotalPages is greater than CurrentPage means it has nextPage  
            //var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            //// Object which we are going to send in header   
            //var paginationMetadata = new
            //{
            //    totalCount = TotalCount,
            //    pageSize = PageSize,
            //    currentPage = CurrentPage,
            //    totalPages = TotalPages,
            //    previousPage,
            //    nextPage
            //};

            //// Setting Header  
            //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            //// Returing List of Registration Collections  

            //return items;
        }

        // GET: api/Registrations/5
        [ResponseType(typeof(FiveGApi.Models.Registration))]
        public IHttpActionResult GetRegistrationByID(int id)
        {
            Registration Registration = new Registration();
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                Registration = db.Registrations.Where(x => x.ID == id && x.SecurityGroupId == userSecurityGroup.SecurityGroupId).FirstOrDefault();
            else
                Registration = db.Registrations.Find(id);
            if (Registration == null)
            {
                return NotFound();
            }
            return Ok(Registration);
        }
        [Route("GetPartyCodeValidation")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult GetPartyCodeValidation(string partyCode)
        {
            var Reg = db.Registrations.AsQueryable().Where(x => x.Code == partyCode).FirstOrDefault();
            if (Reg != null)
            {
                var error = new { message = "Party must be unique" }; //<-- anonymous object
                return this.Content(HttpStatusCode.Conflict, error);
            }

            return Ok(partyCode);
        }
        [Route("GetRegistrationByType")]
        [HttpGet]
        [ResponseType(typeof(List<FiveGApi.DTOModels.ResultViewModel>))]
        public IHttpActionResult GetRegistrationByType(string type)
        {
            var Registration = new List<ResultViewModel>();
            if (type == "staff")
            {
                 if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                    Registration = db.Registrations.Where(x => x.Type == type&&x.SecurityGroupId==userSecurityGroup.SecurityGroupId).Select(item => new ResultViewModel
                    {
                        ID = item.ID,
                        Name = item.StaffName,
                        Code = item.Code,
                        isSelected = false

                    }).ToList();
                else
                Registration= db.Registrations.Where(x => x.Type == type).Select(item => new ResultViewModel
                {
                    ID = item.ID,
                    Name = item.StaffName,
                    Code=item.Code,
                    isSelected = false

                }).ToList(); 
                if (Registration == null)
                {
                    return NotFound();
                }               
                    return Ok(Registration);
            }
            else
            {
                if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                    Registration = db.Registrations.Where(x => x.Type == type && x.SecurityGroupId == userSecurityGroup.SecurityGroupId).Select(item => new ResultViewModel
                    {
                        ID = item.ID,
                        Name = item.StaffName,
                        Code = item.Code,
                        isSelected = false

                    }).ToList();
                else
                    Registration = db.Registrations.Where(x => x.Type == type).Select(item => new ResultViewModel
                    {
                        ID = item.ID,
                        Name = item.StaffName,
                        Code = item.Code,
                        isSelected = false

                    }).ToList(); 
                if (Registration == null)
                {
                    return NotFound();
                }

                return Ok(Registration);
            }
            

           
        }
        // PUT: api/Registrations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRegistration(int id, FiveGApi.Models.Registration Registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existRegistration = db.Registrations.Where(x => x.ID == id).FirstOrDefault();
            var Reg = db.Registrations.Where(x => x.Code == existRegistration.Code && x.ID != id).FirstOrDefault();
            if (Reg != null)
            {
                var error = new { message = "Party Code must be unique" }; //<-- anonymous object
                return this.Content(HttpStatusCode.Conflict, error);
            }
            rebate_Details(id, Registration.Rebate_Details);
            existRegistration.Name = Registration.Name;
            existRegistration.StaffName = Registration.StaffName;
            existRegistration.Designation = Registration.Designation;
            existRegistration.Type = Registration.Type;
            existRegistration.Contact_no = Registration.Contact_no;
            existRegistration.CNIC = Registration.CNIC;
            existRegistration.Affliate_Staff_ID = Registration.Affliate_Staff_ID;
            existRegistration.Company = Registration.Company;
            existRegistration.Sub_Office = Registration.Sub_Office;
            existRegistration.GL_Mapping_ID = Registration.GL_Mapping_ID;
            existRegistration.Resale_Comm = Registration.Resale_Comm;
            existRegistration.Remarks = Registration.Remarks;
            existRegistration.Updated_By = userSecurityGroup.UserName;
            existRegistration.SecurityGroupId = userSecurityGroup.SecurityGroupId;
            existRegistration.Updated_On = DateTime.Now.ToString();
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistrationExists(Registration.ID))
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

        // POST: api/Registrations
        [ResponseType(typeof(FiveGApi.Models.Registration))]
        public IHttpActionResult PostRegistration(FiveGApi.Models.Registration Registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Reg = db.Registrations.Where(x => x.Code == Registration.Code).FirstOrDefault();
            var Seg = db.COA_Segments.Where(x => x.Segment_Value == Registration.Code && x.Type=="Party").FirstOrDefault();
            if (Reg != null)
            {
                var error = new { message = "Party must be unique" }; //<-- anonymous object
                return this.Content(HttpStatusCode.Conflict, error);

            }
            if (Seg != null)
            {
                var error = new { message = "Party Code allready exist in Chart Of Account" }; //<-- anonymous object
                return this.Content(HttpStatusCode.Conflict, error);

            }
            Registration.SecurityGroupId = userSecurityGroup.SecurityGroupId;
            Registration.Created_By = userSecurityGroup.UserName;
            Registration.Created_ON = DateTime.Now;
            db.Registrations.Add(Registration);
            Registration.Rebate_Details = Registration.Rebate_Details.Select(x => { x.Created_By = "Admin"; x.Created_ON = DateTime.Now; return x; }).ToList();
            foreach (var item in Registration.Rebate_Details)
            {
                item.Reg_ID = Registration.ID;
                db.Rebate_Details.Add(item);

            }
            db.SaveChanges();
            COA_Segments _cOA_Segments = new COA_Segments();
            if(Registration.Type== "Dealer")
            {
                _cOA_Segments.Name = Registration.Name;
            }
            if(Registration.Type == "Staff")
            {
                _cOA_Segments.Name = Registration.StaffName;

            }
            _cOA_Segments.Segment = "Party";
            _cOA_Segments.Segment_Value = Registration.Code;
            _cOA_Segments.Status = "Active";
            db.COA_Segments.Add(_cOA_Segments);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Registration.ID }, Registration);
        }
        
        // DELETE: api/Registrations/5
        [ResponseType(typeof(FiveGApi.Models.Registration))]
        public IHttpActionResult DeleteRegistration(int id)
        {
            Registration Registration = db.Registrations.Find(id);
            if (Registration == null)
            {
                return NotFound();
            }

            db.Registrations.Remove(Registration);
            db.SaveChanges();

            return Ok(Registration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegistrationExists(int id)
        {
            return db.Registrations.Count(e => e.ID == id) > 0;
        }

        private IQueryable<FiveGApi.Models.Rebate_Details> rebate_Details(int regID, ICollection<FiveGApi.Models.Rebate_Details> rebate_Details)
        {


            foreach (var item in rebate_Details)
            {
                if (item.R_ID > 0)
                {
                    var existRebateExisted = db.Rebate_Details.Where(x => x.R_ID == item.R_ID).FirstOrDefault();
                    existRebateExisted.Society_ID = item.Society_ID;
                    existRebateExisted.Project_ID = item.Project_ID;
                    existRebateExisted.Rebate = item.Rebate;
                    existRebateExisted.Updated_On = DateTime.Now.ToString();
                    existRebateExisted.Updated_By = "1";
                    db.SaveChanges();
                }
                else
                {
                    item.Reg_ID = regID;
                    item.Created_By = "1";
                    item.Created_ON = DateTime.Now;
                    db.Rebate_Details.Add(item);
                    db.SaveChanges();
                }

            }
            return db.Rebate_Details.Where(x => x.Reg_ID == regID);
        }

    }
}