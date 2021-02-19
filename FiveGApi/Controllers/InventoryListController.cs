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

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/InventoryList")]
    public class InventoryListController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/Inventory_List
        [Route("GetAllInventory_List")]
        [ResponseType(typeof(IQueryable<Inventory_List>))]
        public IQueryable<Inventory_List> GetAllInventory_List(int RDID)
        {
            return db.Inventory_List;
        }

        // GET: api/Inventory_List/5
        [Route("GetInventory_ByID")]
        [ResponseType(typeof(Inventory_List))]
        public IHttpActionResult GetInventory_ByID(int id)
        {
            Inventory_List Inventory_List = db.Inventory_List.Find(id);
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
            Inventory_List.Created_By = "Admin";
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

