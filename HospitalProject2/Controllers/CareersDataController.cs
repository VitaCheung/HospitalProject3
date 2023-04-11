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
using System.Diagnostics;
using HospitalProject2.Migrations;

namespace HospitalProject2.Controllers
{
    public class CareersDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        // GET: api/CareersData/ListCareers
        [HttpGet]
        [ResponseType(typeof(CareersDto))]
        public IHttpActionResult ListCareers()
        {
            List<Careers> Careers = db.Careers.ToList();
            List<CareersDto> CareerDtos = new List<CareersDto>();

            Careers.ForEach(c => CareerDtos.Add(new CareersDto()
            {
                job_id = c.job_id,
                title = c.title,
                department_id = c.department_id,
                category = c.category,
                job_type = c.job_type,
                posting_date = c.posting_date,
                closing_date = c.closing_date

            }));

            return Ok(CareerDtos);
        }

        [HttpGet]
        [ResponseType(typeof(CareersDto))]
        public IHttpActionResult ListCareersForDepartment(int id)
        {
            List<Careers> Careers = db.Careers.Where(c => c.department_id == id).ToList();
            List<CareersDto> CareersDtos = new List<CareersDto>();

            Careers.ForEach(c => CareersDtos.Add(new CareersDto()
            {
                job_id = c.job_id,
                title = c.title,
                category = c.category,
                job_type = c.job_type,
                posting_date = c.posting_date,
                closing_date = c.closing_date

            }));
            return Ok(CareersDtos);
        }

        /// GET: api/CareersData/FindCareers/5
        [HttpGet]
        [ResponseType(typeof(CareersDto))]
        public IHttpActionResult FindCareers(int id)
        {
            Careers Careers = db.Careers.Find(id);
            CareersDto CareersDto = new CareersDto()
            {
                job_id = Careers.job_id,
                title = Careers.title,
                department_id = Careers.department_id,
                category = Careers.category,
                job_type = Careers.job_type,
                posting_date = Careers.posting_date,
                closing_date = Careers.closing_date
            };
            if (Careers == null)
            {
                return NotFound();
            }

            return Ok(CareersDto);
        }

        // POST: api/CareersData/UpdateCareers/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCareers(int id, Careers Careers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Careers.job_id)
            {
                return BadRequest();
            }

            db.Entry(Careers).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CareersExists(id))
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

        // POST: api/CareersData/AddCareers
        [ResponseType(typeof(Careers))]
        [HttpPost]
        public IHttpActionResult AddCareers(Careers Careers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Careers.Add(Careers);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Careers.job_id }, Careers);
        }

        // POST: api/CareersData/DeleteCareers/5
        [ResponseType(typeof(Careers))]
        [HttpPost]
        public IHttpActionResult DeleteCareers(int id)
        {
            Careers Careers = db.Careers.Find(id);
            if (Careers == null)
            {
                return NotFound();
            }

            db.Careers.Remove(Careers);
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

        private bool CareersExists(int id)
        {
            return db.Careers.Count(e => e.job_id == id) > 0;
        }
    }
}