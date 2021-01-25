using FiveGApi.DTOModels;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FiveGApi.Controllers
{
    [Authorize]

    [RoutePrefix("api/COASegments")]
    public class COASegmentsController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        // GET: api/COA_Segments
        [Route("GetAllCOA_Segments")]
        [ResponseType(typeof(IQueryable<COA_Segments>))]
        public IQueryable<COA_Segments> GetAllCOA_Segments()
        {
            return db.COA_Segments;
        }
        [Route("GetCOA_SegmentsBySegment")]
        [ResponseType(typeof(IList<COA_Segments>))]
        public IList<COA_Segments> GetCOA_SegmentsBySegment(string segment)
        {
            return db.COA_Segments.Where(s=>s.Segment==segment).ToList();
        }
        [Route("GetCOA_SegmentBySegment_Value")]
        [ResponseType(typeof(COA_Segments))]
        public COA_Segments GetCOA_SegmentBySegment_Value(string segment,string SegmentValue)
        {
            return db.COA_Segments.Where(s => s.Segment_Value == SegmentValue && s.Segment==segment).FirstOrDefault();
        }
        [Route("GetCOA_SegmentBySegment_IsUnique")]
        [ResponseType(typeof(COA_Segments))]
        public IHttpActionResult GetCOA_SegmentBySegment_IsUnique(string segment,string segmentVlaue)
        {
            var finding= db.COA_Segments.Where(s => s.Segment_Value == segmentVlaue && s.Segment==segment).FirstOrDefault();
            if (finding != null)
            {
                return Ok(finding.ID);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/COA_Segments/5
        [ResponseType(typeof(COA_Segments))]
        public IHttpActionResult GetCOA_Segments(int id)
        {
            COA_Segments Rebate_Detail = db.COA_Segments.Find(id);
            if (Rebate_Detail == null)
            {
                return NotFound();
            }

            return Ok(Rebate_Detail);
        }

        // PUT: api/COA_Segments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCOA_Segments(int id, COA_Segments COA_Segments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != COA_Segments.ID)
            {
                return BadRequest();
            }

            db.Entry(COA_Segments).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!COA_SegmentsExists(id))
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

        // POST: api/COA_Segments
        [ResponseType(typeof(COA_Segments))]
        public IHttpActionResult PostCOA_Segments(COA_Segments COA_Segments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.COA_Segments.Add(COA_Segments);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = COA_Segments.ID }, COA_Segments);
        }


        // DELETE: api/COA_Segments/5
        [ResponseType(typeof(COA_Segments))]
        public IHttpActionResult DeleteCOA_Segments(int id)
        {
            COA_Segments COA_Segments = db.COA_Segments.Find(id);
            if (COA_Segments == null)
            {
                return NotFound();
            }

            db.COA_Segments.Remove(COA_Segments);
            db.SaveChanges();

            return Ok(COA_Segments);
        }
        [Route("SegmentTypeEnum")]
        [HttpGet]
        [ResponseType(typeof(List<ResultViewModel>))]
        public IHttpActionResult SegmentTypeEnum()
        {
            List<ResultViewModel> SegmentType = new List<ResultViewModel>();
            foreach (var item in Enum.GetValues(typeof(COA_Segments_Enums.SegmentType)))
            {
                ResultViewModel responseModel = new ResultViewModel();
                responseModel.ID = (int)item;
                responseModel.Name = item.ToString();
                responseModel.isSelected = false;
                SegmentType.Add(responseModel);
            }
            return Ok(SegmentType);
        }
        [Route("TypeEnum")]
        [HttpGet]
        [ResponseType(typeof(List<ResultViewModel>))]
        public IHttpActionResult TypeEnum()
        {
            List<ResultViewModel> Type = new List<ResultViewModel>();
            foreach (var item in Enum.GetValues(typeof(COA_Segments_Enums.type)))
            {
                ResultViewModel responseModel = new ResultViewModel();
                responseModel.ID = (int)item;
                responseModel.Name = item.ToString();
                responseModel.isSelected = false;
                Type.Add(responseModel);
            }
            return Ok(Type);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool COA_SegmentsExists(int id)
        {
            return db.COA_Segments.Count(e => e.ID == id) > 0;
        }
    }
}
