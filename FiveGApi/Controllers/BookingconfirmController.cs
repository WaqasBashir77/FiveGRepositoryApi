using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Bookingconfirm")]
    public class BookingconfirmController : ApiController
    {


        private MIS_DBEntities1 db = new MIS_DBEntities1();
        [Route("GetALLBookingConfirmed")]
        [HttpGet]       
        // GET: api/BookingConfirm
        [ResponseType(typeof(IQueryable<BookingConfirm>))]
        public IQueryable<BookingConfirm> GetALLBookingConfirmed()
        {
            IQueryable<BookingConfirm> bookingConfirm;

            try
            {
                bookingConfirm = db.BookingConfirms;
            }
            catch (Exception)
            {
                throw;
            }
            return bookingConfirm;
           
        }
        [Route("GetALLBookingConfirmedByFormNumberOrFormID")]
        [HttpGet]
        // GET: api/BookingConfirm
        [ResponseType(typeof(IQueryable<BookingConfirm>))]
        public IQueryable<BookingConfirm> GetALLBookingConfirmedByFormNumberOrFormID(string refNumber,string formNumber)
        {
            IQueryable<BookingConfirm> bookingConfirm;

            try
            {
                if(refNumber==null)
                {
                    bookingConfirm = db.BookingConfirms.Where(x=> x.Form_num == formNumber);

                }else if(formNumber==null)
                {
                    bookingConfirm = db.BookingConfirms.Where(x => x.Ref_num == refNumber);

                }
                else
                {
                    bookingConfirm = db.BookingConfirms.Where(x => x.Ref_num == refNumber || x.Form_num == formNumber);

                }
            }
            catch (Exception)
            {
                throw;
            }
            return bookingConfirm;

        }

        // GET: api/BookingConfirm/5
        [ResponseType(typeof(BookingConfirm))]
        public IHttpActionResult GetBookingConf(int id)
        {
            BookingConfirm BookingConfirm = db.BookingConfirms.Find(id);

            if (BookingConfirm == null)
            {
                return NotFound();
            }

            return Ok(BookingConfirm);
        }

        // PUT: api/BookingConfirm/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBookingConf(int id, BookingConfirm BookingConfirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existBookingConfirm = db.BookingConfirms.Where(x => x.ID == id).FirstOrDefault();
            if (existBookingConfirm != null)
            {
                var property = db.PropertyDefs.Where(x => x.ID == BookingConfirm.Property_ID).FirstOrDefault();

                existBookingConfirm.Ref_num = BookingConfirm.Ref_num;
                existBookingConfirm.Form_num = BookingConfirm.Form_num;
                existBookingConfirm.Authorize_Status = BookingConfirm.Authorize_Status;
                existBookingConfirm.File_Status = BookingConfirm.File_Status;
                existBookingConfirm.Replaced_Form = BookingConfirm.Replaced_Form;
                existBookingConfirm.Applicant_name = BookingConfirm.Applicant_name;
                existBookingConfirm.CNIC = BookingConfirm.CNIC;
                existBookingConfirm.Form_Rec_Date = BookingConfirm.Form_Rec_Date;
                existBookingConfirm.Contact_Num = BookingConfirm.Contact_Num;
                existBookingConfirm.Property_ID = BookingConfirm.Property_ID;
                existBookingConfirm.Member_Num = BookingConfirm.Member_Num;
                existBookingConfirm.Book_Emp = BookingConfirm.Book_Emp;
                existBookingConfirm.Book_Dealer = BookingConfirm.Book_Dealer;
                existBookingConfirm.MS_amount = BookingConfirm.MS_amount;
                existBookingConfirm.Remarks = BookingConfirm.Remarks;
                existBookingConfirm.Confirmation_Date = BookingConfirm.Confirmation_Date;
                existBookingConfirm.Updated_By = BookingConfirm.Updated_By;
                existBookingConfirm.Updated_On = BookingConfirm.Updated_On;
                decimal bookingpercentage = (decimal)(property.BookingCommision / 100);
                decimal confirmpercentage = (decimal)(property.ConfirmationCommision / 100);
                existBookingConfirm.Emp_B_RAmt = ((((property.Price / 100) * BookingConfirm.Emp_Rebate)) * bookingpercentage);
                existBookingConfirm.Dealer_B_RAmt = ((((property.Price / 100) * BookingConfirm.Dealer_Rebate)) * bookingpercentage);
                existBookingConfirm.Com_B_RAmt = (((property.Price / 100) * BookingConfirm.Rebate_Percent)) * bookingpercentage;
                existBookingConfirm.Emp_C_RAmt = ((((property.Price / 100) * BookingConfirm.Emp_Rebate)) * confirmpercentage);
                existBookingConfirm.Dealer_C_RAmt = ((((property.Price / 100) * BookingConfirm.Dealer_Rebate)) * confirmpercentage);
                existBookingConfirm.Com_C_RAmt = (((property.Price / 100) * BookingConfirm.Rebate_Percent)) * confirmpercentage;

                // db.Entry(BookingConfirm).State = EntityState.Modified;
                existBookingConfirm.Updated_On = DateTime.Now.ToString();

                try
                {
                    db.SaveChanges();
                    //BookingPaymentDetails(id, BookingConfirm);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingConfExists((int)BookingConfirm.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return StatusCode(HttpStatusCode.OK);
            }else
            {
                var error = new { message = "Not Exist Entity against this" }; //<-- anonymous object
                return this.Content(HttpStatusCode.NotFound, error);
            }
        }

        // POST: api/BookingConfirm
       // [Route("PostBookingConf")]
        [ResponseType(typeof(BookingConfirm))]
        public IHttpActionResult PostBookingConf(BookingConfirm BookingConfirm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var property = db.PropertyDefs.Where(x => x.ID == BookingConfirm.Property_ID).FirstOrDefault();
            BookingConfirm.Booking_Percent = property.Booking_percent;
            BookingConfirm.Booking_amount = ((property.Price / 100) * property.Booking_percent);
            BookingConfirm.Confirm_Percent = property.Confirm_percent;
            BookingConfirm.Confirm_amount = ((property.Price / 100) * property.Confirm_percent);
            BookingConfirm.Tax_Percent = property.Tax_percent;
            BookingConfirm.Total_amount = property.Price;
            BookingConfirm.Rebate_Percent = property.Rebate_percent;
            int employeeID = Convert.ToInt32(BookingConfirm.Book_Emp);
            var employeRebate=db.Rebate_Details.Where(x => x.Reg_ID == employeeID).Select(x => x.Rebate).FirstOrDefault();
            BookingConfirm.Emp_Rebate = employeRebate;
            int dealerID = Convert.ToInt32(BookingConfirm.Book_Dealer);
            var dealerRebate = db.Rebate_Details.Where(x => x.Reg_ID == dealerID).Select(x => x.Rebate).FirstOrDefault();
            BookingConfirm.Dealer_Rebate = dealerRebate;            
            BookingConfirm.Flex_1 = "true";
            BookingConfirm.Flex_2 = "true";
            BookingConfirm.Created_By = "1";
            BookingConfirm.Payment_B_Status = "unPaid";
            BookingConfirm.Payment_C_Status = "unPaid";
            BookingConfirm.Payment_MSFee_Status = "unPaid";
            BookingConfirm.Flex_1 = "1";
            BookingConfirm.Flex_2 = "1";
            BookingConfirm.Created_By = "1";
            BookingConfirm.Created_ON = DateTime.Now;
            decimal bookingpercentage = (decimal)(property.BookingCommision / 100);
            decimal confirmpercentage = (decimal)(property.ConfirmationCommision / 100);
            BookingConfirm.Emp_B_RAmt = ((((property.Price / 100) * BookingConfirm.Emp_Rebate) )*bookingpercentage);
            BookingConfirm.Dealer_B_RAmt = ((((property.Price / 100) * BookingConfirm.Dealer_Rebate) )*bookingpercentage);
            BookingConfirm.Com_B_RAmt = (((property.Price / 100) * BookingConfirm.Rebate_Percent))*bookingpercentage;
            BookingConfirm.Emp_C_RAmt = ((((property.Price / 100) * BookingConfirm.Emp_Rebate) ) * confirmpercentage);
            BookingConfirm.Dealer_C_RAmt = ((((property.Price / 100) * BookingConfirm.Dealer_Rebate) ) * confirmpercentage);
            BookingConfirm.Com_C_RAmt = (((property.Price / 100) * BookingConfirm.Rebate_Percent) ) * confirmpercentage;

            //BookingConfirm.Emp_B_RAmt = (((property.Price / 100) * employeRebate) / 2);
            //BookingConfirm.Emp_C_RAmt = (((property.Price / 100) * employeRebate) / 2);
            //BookingConfirm.Dealer_B_RAmt = (((property.Price / 100) * dealerRebate) / 2);
            //BookingConfirm.Dealer_C_RAmt = (((property.Price / 100) * dealerRebate) / 2);
            //BookingConfirm.Com_B_RAmt = (((property.Price / 100) * employeRebate) / 2);
            //BookingConfirm.Com_C_RAmt = ((property.Price / 100) * property.Rebate_percent);
            db.BookingConfirms.Add(BookingConfirm);
            db.SaveChanges();
            insertBookingEntries(BookingConfirm.ID, property, BookingConfirm);
            return CreatedAtRoute("DefaultApi", new { id = BookingConfirm.ID }, BookingConfirm);
        }

        // DELETE: api/BookingConfirm/5
        [ResponseType(typeof(BookingConfirm))]
        public IHttpActionResult DeleteBookingConf(int id)
        {
            BookingConfirm BookingConfirm = db.BookingConfirms.Find(id);
            if (BookingConfirm == null)
            {
                return NotFound();
            }

            db.BookingConfirms.Remove(BookingConfirm);
            db.SaveChanges();

            return Ok(BookingConfirm);
        }
        // DELETE: api/BookingConfirm/5
        [Route("AuthorizedBookingConfCount")]
        [ResponseType(typeof(BookingConfirm))]
        [HttpGet]
        public IHttpActionResult AuthorizedBookingConfCount(int Propertyid)
        {
            var BookingConfirm = db.BookingConfirms.Where(x=>x.Authorize_Status== "Authorized"&&x.Property_ID== Propertyid).Count();
            return Ok(BookingConfirm);
        }
        [Route("BookingConfByPropertyID")]
        [ResponseType(typeof(List<BookingConfirm>))]
        [HttpGet]
        public IHttpActionResult BookingConfByPropertyID(int Propertyid)
        {
            var BookingConfirm = db.BookingConfirms.Where(x => x.Property_ID == Propertyid).ToList();
            return Ok(BookingConfirm);
        }
        [Route("UnAuthorizedBookingConfCount")]
        [ResponseType(typeof(BookingConfirm))]
        [HttpGet]
        public IHttpActionResult UnAuthorizedBookingConfCount(int Propertyid)
        {
            var BookingConfirm = db.BookingConfirms.Where(x => x.Authorize_Status != "Authorized" && x.Property_ID == Propertyid).Count();
            return Ok(BookingConfirm);
        }
        [Route("BookingConfTotalPaymentsCount")]
        [ResponseType(typeof(BookingPayment))]
        [HttpGet]
        public IHttpActionResult BookingConfTotalPaymentsCount(int BookingID)
        {
            var BookingConfirm = db.BookingPayments.Where(x=> x.ID == BookingID).Count();
            return Ok(BookingConfirm);
        }
        [Route("BookingPaymentsTotalAuthorizedPaymentsCount")]
        [ResponseType(typeof(BookingPayment))]
        [HttpGet]
        public IHttpActionResult BookingPaymentsTotalAuthorizedPaymentsCount(int BookingID)
        {
            var BookingConfirm = db.BookingPayments.Where(x => x.Authorize_Status == "Authorized" && x.ID == BookingID).Count();
            return Ok(BookingConfirm);
        }
        [Route("BookingPaymentsTotalUnAuthorizedPaymentsCount")]
        [ResponseType(typeof(BookingPayment))]
        [HttpGet]
        public IHttpActionResult BookingPaymentsTotalUnAuthorizedPaymentsCount(int BookingID)
        {
            var BookingConfirm = db.BookingPayments.Where(x => x.Authorize_Status != "Authorized" && x.ID == BookingID).Count();
            return Ok(BookingConfirm);
        }
        [Route("BookingTotalPayments")]
        [ResponseType(typeof(BookingPayment))]
        [HttpGet]
        public IHttpActionResult BookingTotalPayments(int BookingID)
        {            
            var result = db.BookingPayments.Where(o => o.ID==BookingID)
                   .Sum(g => g.Payment_amount);

            return Ok(result);
        }
        [Route("BookingAuthorizedTotalPayments")]
        [ResponseType(typeof(BookingPayment))]
        [HttpGet]
        public IHttpActionResult BookingAuthorizedTotalPayments(int BookingID)
        {
            var result = db.BookingPayments.Where(x => x.ID == BookingID && x.Authorize_Status == "Authorized")
                   .Sum(g => g.Payment_amount);

            return Ok(result);
        }
        [Route("BookingUnAuthorizedTotalPayments")]
        [ResponseType(typeof(BookingPayment))]
        [HttpGet]
        public IHttpActionResult BookingUnAuthorizedTotalPayments(int BookingID)
        {
            var result = db.BookingPayments.Where(x => x.ID == BookingID && x.Authorize_Status != "Authorized")
                   .Sum(g => g.Payment_amount);

            return Ok(result);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Route("AuthorizeBookingConfirm")]
        [ResponseType(typeof(void))]
        [HttpGet]
        public IHttpActionResult AuthorizeBookingConfirm(int BookingID,string AuthorizedBy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existBooking_Confirm = db.BookingConfirms.Where(x => x.ID == BookingID).FirstOrDefault();
            existBooking_Confirm.Authorize_By = AuthorizedBy;
            existBooking_Confirm.Authorize_Status = "Authorized";
            existBooking_Confirm.Authorize_Date = DateTime.Now;
            if (existBooking_Confirm != null)
            {
                try
                {
                    db.SaveChanges();
                    //BookingPaymentDetails(id, BookingConfirm);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingConfExists((int)existBooking_Confirm.ID))
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
            else
            {
                var error = new { message = "Not Exist Entity against this" }; //<-- anonymous object
                return this.Content(HttpStatusCode.NotFound, error);
            }
        }
        private bool BookingConfExists(int id)
        {
            return db.BookingConfirms.Count(e => e.ID == id) > 0;
        }
        [NonAction]
        public void insertBookingEntries(int bookingID,PropertyDef property,BookingConfirm bookingConfirm)
        {
            Booking_Entries booking_EntriesforROS = new Booking_Entries();
            booking_EntriesforROS.Transaction_ID = bookingID;
            booking_EntriesforROS.Entry_Date = DateTime.Now;
            booking_EntriesforROS.Entry_Type = "Rebate";
            booking_EntriesforROS.Created_By = "Admin";
            booking_EntriesforROS.Created_On = DateTime.Now;
            booking_EntriesforROS.Status = "Draft";
            var c_Code = property.Company + "." + property.Project + "." + property.Location;
            #region Compant Rebates
            //////----------------Receivable from society--------------------///////////
            ///-------------------Booking Rebate Debit------------------------------///////////
            var coa_Segment = db.COA_Segments.Where(x => x.Name == "Receivable from society").FirstOrDefault();
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
            booking_EntriesforROS.Debit = bookingConfirm.Com_B_RAmt;
            booking_EntriesforROS.Credit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            ///-------------------Confirmation Rebate Debit------------------------------///////////
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
            booking_EntriesforROS.Debit = bookingConfirm.Com_C_RAmt;
            booking_EntriesforROS.Credit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            //////----------------Commission income--------------------///////////
            ///-------------------Booking Rebate Credit------------------------------///////////            
            coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission income").FirstOrDefault();
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
            booking_EntriesforROS.Credit = bookingConfirm.Com_B_RAmt;
            booking_EntriesforROS.Debit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            ///-------------------Confirmation Rebate Credit------------------------------///////////            
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
            booking_EntriesforROS.Credit = bookingConfirm.Com_C_RAmt;
            booking_EntriesforROS.Debit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            ////////////////----------------------End Company--------------------//////////////////
            #endregion Company Rebates
            #region Staff Rebates
            //////----------------Receivable from society--------------------///////////
            ///-------------------Booking Rebate Debit------------------------------///////////
             coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission to staff").FirstOrDefault();
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
            booking_EntriesforROS.Debit = bookingConfirm.Emp_B_RAmt;
            booking_EntriesforROS.Credit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            ///-------------------Confirmation Rebate Debit------------------------------///////////
            var bEmpID = Convert.ToInt32(bookingConfirm.Book_Emp);
            var reg = db.Registrations.Where(x => x.ID == bEmpID).FirstOrDefault();
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + "."+reg.Code;
            booking_EntriesforROS.Credit = bookingConfirm.Emp_B_RAmt;
            booking_EntriesforROS.Debit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            //////----------------Commission payable to staff--------------------///////////
            ///-------------------Booking Rebate Credit------------------------------///////////            
            coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission payable to staff").FirstOrDefault();
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
            booking_EntriesforROS.Debit = bookingConfirm.Emp_C_RAmt;
            booking_EntriesforROS.Credit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            ///-------------------Confirmation Rebate Credit------------------------------///////////            
            booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + "."+ reg.Code;
            booking_EntriesforROS.Credit = bookingConfirm.Emp_C_RAmt;
            booking_EntriesforROS.Debit = 0;
            db.Booking_Entries.Add(booking_EntriesforROS);
            db.SaveChanges();
            ////////////////----------------------End Company--------------------//////////////////
            #endregion  Staff Rebates
            #region Agent Rebates
            if (bookingConfirm.Book_Dealer != null)
            {
                var bDeaID = Convert.ToInt32(bookingConfirm.Book_Dealer);
                //////----------------Commission to agent--------------------///////////
                ///-------------------Booking Rebate Debit------------------------------///////////
                coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission to agent").FirstOrDefault();
                booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
                booking_EntriesforROS.Debit = bookingConfirm.Dealer_B_RAmt;
                booking_EntriesforROS.Credit = 0;
                db.Booking_Entries.Add(booking_EntriesforROS);
                db.SaveChanges();
                ///-------------------Confirmation Rebate Debit------------------------------///////////
                 reg = db.Registrations.Where(x => x.ID == bDeaID).FirstOrDefault();
                booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + "." + reg.Code;
                booking_EntriesforROS.Credit = bookingConfirm.Dealer_B_RAmt;
                booking_EntriesforROS.Debit = 0;
                db.Booking_Entries.Add(booking_EntriesforROS);
                db.SaveChanges();
                //////----------------Commission payable to staff--------------------///////////
                ///-------------------Booking Rebate Credit------------------------------///////////            
                coa_Segment = db.COA_Segments.Where(x => x.Name == "Commission payable to agent").FirstOrDefault();
                booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + ".000";
                booking_EntriesforROS.Debit = bookingConfirm.Dealer_B_RAmt;
                booking_EntriesforROS.Credit = 0;
                db.Booking_Entries.Add(booking_EntriesforROS);
                db.SaveChanges();
                ///-------------------Confirmation Rebate Credit------------------------------///////////            
                booking_EntriesforROS.C_CODE = c_Code + "." + coa_Segment.Segment_Value + "." + reg.Code;
                booking_EntriesforROS.Credit = bookingConfirm.Dealer_B_RAmt;
                booking_EntriesforROS.Debit = 0;
                db.Booking_Entries.Add(booking_EntriesforROS);
                db.SaveChanges();
                ////////////////----------------------End Company--------------------//////////////////
            }
            #endregion Agent Reabtes
        }
        //private IQueryable<BookingPayment> BookingPaymentDetails(int BookingID, BookingConfirm BookingConfirm)
        //{


        //    foreach (var item in BookingConfirm.BookingPayments)
        //    {
        //        if (item.ID > 0)
        //        {
        //            var existBookingPaymentExisted = db.BookingPayments.Where(x => x.ID == item.ID).FirstOrDefault();
        //            existBookingPaymentExisted.Payment_mode = item.Payment_mode;
        //            existBookingPaymentExisted.Instrument_Type = item.Instrument_Type;
        //            existBookingPaymentExisted.Ins_Type = item.Ins_Type;
        //            existBookingPaymentExisted.Payment_amount = item.Payment_amount;
        //            existBookingPaymentExisted.instrument_number = item.instrument_number;
        //            existBookingPaymentExisted.instrument_bank = item.instrument_bank;
        //            existBookingPaymentExisted.instrument_bank_Branch = item.instrument_bank_Branch;
        //            existBookingPaymentExisted.instrument_date = item.instrument_date;
        //            existBookingPaymentExisted.instrument_remarks = item.instrument_remarks;
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            item.ID = BookingID;
        //            db.BookingPayments.Add(item);
        //            db.SaveChanges();
        //        }

        //    }
        //    return db.BookingPayments.Where(x => x.ID == BookingID);
        //}
    }
}

