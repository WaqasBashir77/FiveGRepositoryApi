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
    public class Booking_PaymentController : ApiController
    {
        private MIS_DBEntities db = new MIS_DBEntities();

        // GET: api/Booking_Payments
        [ResponseType(typeof(IQueryable<Booking_Payments>))]
        public IQueryable<Booking_Payments> AllBooking_Payments([FromUri] PagingParameterModel pagingparametermodel)
        {
            var source= db.Booking_Payments;
            // Get's No of Rows Count   
            int count = source.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            // Setting Header  
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return (IQueryable<Booking_Payments>)items;
        }

        // GET: api/Booking_Payments/5
        [ResponseType(typeof(Booking_Payments))]
        public IHttpActionResult GetBooking_Payments(int id)
        {
            Booking_Payments Booking_Payments = db.Booking_Payments.Find(id);

            if (Booking_Payments == null)
            {
                return NotFound();
            }

            return Ok(Booking_Payments);
        }

        // PUT: api/Booking_Payments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooking_Payments(int id, Booking_Payments Booking_Payments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existBooking_Payments = db.Booking_Payments.Where(x => x.ID == id).FirstOrDefault();
            ////existBooking_Payments.Booking_PaymentsDetails
            //if (existBooking_Payments.Booking_PaymentsDetails.Count > 0)
            //{
            //    foreach (var item in existBooking_Payments.Booking_PaymentsDetails.ToList())
            //    {
            //        existBooking_Payments.Booking_PaymentsDetails.Remove(item);
            //    }
            //}

            //existBooking_Payments.Booking_PaymentsDetails = Booking_Payments.Booking_PaymentsDetails;



            // db.Entry(Booking_Payments).State = EntityState.Modified;
           // existBooking_Payments.up = DateTime.Now.ToString();
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Booking_PaymentsExists((int)Booking_Payments.ID))
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

        // POST: api/Booking_Payments
        [ResponseType(typeof(Booking_Payments))]
        public IHttpActionResult PostBooking_Payments(Booking_Payments Booking_Payments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Booking_Payments.Add(Booking_Payments);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Booking_Payments.ID }, Booking_Payments);
        }

        // DELETE: api/Booking_Payments/5
        [ResponseType(typeof(Booking_Payments))]
        public IHttpActionResult DeleteBooking_Payments(int id)
        {
            Booking_Payments Booking_Payments = db.Booking_Payments.Find(id);
            if (Booking_Payments == null)
            {
                return NotFound();
            }

            db.Booking_Payments.Remove(Booking_Payments);
            db.SaveChanges();

            return Ok(Booking_Payments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Booking_PaymentsExists(int id)
        {
            return db.Booking_Payments.Count(e => e.ID == id) > 0;
        }
    }
}