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

        
        /// <summary>
        /// Returns all FAQsin the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all FAQs in the database
        /// </returns>
        /// <example>
        // GET: api/FAQsData/ListFAQs
        /// </example>

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
        
        
        /// <summary>
        /// Edit a particular FAQ in the system with POST Data input
        /// </summary>
        /// <param name="id">FAQ primary key</param>
        /// <param name="FAQ">JSON form data of FAQ</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // PUT: api/FAQsData/EditFAQs/5
        /// FORM DATA: FAQ JSON Object
        /// </example>


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
        
        /// <summary>
        /// Add FAQ to the system
        /// </summary>
        /// <param name="FAQ">JSON form data of FAQ</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:FAQ ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/FAQsData/CreateFAQ
        /// </example>
        

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
        
        /// <summary>
        /// Deletes FAQ from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of FAQ</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/FAQsData/DeleteFAQ/5
        /// FORM DATA: (empty)
        /// </example>


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
