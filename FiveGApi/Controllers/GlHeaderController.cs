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
            ///changes on 15-03-2021-------
            if(gL_Headers.Trans_Status=="Posted")
            {
                existgL_Headers.Posted_date = DateTime.Now.ToString();

            }
            /// end   changes on 15-03-2021-------

            existgL_Headers.Trans_Status=   gL_Headers.Trans_Status;
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
                DateTime jdate = (DateTime)gL_Headers.J_Date;
                var Month = jdate.ToString("MMM").ToUpper();
                var year = jdate.ToString("yyyy");
                if (gL_Headers.Source== "Manual")
                {
                    var getLastRefFieldEntry = db.GL_Headers.Where(x => x.J_Date.Month == jdate.Month && x.J_Date.Year ==jdate.Year && x.Source == "Manual").AsQueryable().OrderByDescending(x => x.H_ID).Select(x => x.Ref_Field).FirstOrDefault();
                    if(getLastRefFieldEntry != null)
                    {
                        var result = getLastRefFieldEntry.Substring(getLastRefFieldEntry.Length - 4);
                        int value = Convert.ToInt32(result);
                        value = value + 1;
                        var val=value.ToString();
                        var len = val.Length;
                        if (len == 1)
                            val = "000" + val;
                        else if (len == 2)
                            val = "00" + val;
                        else if (len == 3)
                            val = "0" + val;
                        gL_Headers.Ref_Field = "JV" + Month + val;
                    }
                    else
                    {
                        gL_Headers.Ref_Field = "JV" + Month + "0001";
                    }
                }
                else if (gL_Headers.Source == "Deposit")
                {
                    var mode = "BRV";
                    var getLastRefFieldEntry = db.GL_Headers.Where(x => x.J_Date.Month == jdate.Month && x.J_Date.Year == jdate.Year && x.Source == "Deposit").AsQueryable().OrderByDescending(x => x.H_ID).Select(x=>x.Ref_Field).FirstOrDefault();
                    if(gL_Headers.Dep_Mode== "CASH")
                    {
                        mode = "CRV";
                    }
                    if (getLastRefFieldEntry != null)
                    {
                        var result = getLastRefFieldEntry.Substring(getLastRefFieldEntry.Length - 4);
                        int value = Convert.ToInt32(result);
                        value = value + 1;
                        var val = value.ToString();
                        var len = val.Length;
                        if (len == 1)
                            val = "000" + val;
                        else if (len == 2)
                            val = "00" + val;
                        else if (len == 3)
                            val = "0" + val;
                        gL_Headers.Ref_Field = mode + Month + val;
                    }
                    else
                    {
                        gL_Headers.Ref_Field = mode + Month + "0001";
                    }
                }
                else if (gL_Headers.Source == "Payments")
                {
                    var mode = "BPV";
                    var getLastRefFieldEntry = db.GL_Headers.Where(x => x.J_Date.Month == jdate.Month && x.J_Date.Year == jdate.Year && x.Source == "Payments").AsQueryable().OrderByDescending(x => x.H_ID).Select(x => x.Ref_Field).FirstOrDefault();
                    if (gL_Headers.Pay_Mode == "CASH")
                    {
                        mode = "CPV";
                    }
                    if (getLastRefFieldEntry != null)
                    {
                        var result = getLastRefFieldEntry.Substring(getLastRefFieldEntry.Length - 4);
                        int value = Convert.ToInt32(result);
                        value = value + 1;
                        var val = value.ToString();
                        var len = val.Length;
                        if (len == 1)
                            val = "000" + val;
                        else if (len == 2)
                            val = "00" + val;
                        else if (len == 3)
                            val = "0" + val;
                        gL_Headers.Ref_Field = mode + Month + val;
                    }
                    else
                    {
                        gL_Headers.Ref_Field = mode + Month + "0001";
                    }
                }                
                gL_Headers.GL_Lines = gL_Headers.GL_Lines.Select(x => { x.Created_By = "Admin";x.Created_On = DateTime.Now; return x; }).ToList() ;
                gL_Headers.Created_By = "Admin";
                gL_Headers.Created_On = DateTime.Now;
                db.GL_Headers.Add(gL_Headers);
                db.SaveChanges();
                foreach (var gllines in gL_Headers.GL_Lines)
                {
                    var glBalance = db.GL_Balances.Where(x => x.C_CODE == gllines.C_CODE).AsQueryable().FirstOrDefault();
                    if(glBalance!=null)
                    {
                        if(gllines.Credit!=null)
                        {
                            glBalance.Credit = glBalance.Credit + gllines.Credit;
                        }
                        if(gllines.Debit!=null)
                        {
                            glBalance.Debit = glBalance.Debit + gllines.Debit;

                        }
                        glBalance.Effect_Trans_ID = gllines.L_ID.ToString();
                        glBalance.Updated_By = "Admin";
                        glBalance.Updated_On = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        GL_Balances gL_Balances = new GL_Balances();
                        if (gllines.Credit != null)
                        {
                            gL_Balances.Credit =  gllines.Credit;
                        }
                        if (gllines.Debit != null)
                        {
                            gL_Balances.Debit =  gllines.Debit;

                        }                        
                        //gL_Balances.Debit = gllines.Debit;
                        gL_Balances.C_CODE = gllines.C_CODE;
                        gL_Balances.Bal_Date = DateTime.Now;
                        gL_Balances.Effect_Trans_ID = gllines.L_ID.ToString();
                        gL_Balances.Created_By ="Admin";
                        gL_Balances.Created_On =DateTime.Now;
                        db.GL_Balances.Add(gL_Balances);
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                
            }


            return CreatedAtRoute("DefaultApi", new { id = gL_Headers.H_ID }, gL_Headers);
        }
        [HttpGet]
        [Route("GlheaderLinesExistOrNot")]
        public IHttpActionResult GetGlheaderLinesExistOrNot(int GlHeaderID)
        {
            var GlLineCount = db.GL_Lines.Where(x => x.H_ID == GlHeaderID).AsQueryable().Count();
            if(GlLineCount>0)
            {
                return Ok(GlLineCount);
            }
            else
            {
                return NotFound();
            }
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
        public IList<FiveGApi.Models.GL_Headers> Gl_gL_HeaderByVlaues(DateTime? startDate, DateTime? endDate, string source, string referance,string status)
        {

            IList<GL_Headers> gL_Headers;
            if (referance!=null)
            {
                if (source != null)
                {
                    
                    if(startDate!=null && endDate != null)
                    {

                        gL_Headers =db.GL_Headers.Where(x => x.Source == source&&x.Description==referance&& x.J_Date>=startDate&&x.J_Date<=endDate).ToList();
                    }
                    else if(startDate!=null && endDate==null)
                    {
                        gL_Headers = db.GL_Headers.Where(x => x.Source == source && x.Description == referance && x.J_Date >= startDate ).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        gL_Headers = db.GL_Headers.Where(x => x.Source == source && x.Description == referance && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        gL_Headers = db.GL_Headers.Where(x => x.Source == source && x.Description == referance).ToList();

                    }

                }
                else
                { // return db.GL_Headers.Where(x => x.Source == source).ToList();
                    if (startDate != null && endDate != null)
                    {
                        gL_Headers = db.GL_Headers.Where(x=> x.Description == referance && x.J_Date >= startDate && x.J_Date <= endDate).ToList();
                    }
                    else if (startDate != null && endDate == null)
                    {
                        gL_Headers = db.GL_Headers.Where(x=> x.Description == referance && x.J_Date >= startDate).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        gL_Headers = db.GL_Headers.Where(x=> x.Description == referance && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        gL_Headers = db.GL_Headers.Where(x =>  x.Description == referance).ToList();

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
                        gL_Headers = db.GL_Headers.Where(x => x.Source == source && x.J_Date >= startDate && x.J_Date <= endDate).ToList();
                    }
                    else if (startDate != null && endDate == null)
                    {
                        gL_Headers = db.GL_Headers.Where(x => x.Source == source && x.J_Date >= startDate).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        gL_Headers = db.GL_Headers.Where(x => x.Source == source && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        gL_Headers = db.GL_Headers.Where(x => x.Source == source ).ToList();

                    }

                }
                else
                { // return db.GL_Headers.Where(x => x.Source == source).ToList();
                    if (startDate != null && endDate != null)
                    {
                        gL_Headers = db.GL_Headers.Where(x =>  x.Description == referance && x.J_Date >= startDate && x.J_Date <= endDate).ToList();
                    }
                    else if (startDate != null && endDate == null)
                    {
                        gL_Headers = db.GL_Headers.Where(x =>  x.Description == referance && x.J_Date >= startDate).ToList();

                    }
                    else if (startDate == null && endDate != null)
                    {
                        gL_Headers = db.GL_Headers.Where(x =>  x.Description == referance && x.J_Date <= endDate).ToList();

                    }
                    else
                    {
                        gL_Headers = db.GL_Headers.ToList();

                    }
                }
            }
            if(status=="Posted")
            {
                return gL_Headers.OrderBy(x => x.Trans_Status == "Posted").ToList();
            }
            else if (status == "UnPosted")
            {
                return gL_Headers.OrderBy(x => x.Trans_Status == "UnPosted").ToList();

            }
            else
            {
                return gL_Headers;
            }
            return null;
            
        }
    }
}











