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

        /// <summary>
        /// Returns all Careers in the system
        /// </summary>
        /// <returns>
        /// Content: all Careers information in the database, including the related department names
        /// </returns>
        /// <example> GET: api/CareersData/ListCareers </example>       
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
                name = c.Departments.name,
                category = c.category,
                job_type = c.job_type,
                posting_date = c.posting_date,
                closing_date = c.closing_date

            }));

            return Ok(CareerDtos);
        }

        /// <summary>
        /// Returns all Careers in the system related to a particular department
        /// </summary>
        /// <param name="id">Department Primary key</param>
        /// <returns> CONTENT: all Careers in the database under a particular department</returns>
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

        /// <summary>
        /// Find specific career in the system
        /// </summary>
        /// <param name="id">The primary key of the Careers</param>
        /// <returns>CONTENT: An careers in the system matching up to the job_id primary key</returns>
        /// <example>GET: api/CareersData/FindCareers/5</example>
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
                name = Careers.Departments.name,
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

        /// <summary>
        /// Updates a particular Career in the system with POST data input
        /// </summary>
        /// <param name="id">The job_id which represents the career</param>
        /// <param name="Careers">JSON form data of Careers</param>
        /// <returns></returns>
        /// <example> POST: api/CareersData/UpdateCareers/5 </example> 
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Add a career to the system
        /// </summary>
        /// <param name="Careers">JSON form data of Careers</param>
        /// <returns>CONTENT: job_id, Career data</returns>
        /// <example>POST: api/CareersData/AddCareers</example> 
        [ResponseType(typeof(Careers))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Delete a career from the system by its id
        /// </summary>
        /// <param name="id">The primary key of the Careers</param>
        /// <returns></returns>
        /// <example>POST: api/CareersData/DeleteCareers/5</example> 
        [ResponseType(typeof(Careers))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
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