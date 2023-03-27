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

namespace HospitalProject2.Controllers
{
    public class DepartmentsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DepartmentsData/ListDepartments
        [HttpGet]
        public IEnumerable<DepartmentsDto> ListDepartments()
        {
            List<Departments> Departments = db.Departments.ToList();
            List<DepartmentsDto> DepartmentsDtos = new List<DepartmentsDto>();

            Departments.ForEach(a => DepartmentsDtos.Add(new DepartmentsDto()
            {
               department_id = a.department_id,
               name = a.name,
               location = a.location,
               size = a.size
            }));

            return DepartmentsDtos;
        }

        // GET: api/DepartmentsData/FindDepartment/5
        [ResponseType(typeof(Departments))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Departments Departments = db.Departments.Find(id);
            DepartmentsDto DepartmentsDto = new DepartmentsDto()
            {
                department_id = Departments.department_id,
                name = Departments.name,
                location = Departments.location,
                size = Departments.size
            };
            if (Departments == null)
            {
                return NotFound();
            }

            return Ok(DepartmentsDto);
        }

        // UPDATE: api/DepartmentsData/UpdateDepartment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, Departments departments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != departments.department_id)
            {
                return BadRequest();
            }

            db.Entry(departments).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentsExists(id))
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

        // ADD: api/DepartmentsData/AddDepartment
        [ResponseType(typeof(Departments))]
        [HttpPost]
        public IHttpActionResult AddDepartment(Departments departments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(departments);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = departments.department_id }, departments);
        }

        // DELETE: api/DepartmentsData/DeleteDepartments/5
        [ResponseType(typeof(Departments))]
        [HttpPost]
        public IHttpActionResult DeleteDepartments(int id)
        {
            Departments departments = db.Departments.Find(id);
            if (departments == null)
            {
                return NotFound();
            }

            db.Departments.Remove(departments);
            db.SaveChanges();

            return Ok(departments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentsExists(int id)
        {
            return db.Departments.Count(e => e.department_id == id) > 0;
        }
    }
}