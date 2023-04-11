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
            List<FAQ> FAQs = db.FAQs.Include(x => x.Departments).ToList();
            List<FAQDto> FAQDtos = new List<FAQDto>();

            fAQs.ForEach(a => fAQDtos.Add(new FAQDto()
            {
                FAQ_Id = a.FAQ_Id,
                department_id = a.department_id,
                Answer = a.Answer,
                Question = a.Question,
                Departments = new DepartmentsDto() { department_id = a.Departments.department_id, name = a.Departments.name }
            }));
            return Ok(fAQDtos);
        }

        [ResponseType(typeof(FAQ))]
        [HttpGet]
        public IHttpActionResult FindFAQ(int id)
        {
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return NotFound();
            }

            FAQDto FAQDto = new FAQDto()
            {
                FAQ_Id = FAQ.FAQ_Id,
                department_id = FAQ.department_id,
                Answer = FAQ.Answer,
                Question = FAQ.Question,
                Departments = new DepartmentsDto() { name = fAQ.Departments.name }
            };

            return Ok(fAQDto);
        }


        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult EditFAQ(int id, FAQ fAQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != FAQ.FAQ_Id)
            {
                return BadRequest();
            }

            db.Entry(fAQ).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FAQExists(id))
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

        [ResponseType(typeof(FAQ))]
        [HttpPost]
        public IHttpActionResult CreateFAQ(FAQ fAQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FAQs.Add(FAQ);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = FAQ.FAQ_Id }, FAQ);
        }



        [ResponseType(typeof(FAQ))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteFAQ(int id)
        {
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return NotFound();
            }

            db.FAQs.Remove(fAQ);
            db.SaveChanges();

            return Ok(fAQ);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FAQExists(int id)
        {
            return db.FAQs.Count(e => e.FAQ_Id == id) > 0;
        }


    }