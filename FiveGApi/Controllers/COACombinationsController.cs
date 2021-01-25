using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    [Authorize]

    [RoutePrefix("api/COACombinations")]
    public class COACombinationsController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        // POST: api/BookingConfirm
        [Route("PostCOA_Combinations")]
        [ResponseType(typeof(COA_Combinations))]
        [HttpPost]
        public IHttpActionResult PostCOA_Combinations(string CCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ExistedCOA_Combinations = db.COA_Combinations.Where(x => x.C_Code == CCode).FirstOrDefault();
            if (ExistedCOA_Combinations != null)
            {
                return Ok(ExistedCOA_Combinations.C_ID);
            }else
            {
                string[] codeParts = CCode.Split('.');
                COA_Combinations cOA_Combinations = new COA_Combinations();
                cOA_Combinations.C_Code = CCode;
                cOA_Combinations.Company = codeParts[0];
                cOA_Combinations.Project = codeParts[1];
                cOA_Combinations.Location = codeParts[2];
                cOA_Combinations.Account = codeParts[3];
                cOA_Combinations.Party = codeParts[4];
                cOA_Combinations.Created_By = "1";
                cOA_Combinations.Created_ON = DateTime.Now;
                db.COA_Combinations.Add(cOA_Combinations);
                db.SaveChanges();
                return Ok(cOA_Combinations.C_ID);
            }

                  
        }
        [Route("GetCOA_CombinationID")]
        [ResponseType(typeof(COA_Combinations))]
        [HttpGet]
        public IHttpActionResult GetCOA_CombinationID(int CoID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ExistedCOA_Combinations = db.COA_Combinations.Where(x => x.C_ID == CoID).FirstOrDefault();
            if (ExistedCOA_Combinations != null)
            {
                return Ok(ExistedCOA_Combinations.C_Code);
            }
            else
            {                 
                return NotFound();
            }


        }
    }
}
