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
using FiveGApi.Models;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/GL_Header")]
    public class GlHeaderController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/GL_Headers
        public IQueryable<GL_Headers> GetAllGL_Headers()
        {
            IQueryable<GL_Headers> GL_Headers;

            try
            {
                GL_Headers = db.GL_Headers;

            }
            catch (Exception ex)
            {

                throw;
            }
            return GL_Headers;
        }

        // GET: api/GL_Headers/5
        [ResponseType(typeof(GL_Headers))]
        public IHttpActionResult GetGL_HeadersByID(int id)
        {
            GL_Headers gL_Headers = db.GL_Headers.Find(id);

            if (gL_Headers == null)
            {
                return NotFound();
            }

            return Ok(gL_Headers);
        }

        // PUT: api/GL_Headers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGL_Headers(int id, GL_Headers gL_Headers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existgL_Headers = db.GL_Headers.Where(x => x.H_ID == id).FirstOrDefault();
            existgL_Headers.J_Date = gL_Headers.J_Date;
            existgL_Headers.Doc_Date = gL_Headers.Doc_Date;
            existgL_Headers.Currency = gL_Headers.Currency;
            existgL_Headers.Description = gL_Headers.Description;
            existgL_Headers.Remarks = gL_Headers.Remarks;
            existgL_Headers.Source = gL_Headers.Source;
            existgL_Headers.Source_Tran_Id = gL_Headers.Source_Tran_Id;
            existgL_Headers.Pay_Mode = gL_Headers.Pay_Mode;
            existgL_Headers.Pay_Bank = gL_Headers.Pay_Bank;
            existgL_Headers.Pay_To = gL_Headers.Pay_To;
            existgL_Headers.Pay_Company =  gL_Headers.Pay_Company;
            existgL_Headers.Pay_Project =  gL_Headers.Pay_Project;
            existgL_Headers.Pay_Location = gL_Headers.Pay_Location;
            existgL_Headers.Dep_Mode =     gL_Headers.Dep_Mode;
            existgL_Headers.Dep_Bank =     gL_Headers.Dep_Bank;
            existgL_Headers.Dep_Company =  gL_Headers.Dep_Company;
            existgL_Headers.Dep_Project =  gL_Headers.Dep_Project;
            existgL_Headers.Dep_Location = gL_Headers.Dep_Location;
            existgL_Headers.Trans_Status = gL_Headers.Trans_Status;
            existgL_Headers.Posted_date=   gL_Headers.Posted_date;
            existgL_Headers.Flex_1=        gL_Headers.Flex_1;
            existgL_Headers.Flex_2 =       gL_Headers.Flex_2; 
            existgL_Headers.Updated_By=    "Admin";
            existgL_Headers.Updated_On = DateTime.Now;
            gL_LinesUpdate(existgL_Headers.H_ID, gL_Headers.GL_Lines);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gL_HeadersExists(gL_Headers.H_ID))
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

        // POST: api/GL_Headers
        [ResponseType(typeof(GL_Headers))]
        public IHttpActionResult PostgL_Headers(GL_Headers gL_Headers)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }               
                gL_Headers.GL_Lines = gL_Headers.GL_Lines.Select(x => { x.Created_By = "Admin";x.Created_On = DateTime.Now; return x; }).ToList() ;
                gL_Headers.Created_By = "Admin";
                gL_Headers.Created_On = DateTime.Now;
                db.GL_Headers.Add(gL_Headers);
                db.SaveChanges();
            }
            catch (Exception)
            {
                
            }


            return CreatedAtRoute("DefaultApi", new { id = gL_Headers.H_ID }, gL_Headers);
        }

        // DELETE: api/GL_Headers/5
        [ResponseType(typeof(GL_Headers))]
        public IHttpActionResult DeleteGL_Headers(int id)
        {
            GL_Headers GL_Headers = db.GL_Headers.Find(id);
            if (GL_Headers == null)
            {
                return NotFound();
            }

            db.GL_Headers.Remove(GL_Headers);
            db.SaveChanges();

            return Ok(GL_Headers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool gL_HeadersExists(int id)
        {
            return db.GL_Headers.Count(e => e.H_ID == id) > 0;
        }

        

        
        private IQueryable<FiveGApi.Models.GL_Lines> gL_LinesUpdate(int H_ID, ICollection<FiveGApi.Models.GL_Lines> gL_Lines)
        {
            var gllinesCount = gL_Lines.Count();

            foreach (var item in gL_Lines)
            {
                var existgL_LinesExisted = db.GL_Lines.Where(x => x.L_ID == item.L_ID).FirstOrDefault();

                if (existgL_LinesExisted!=null)
                {
                    existgL_LinesExisted.H_ID       = item.H_ID;
                    existgL_LinesExisted.C_CODE     = item.C_CODE;
                    existgL_LinesExisted.Debit      = item.Debit;
                    existgL_LinesExisted.Credit     = item.Credit;
                    existgL_LinesExisted.Description= item.Description;
                    existgL_LinesExisted.Flex_1     = item.Flex_1;
                    existgL_LinesExisted.Flex_2     = item.Flex_2;           
                    existgL_LinesExisted.Updated_By = "Admin" ;
                    existgL_LinesExisted.Updated_On = DateTime.Now;
                    db.SaveChanges();
                }                                     
                else
                {

                    item.H_ID = H_ID;
                    item.Created_By = "Admin";
                    item.Created_On = DateTime.Now;                   
                    db.GL_Lines.Add(item);
                    db.SaveChanges();
                }               
            }
            return db.GL_Lines.Where(x => x.H_ID == H_ID);
        }
        [Route("gLLinesByVlaues")]
        [HttpGet]
        public IList<FiveGApi.Models.GL_Headers> Gl_gL_HeaderByVlaues(DateTime? startDate, DateTime? endDate, string source, string referance)
        {
            
            
            if (referance!=null)
            {
                if (source != null)
                {
                    
                    if(startDate!=null && endDate != null)
                    {
                        
                        return db.GL_Headers.Where(x => x.Source == source&&x.Description==referance&& x.J_Date>=startDate&&x.J_Date<=endDate).ToList();
                    }
                    else if(startDate!=null && endDate==null)
                    {
                        return db.GL_Headers.Where(x => x.Source == source && x.Description == referance && x.J_Date >= startDate ).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        return db.GL_Headers.Where(x => x.Source == source && x.Description == referance && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        return db.GL_Headers.Where(x => x.Source == source && x.Description == referance).ToList();

                    }

                }
                else
                { // return db.GL_Headers.Where(x => x.Source == source).ToList();
                    if (startDate != null && endDate != null)
                    {
                        return db.GL_Headers.Where(x=> x.Description == referance && x.J_Date >= startDate && x.J_Date <= endDate).ToList();
                    }
                    else if (startDate != null && endDate == null)
                    {
                        return db.GL_Headers.Where(x=> x.Description == referance && x.J_Date >= startDate).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        return db.GL_Headers.Where(x=> x.Description == referance && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        return db.GL_Headers.Where(x =>  x.Description == referance).ToList();

                    }
                }
            }
            else
            {
                if (source != null)
                {
                    // return db.GL_Headers.Where(x => x.Source == source).ToList();
                    if (startDate != null && endDate != null)
                    {
                        return db.GL_Headers.Where(x => x.Source == source && x.J_Date >= startDate && x.J_Date <= endDate).ToList();
                    }
                    else if (startDate != null && endDate == null)
                    {
                        return db.GL_Headers.Where(x => x.Source == source && x.J_Date >= startDate).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        return db.GL_Headers.Where(x => x.Source == source && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        return db.GL_Headers.Where(x => x.Source == source ).ToList();

                    }

                }
                else
                { // return db.GL_Headers.Where(x => x.Source == source).ToList();
                    if (startDate != null && endDate != null)
                    {
                        return db.GL_Headers.Where(x =>  x.Description == referance && x.J_Date >= startDate && x.J_Date <= endDate).ToList();
                    }
                    else if (startDate != null && endDate == null)
                    {
                        return db.GL_Headers.Where(x =>  x.Description == referance && x.J_Date >= startDate).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        return db.GL_Headers.Where(x =>  x.Description == referance && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        return db.GL_Headers.ToList();

                    }
                }
            }
            return null;
            
        }
    }
}











