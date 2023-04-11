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
    public class VolunteersDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/VolunteersData/ListVolunteers
        [ResponseType(typeof(VolunteersDto))]
        [HttpGet]
        public IHttpActionResult ListVolunteers()
        {
            List<Volunteers> Volunteers = db.Volunteers.ToList();
            List<VolunteersDto> VolunteersDtos = new List<VolunteersDto>();

            Volunteers.ForEach(v => VolunteersDtos.Add(new VolunteersDto()
            {
                volunteer_id = v.volunteer_id,
                f_name = v.f_name,
                l_name = v.l_name,
                contact = v.contact,
                email = v.email,
                program_id = v.program_id,
                hours = v.hours

            }));
      
            return Ok(VolunteersDtos);
        }

        [HttpGet]
        [ResponseType(typeof(VolunteersDto))]
        public IHttpActionResult ListVolunteersForProgram(int id)
        {
            List<Volunteers> Volunteers = db.Volunteers.Where(v => v.program_id == id).ToList();
            List<VolunteersDto> VolunteersDtos = new List<VolunteersDto>();

            Volunteers.ForEach(v => VolunteersDtos.Add(new VolunteersDto()
            {
                volunteer_id = v.volunteer_id,
                f_name = v.f_name,
                l_name = v.l_name,
                contact = v.contact,
                email = v.email,
                hours = v.hours

            }));
            return Ok(VolunteersDtos);
        }

        // GET: api/VolunteersData/FindVolunteers/5
        [ResponseType(typeof(Volunteers))]
        [HttpGet]
        public IHttpActionResult FindVolunteers(int id)
        {
            Volunteers Volunteers = db.Volunteers.Find(id);
            VolunteersDto VolunteersDto = new VolunteersDto()
            {
                volunteer_id = Volunteers.volunteer_id,
                f_name = Volunteers.f_name,
                l_name = Volunteers.l_name,
                contact = Volunteers.contact,
                email = Volunteers.email,
                program_id = Volunteers.program_id,
                hours = Volunteers.hours
            };

            if (Volunteers == null)
            {
                return NotFound();
            }

            return Ok(VolunteersDto);
        }

        // POST: api/VolunteersData/UpdateVolunteers/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateVolunteers(int id, Volunteers Volunteers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Volunteers.volunteer_id)
            {
                return BadRequest();
            }

            db.Entry(Volunteers).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteersExists(id))
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

        // POST: api/VolunteersData/AddVolunteers
        [ResponseType(typeof(Volunteers))]
        [HttpPost]
        public IHttpActionResult AddVolunteers(Volunteers Volunteers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Volunteers.Add(Volunteers);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Volunteers.volunteer_id }, Volunteers);
        }

        // POST: api/VolunteersData/DeleteVolunteers/5
        [ResponseType(typeof(Volunteers))]
        [HttpPost]
        public IHttpActionResult DeleteVolunteers(int id)
        {
            Volunteers Volunteers = db.Volunteers.Find(id);
            if (Volunteers == null)
            {
                return NotFound();
            }

            db.Volunteers.Remove(Volunteers);
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

        private bool VolunteersExists(int id)
        {
            return db.Volunteers.Count(e => e.volunteer_id == id) > 0;
        }
    }
}