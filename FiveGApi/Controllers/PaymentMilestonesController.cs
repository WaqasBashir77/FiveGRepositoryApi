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
    public class PaymentMilestonesController : ApiController
    {
        private string UserId;
        public PaymentMilestonesController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
        //private FiveG_DBEntities db = new FiveG_DBEntities();
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        // GET: api/PaymentMilestones
        public IHttpActionResult GetPaymentMilestones()
        {
            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            List<PaymentMilestone> paymentMilestonesList = new List<PaymentMilestone>();
            try
            {
                if (!SecurityGroupDTO.CheckSuperAdmin(groupId))
                    paymentMilestonesList = db.PaymentMilestones.Where(x => x.SecurityGroupId == groupId&& x.Status==true).OrderByDescending(x => x.Created_Date).AsQueryable().ToList();
                else
                    paymentMilestonesList = db.PaymentMilestones.Where(x=>x.Status == true).OrderByDescending(x => x.Created_Date).AsQueryable().ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return Ok(paymentMilestonesList);
        }

        // GET: api/PaymentMilestones/5
        [ResponseType(typeof(PaymentMilestone))]
        public IHttpActionResult GetPaymentMilestone(int id)
        {

            var re = Request;
            var headers = re.Headers;
            int groupId = 0;
            if (headers.Contains("GroupId"))
            {
                groupId = Convert.ToInt32(headers.GetValues("GroupId").First());
            }
            PaymentMilestone responseMilestone = new PaymentMilestone();
            PaymentMilestone paymentMilestone = db.PaymentMilestones.Find(id);
            if (paymentMilestone == null)
            {
                return NotFound();
            }
            bool isAdmin = SecurityGroupDTO.CheckSuperAdmin(groupId);
            if (paymentMilestone.SecurityGroupId != groupId && !isAdmin)
            {
                return Ok(responseMilestone);
            }
            else
            {
                responseMilestone = paymentMilestone;
            }

            return Ok(responseMilestone);
        }

        // PUT: api/PaymentMilestones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPaymentMilestone(int id, PaymentMilestone paymentMilestone)
        {
            ResponseModel response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                response.Code = 0;
                return Ok(response);
            }

            paymentMilestone.Id = id;
            if (id != paymentMilestone.Id)
            {
                return BadRequest();
            }

            var milestoneMaster = db.PaymentMilestones.Where(x => x.Id == id).FirstOrDefault();
            milestoneMaster.Tax = paymentMilestone.Tax;
            milestoneMaster.GracePeriodDays = paymentMilestone.GracePeriodDays;
            milestoneMaster.description = paymentMilestone.description;
            milestoneMaster.Status = paymentMilestone.Status;
            milestoneMaster.LateFeePercent = paymentMilestone.LateFeePercent;
            milestoneMaster.Update_By = paymentMilestone.Update_By;
            milestoneMaster.Update_Date = DateTime.Now;

            var tempmilestone = db.TempTableForInstallments.Where(x => x.parentId == paymentMilestone.PaymentScheduleCode).ToList();
            if (tempmilestone != null || tempmilestone.Count > 0)
            {
                foreach (var item in tempmilestone)
                {
                    db.TempTableForInstallments.Remove(item);
                }
              
            }
            var child = db.PaymentMilestoneDetails.Where(x => x.parentId == id).ToList();
            if (child != null || child.Count > 0)
            {
                foreach (var item in child)
                {
                    db.PaymentMilestoneDetails.Remove(item);
                }

            }
            if (paymentMilestone.PaymentMilestoneDetails != null && paymentMilestone.PaymentMilestoneDetails.Count > 0)
            {
                List<TempTableForInstallment> tempTableForInstallments = new List<TempTableForInstallment>();
                List<PaymentMilestoneDetail> paymentDetaillist = new List<PaymentMilestoneDetail>();
                foreach (var item in paymentMilestone.PaymentMilestoneDetails.ToList())
                {
                    for (int i = 1; i <= item.InstallmentNumber; i++)
                    {
                        TempTableForInstallment tempTableFor = new TempTableForInstallment();
                        tempTableFor.InstallmentType = item.Milestones;
                        tempTableFor.Frequency = item.Frequency;
                        tempTableFor.parentId = paymentMilestone.PaymentScheduleCode;
                        
                        if (item.Milestones == "Installment")
                        {
                            tempTableFor.Installment = "Installment " + i.ToString();
                        }
                        else
                        {
                            tempTableFor.Installment = item.Milestones;
                        }
                        tempTableForInstallments.Add(tempTableFor);
                        tempTableFor.Percentage = Convert.ToInt32(item.Percentage);
                    }
                }

                db.TempTableForInstallments.AddRange(tempTableForInstallments);

                foreach (var item in paymentMilestone.PaymentMilestoneDetails.ToList())
                {
                    PaymentMilestoneDetail paymentDetail = new PaymentMilestoneDetail();

                    paymentDetail.parentId = id;
                    paymentDetail.Percentage = item.Percentage;
                    paymentDetail.Milestones = item.Milestones;
                    paymentDetail.InstallmentNumber = item.InstallmentNumber;
                    paymentDetail.Frequency = item.Frequency;
                    paymentDetail.Update_By = paymentMilestone.Update_By;
                    paymentDetail.Update_Date = DateTime.Now;
                    paymentDetail.SecurityGroupId = paymentMilestone.SecurityGroupId;
                    paymentDetail.Created_By = milestoneMaster.Created_By;
                    paymentDetail.Created_Date = milestoneMaster.Created_Date;
                    paymentDetaillist.Add(paymentDetail);
                }

                db.PaymentMilestoneDetails.AddRange(paymentDetaillist);
            }

            //db.Entry(paymentMilestone).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                response.Code = 1;
            }
            catch (DbUpdateConcurrencyException)
            {
                response.Code = 0;
                if (!PaymentMilestoneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(response);
        }

        // POST: api/PaymentMilestones
        [ResponseType(typeof(PaymentMilestone))]
        public IHttpActionResult PostPaymentMilestone(PaymentMilestone paymentMilestone)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    response.Code = 0;
                    return Ok(response);
                }

                db.PaymentMilestones.Add(paymentMilestone);

                if (paymentMilestone.PaymentMilestoneDetails != null && paymentMilestone.PaymentMilestoneDetails.Count > 0)
                {
                    List<TempTableForInstallment> tempTableForInstallments = new List<TempTableForInstallment>();
                    foreach (var item in paymentMilestone.PaymentMilestoneDetails.ToList())
                    {
                        for (int i = 1; i <= item.InstallmentNumber; i++)
                        {
                            TempTableForInstallment tempTableFor = new TempTableForInstallment();
                            tempTableFor.InstallmentType = item.Milestones;
                            tempTableFor.Frequency = item.Frequency;
                            tempTableFor.parentId = paymentMilestone.PaymentScheduleCode;
                            if (item.Milestones == "Installment")
                            {
                                tempTableFor.Installment = "Installment " + i.ToString();
                            }
                            else
                            {
                                tempTableFor.Installment = item.Milestones;
                            }
                            tempTableForInstallments.Add(tempTableFor);
                            tempTableFor.Percentage = Convert.ToInt32( item.Percentage);
                        }
                    }

                    db.TempTableForInstallments.AddRange(tempTableForInstallments);
                }

                //var getProperty = db.Projects.Where(x => x.Id == paymentMilestone.projectId).FirstOrDefault();
                //if (getProperty != null)
                //{
                    //getProperty.PaymentPlanStatus = true;
                //}
                db.SaveChanges();
                response.Code = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Code = 0;
            }
            return Ok(paymentMilestone);
        }

        // DELETE: api/PaymentMilestones/5
        [ResponseType(typeof(PaymentMilestone))]
        public IHttpActionResult DeletePaymentMilestone(int id)
        {
            PaymentMilestone paymentMilestone = db.PaymentMilestones.Find(id);
            if (paymentMilestone == null)
            {
                return NotFound();
            }

            db.PaymentMilestones.Remove(paymentMilestone);
            db.SaveChanges();

            return Ok(paymentMilestone);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentMilestoneExists(int id)
        {
            return db.PaymentMilestones.Count(e => e.Id == id) > 0;
        }

       
    }
}