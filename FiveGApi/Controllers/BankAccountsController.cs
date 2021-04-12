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
   
    [RoutePrefix("api/BankAccounts")]
    [Authorize]
    public class BankAccountsController : ApiController
    {
        private string UserId;
        private User userSecurityGroup=new User();
        public BankAccountsController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;
            userSecurityGroup = db.Users.Where(x => x.UserName == UserId).AsQueryable().FirstOrDefault();

        }
        private MIS_DBEntities1 db = new MIS_DBEntities1();
            // GET: api/Bank_Accounts
            [ResponseType(typeof(IQueryable<Bank_Accounts>))]
            [Route("GetALLBank_Accounts")]
            [HttpGet]
            public IQueryable<Bank_Accounts> GetALLBank_Accounts()
            {
                  if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                      return db.Bank_Accounts.Where(x => x.SecurityGroupId == userSecurityGroup.SecurityGroupId && x.Status== "Active").AsQueryable();
                  else
                      return db.Bank_Accounts.Where(x=>x.Status== "Active").AsQueryable();
            }
            [ResponseType(typeof(IQueryable<Bank_Accounts>))]
            [Route("GetALLBank_AccountsWithoutFilter")]
            [HttpGet]
            public IQueryable<Bank_Accounts> GetALLBank_AccountsWithoutFilter()
            {
                if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                    return db.Bank_Accounts.Where(x => x.SecurityGroupId == userSecurityGroup.SecurityGroupId).AsQueryable();
                else
                    return db.Bank_Accounts;
            }
        // GET: api/Bank_Accounts/5
        [ResponseType(typeof(Bank_Accounts))]
            public IHttpActionResult GetBank_AccountsByID(int id)
            {
            Bank_Accounts Bank_Accounts = new Bank_Accounts();
            if (!SecurityGroupDTO.CheckSuperAdmin((int)userSecurityGroup.SecurityGroupId))
                Bank_Accounts = db.Bank_Accounts.Where(x=>x.ID==id&& x.SecurityGroupId==userSecurityGroup.SecurityGroupId).FirstOrDefault();
            else
                Bank_Accounts = db.Bank_Accounts.Where(x => x.ID == id).FirstOrDefault();

            if (Bank_Accounts == null)
                {
                    return NotFound();
                }

                return Ok(Bank_Accounts);
            }

            // PUT: api/Bank_Accounts/5
            [ResponseType(typeof(void))]
            public IHttpActionResult PutBank_Accounts(int id, Bank_Accounts Bank_Accounts)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existBank_Accounts = db.Bank_Accounts.Where(x => x.ID == id).FirstOrDefault();
                existBank_Accounts.Acc_Name =Bank_Accounts.Acc_Name;
                existBank_Accounts.Acc_Number= Bank_Accounts.Acc_Number;
                existBank_Accounts.Acc_Title= Bank_Accounts.Acc_Title;
                existBank_Accounts.IBAN= Bank_Accounts.IBAN;
                existBank_Accounts.Acc_Currency = Bank_Accounts.Acc_Currency;
                existBank_Accounts.Bank  = Bank_Accounts.Bank;
                existBank_Accounts.Branch = Bank_Accounts.Branch;
                existBank_Accounts.Swift_Code   = Bank_Accounts.Swift_Code;
                existBank_Accounts.Type = Bank_Accounts.Type;
                existBank_Accounts.Status= Bank_Accounts.Status;
                existBank_Accounts.GL_Mapping = Bank_Accounts.GL_Mapping;
                existBank_Accounts.Flex_1= Bank_Accounts.Flex_1;
                existBank_Accounts.Flex_2 = Bank_Accounts.Flex_2;
                existBank_Accounts.Updated_By= userSecurityGroup.UserName;
                existBank_Accounts.Updated_On = DateTime.Now;
                existBank_Accounts.SecurityGroupId = userSecurityGroup.SecurityGroupId;
                existBank_Accounts.PaymentType = Bank_Accounts.PaymentType;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Bank_AccountsExists((int)Bank_Accounts.ID))
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
        // POST: api/Bank_Accounts
        [ResponseType(typeof(Bank_Accounts))]
        public IHttpActionResult PostBank_Accounts(Bank_Accounts Bank_Accounts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Bank_Accounts.Created_By = userSecurityGroup.UserName;
            Bank_Accounts.Created_On = DateTime.Now;
            Bank_Accounts.SecurityGroupId = userSecurityGroup.SecurityGroupId;
            var bankDublicate = db.Bank_Accounts.Where(s => s.Acc_Name == Bank_Accounts.Acc_Name).FirstOrDefault();
            if (bankDublicate != null)
            {
                var error = new { message = "Bank Account must be unique" }; //<-- anonymous object
                return this.Content(HttpStatusCode.Conflict, error);
            }
            else
            {
                db.Bank_Accounts.Add(Bank_Accounts);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = Bank_Accounts.ID }, Bank_Accounts);
            }
        }


            // DELETE: api/Bank_Accounts/5
            [ResponseType(typeof(Bank_Accounts))]
            public IHttpActionResult DeleteBank_Accounts(int id)
            {
                Bank_Accounts Bank_Accounts = db.Bank_Accounts.Find(id);
                if (Bank_Accounts == null)
                {
                    return NotFound();
                }           
                db.Bank_Accounts.Remove(Bank_Accounts);
                db.SaveChanges();

                return Ok(Bank_Accounts);
            
        }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool Bank_AccountsExists(int id)
            {
                return db.Bank_Accounts.Count(e => e.ID == id) > 0;
            }
        }
    }