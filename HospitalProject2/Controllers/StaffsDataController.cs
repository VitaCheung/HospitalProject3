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

namespace HospitalProject2.Controllers
{
    public class StaffsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // LIST: api/StaffsData/ListStaffs
        [HttpGet]
        public IEnumerable<StaffsDto> ListStaffs()
        {
            List<Staffs> Staffs = db.Staffs.ToList();
            List<StaffsDto> StaffsDtos = new List<StaffsDto>();

            Staffs.ForEach(a => StaffsDtos.Add(new StaffsDto()
            {
                staff_id = a.staff_id,
                f_name = a.f_name,
                l_name = a.l_name,
                department_id = a.department_id,
                bio = a.bio,
                image = a.image
            }));

            return StaffsDtos;

        }

        // GET: api/StaffsData/FindStaff/5
        [ResponseType(typeof(Staffs))]
        [HttpGet]
        public IHttpActionResult FindStaff(int id)
        {
            Staffs Staffs = db.Staffs.Find(id);
            StaffsDto StaffsDto = new StaffsDto()
            {
                staff_id = Staffs.staff_id,
                f_name = Staffs.f_name,
                l_name = Staffs.l_name,
                department_id = Staffs.department_id,
                bio = Staffs.bio,
                image = Staffs.image
            };
            if (Staffs == null)
            {
                return NotFound();
            }

            return Ok(StaffsDto);
        }

        // UPDATE: api/StaffsData/UpdateStaff/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateStaff(int id, Staffs staffs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffs.staff_id)
            {
                return BadRequest();
            }

            db.Entry(staffs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffsExists(id))
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

        // ADD: api/StaffsData/AddStaff
        [ResponseType(typeof(Staffs))]
        [HttpPost]
        public IHttpActionResult AddStaff(Staffs staffs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Staffs.Add(staffs);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = staffs.staff_id }, staffs);
        }

        // DELETE: api/StaffsData/DeleteAnimal/5
        [ResponseType(typeof(Staffs))]
        [HttpPost]
        public IHttpActionResult DeleteStaffs(int id)
        {
            Staffs staffs = db.Staffs.Find(id);
            if (staffs == null)
            {
                return NotFound();
            }

            db.Staffs.Remove(staffs);
            db.SaveChanges();

            return Ok(staffs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StaffsExists(int id)
        {
            return db.Staffs.Count(e => e.staff_id == id) > 0;
        }
    }
}