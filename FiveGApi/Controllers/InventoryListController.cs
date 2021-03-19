using System;
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
using System.Security.Claims;
using FiveGApi.Helper;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/InventoryList")]
    public class InventoryListController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        private string UserId;
        private User userSecurityGroup = new User();
        public InventoryListController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;
            userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().FirstOrDefault();

        }
        // GET: api/Inventory_List
        [Route("GetAllInventory_List")]
        [ResponseType(typeof(IQueryable<Inventory_List>))]
        public IQueryable<Inventory_List> GetAllInventory_List(int RDID)
        {
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                return db.Inventory_List.Where(x => x.SecurityGroupId == userSecurityGroup.SecurityGroupId);
            else
                return db.Inventory_List;
            //return db.Inventory_List;
        }

        // GET: api/Inventory_List/5
        [Route("GetInventory_ByID")]
        [ResponseType(typeof(Inventory_List))]
        public IHttpActionResult GetInventory_ByID(int id)
        {
            Inventory_List Inventory_List = new Inventory_List();  //db.Inventory_List.Find(id);
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                Inventory_List= db.Inventory_List.Where(x =>x.ID==id && x.SecurityGroupId == userSecurityGroup.SecurityGroupId).FirstOrDefault();
            else
                Inventory_List= db.Inventory_List.Find(id);
            if (Inventory_List == null)
            {
                return NotFound();
            }

            return Ok(Inventory_List);
        }

        // PUT: api/Inventory_List/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInventory_List(int id, Inventory_List Inventory_List)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Inventory_List.ID)
            {
                return BadRequest();
            }
            Inventory_List.Updated_By = userSecurityGroup.UserName;
            Inventory_List.Updated_On = DateTime.Now;
            Inventory_List.SecurityGroupId = userSecurityGroup.SecurityGroupId;
            db.Entry(Inventory_List).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Inventory_ListExists(id))
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

        // POST: api/Inventory_List
        [ResponseType(typeof(Inventory_List))]
        public IHttpActionResult PostInventory_List(Inventory_List Inventory_List)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Inventory_List.Created_By = userSecurityGroup.UserName;
            Inventory_List.SecurityGroupId = userSecurityGroup.SecurityGroupId;
            Inventory_List.Created_On = DateTime.Now;
            db.Inventory_List.Add(Inventory_List);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Inventory_List.ID }, Inventory_List);
        }

        // DELETE: api/Inventory_List/5
        [ResponseType(typeof(Inventory_List))]
        public IHttpActionResult DeleteInventory_List(int id)
        {
            Inventory_List Inventory_List = db.Inventory_List.Find(id);
            if (Inventory_List == null)
            {
                return NotFound();
            }

            db.Inventory_List.Remove(Inventory_List);
            db.SaveChanges();

            return Ok(Inventory_List);
        }
        // DELETE: api/Inventory_List/5
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Inventory_ListExists(int id)
        {
            return db.Inventory_List.Count(e => e.ID == id) > 0;
        }
    }
}

