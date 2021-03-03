﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using FiveGApi.DTOModels;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/PropertyTransfer")]
    public class PropertyTransferController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/PropertyTransfer

        [ResponseType(typeof(IQueryable<PropertyTransfer>))]
        public IQueryable<PropertyTransfer> GetAllPropertyTransfer(int RDID)
        {
            return db.PropertyTransfers;
        }
        [Route("GetPropertyTransferByID")]
        // GET: api/PropertyTransfer/5
        [ResponseType(typeof(PropertyTransfer))]
        public IHttpActionResult GetPropertyTransferByID(int id)
        {
            PropertyTransfer PropertyTransfer = db.PropertyTransfers.Find(id);
            if (PropertyTransfer == null)
            {
                return NotFound();
            }

            return Ok(PropertyTransfer);
        }

        // PUT: api/PropertyTransfer/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPropertyTransfer(int id, PropertyTransfer PropertyTransfer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != PropertyTransfer.ID)
            {
                return BadRequest();
            }

            db.Entry(PropertyTransfer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyTransferExists(id))
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

        // POST: api/PropertyTransfer
        [ResponseType(typeof(PropertyTransfer))]
        public IHttpActionResult PostPropertyTransfer(PropertyTransfer PropertyTransfer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //PropertyTransfer propertyTransferDM = new PropertyTransfer();
            //propertyTransferDM.Booking_ID = PropertyTransfer.Booking_ID;
            //propertyTransferDM.Project_ID= PropertyTransfer.Project_ID;
            //propertyTransferDM.Unit_ID = PropertyTransfer.Unit_ID;
            //propertyTransferDM.Buyer_Name= PropertyTransfer.Buyer_Name;
            //propertyTransferDM.Buyer_FatherName= PropertyTransfer.Buyer_FatherName;
            //propertyTransferDM.BuyerAddress = PropertyTransfer.BuyerAddress;
            //propertyTransferDM.BuyerEmail= PropertyTransfer.BuyerEmail;
            //propertyTransferDM.BuyerCNIC = PropertyTransfer.BuyerCNIC;
            //propertyTransferDM.BuyerMobile_1 = PropertyTransfer.BuyerMobile_1;
            //propertyTransferDM.BuyerMobile_2= PropertyTransfer.BuyerMobile_2;
            //propertyTransferDM.BuyerMember_Reg_No = PropertyTransfer.BuyerMember_Reg_No;
            ////propertyTransferDM.Buyer_Picture = PropertyTransfer.Buyer_Picture;
            //propertyTransferDM.Seller_Name = PropertyTransfer.Seller_Name;
            //propertyTransferDM.Seller_FatherName = PropertyTransfer.Seller_FatherName;
            //propertyTransferDM.SellerAddress = PropertyTransfer.SellerAddress;
            //propertyTransferDM.SellerEmail = PropertyTransfer.SellerEmail;
            //propertyTransferDM.SellerCNIC= PropertyTransfer.SellerCNIC;
            //propertyTransferDM.SellerMobile_1 = PropertyTransfer.SellerMobile_1;
            //propertyTransferDM.SellerMobile_2= PropertyTransfer.SellerMobile_2;
            //propertyTransferDM.SellerMember_Reg_No = PropertyTransfer.SellerMember_Reg_No;
            //propertyTransferDM.Description = PropertyTransfer.Description;
            //propertyTransferDM.TransferDate = PropertyTransfer.TransferDate;
            //propertyTransferDM.TransferStatus = PropertyTransfer.TransferStatus;
            //propertyTransferDM.Flex_1 = PropertyTransfer.Flex_1;
            //propertyTransferDM.Flex_2 = PropertyTransfer.Flex_2;
            PropertyTransfer.Created_By = "Admin";
            PropertyTransfer.Created_On = DateTime.Now;
            //if (PropertyTransfer.Buyer_Picture != "")
            //{
            //    string[] image = PropertyTransfer.Buyer_Picture.Split(',');
            //    propertyTransferDM.Buyer_Picture = Convert.FromBase64String(image[1]);

            //}
            //if (PropertyTransfer.Seller_Picture != "")
            //{
            //    string[] image = PropertyTransfer.Seller_Picture.Split(',');
            //    propertyTransferDM.Seller_Picture = Convert.FromBase64String(image[1]);

            //}
            db.PropertyTransfers.Add(PropertyTransfer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = PropertyTransfer.ID }, PropertyTransfer);
        }

        // DELETE: api/PropertyTransfer/5
        [ResponseType(typeof(PropertyTransfer))]
        public IHttpActionResult DeletePropertyTransfer(int id)
        {
            PropertyTransfer PropertyTransfer = db.PropertyTransfers.Find(id);
            if (PropertyTransfer == null)
            {
                return NotFound();
            }

            db.PropertyTransfers.Remove(PropertyTransfer);
            db.SaveChanges();

            return Ok(PropertyTransfer);
        }
        [Route("SearchByProjectIdUnitCodeBuyerName")]
        [HttpGet]
        [ResponseType(typeof(FiveGApi.Models.PropertySale))]
        public IHttpActionResult SearchByProjectIdUnitCodeBuyerName(string projectCode,string unitCode,string buyerName, string nomineeName,string BuyerCnic)
        {
            var projectID = db.Projects.Where(x => x.projectCode == projectCode).Select(x=>x.Id).AsQueryable().FirstOrDefault();
            var unitId = db.ProjectDetails.Where(x => x.unitNumber == unitCode).Select(x => x.Id).AsQueryable().FirstOrDefault();
            var result = db.PropertySales.Where(x =>x.Project_ID==projectID && x.Unit_ID==unitId && x.Buyer_Name==buyerName && x.Nominee_Name==nomineeName&&x.CNIC==BuyerCnic).AsQueryable().FirstOrDefault();
            if (result == null)
            {

                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyTransferExists(int id)
        {
            return db.PropertyTransfers.Count(e => e.ID == id) > 0;
        }
    }
}

