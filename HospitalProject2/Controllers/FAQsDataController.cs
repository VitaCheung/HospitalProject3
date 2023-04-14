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
using HospitalProject2.Models;
using HospitalProject2.Models.ViewModels;
using System.Web.Script.Serialization;
using HospitalProject2.Migrations;

namespace HospitalProject2.Controllers
{
    public class FAQsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [HttpGet]
        public IHttpActionResult ListFAQs()
        {
            List<FAQs> FAQs = db.FAQs.Include(x => x.Departments).ToList();
            List<FAQsDto> FAQsDtos = new List<FAQsDto>();

            FAQs.ForEach(a => FAQsDtos.Add(new FAQsDto()
            {
                FAQ_id = a.FAQ_id,
                department_id = a.department_id,
                question = a.question,
                answer = a.answer
            }));
            return Ok(FAQsDtos);
        }

        [ResponseType(typeof(FAQs))]
        [HttpGet]
        public IHttpActionResult FindFAQs(int id)
        {
            FAQs FAQs = db.FAQs.Find(id);
            FAQsDto FAQsDto = new FAQsDto()
            {
                FAQ_id = FAQs.FAQ_id,
                department_id = FAQs.department_id,
                answer = FAQs.answer,
                question = FAQs.question
            };
            if (FAQs == null)
            {
                return NotFound();
            }


            return Ok(FAQsDto);
        }


        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult EditFAQ(int id, FAQs FAQs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != FAQs.FAQ_id)
            {
                return BadRequest();
            }

            db.Entry(FAQs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FAQsExists(id))
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

        [ResponseType(typeof(FAQs))]
        [HttpPost]
        public IHttpActionResult CreateFAQ(FAQs FAQs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FAQs.Add(FAQs);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = FAQs.FAQ_id }, FAQs);
        }



        [ResponseType(typeof(FAQs))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteFAQ(int id)
        {
            FAQs FAQs = db.FAQs.Find(id);
            if (FAQs == null)
            {
                return NotFound();
            }

            db.FAQs.Remove(FAQs);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FAQsExists(int id)
        {
            return db.FAQs.Count(e => e.FAQ_id == id) > 0;
        }


    }
}