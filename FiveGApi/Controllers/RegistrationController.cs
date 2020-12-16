using FiveGApi.DTOModels;
using FiveGApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    [RoutePrefix("api/Registration")]
       public class RegistrationController : ApiController
    {
        private FiveG_DBEntities db = new FiveG_DBEntities();

        // GET: api/AllRegistrations
        //[Route("api/Registration/GetRegistrations")]
        ////[HttpPost]
        //[ResponseType(typeof(IQueryable<Registration>))]
        public IQueryable<Registration> GetRegistrations()//[FromUri] PagingParameterModel pagingparametermodel)
        {
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
        [ResponseType(typeof(Registration))]
        public IHttpActionResult GetRegistration(int id)
        {
            Registration Registration = db.Registrations.Find(id);

            if (Registration == null)
            {
                return NotFound();
            }

            return Ok(Registration);
        }

        // PUT: api/Registrations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRegistration(int id, Registration Registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existRegistration = db.Registrations.Where(x => x.ID == id).FirstOrDefault();
            ////existRegistration.RegistrationDetails
            //if (existRegistration.RegistrationDetails.Count > 0)
            //{
            //    foreach (var item in existRegistration.RegistrationDetails.ToList())
            //    {
            //        existRegistration.RegistrationDetails.Remove(item);
            //    }
            //}

            //existRegistration.RegistrationDetails = Registration.RegistrationDetails;



            // db.Entry(Registration).State = EntityState.Modified;
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Registrations
        [ResponseType(typeof(Registration))]
        public IHttpActionResult PostRegistration(Registration Registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Reg = db.Registrations.Where(x=>x.Code==Registration.Code).FirstOrDefault();
            if(Reg!=null)
            {
                var error = new { message = "Party must be unique" }; //<-- anonymous object
                return this.Content(HttpStatusCode.Conflict, error);
                
            }

            db.Registrations.Add(Registration);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Registration.ID }, Registration);
        }

        // DELETE: api/Registrations/5
        [ResponseType(typeof(Registration))]
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
    }
}
