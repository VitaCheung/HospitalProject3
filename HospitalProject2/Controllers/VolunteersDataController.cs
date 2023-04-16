using System;
using System.IO;
using System.Web;
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

using Microsoft.AspNet.Identity;


namespace HospitalProject2.Controllers
{
    public class VolunteersDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/VolunteersData/ListVolunteers
        [ResponseType(typeof(VolunteersDto))]
        [HttpGet]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult ListVolunteers()
        {
            bool isAdmin = User.IsInRole("Admin");
            //Admins see all, guests only see their own
            List<Volunteers> Volunteers;
            Debug.WriteLine("id is " + User.Identity.GetUserId());
            if (isAdmin) Volunteers = db.Volunteers.ToList();
            else
            {
                string UserId = User.Identity.GetUserId();
                Volunteers = db.Volunteers.Where(v => v.UserID == UserId).ToList();
            }

            //List<Volunteers> Volunteers = db.Volunteers.ToList();

            List<VolunteersDto> VolunteersDtos = new List<VolunteersDto>();

            Volunteers.ForEach(v => VolunteersDtos.Add(new VolunteersDto()
            {
                volunteer_id = v.volunteer_id,
                f_name = v.f_name,
                l_name = v.l_name,
                contact = v.contact,
                email = v.email,
                program_id = v.program_id,
                hours = v.hours,
                UserId = v.UserID

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
                hours = v.hours,
                UserId= v.UserID

            }));
            return Ok(VolunteersDtos);
        }

        // GET: api/VolunteersData/FindVolunteers/5
        [ResponseType(typeof(Volunteers))]
        [HttpGet]
        [Authorize(Roles = "Admin,Guest")]
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
                hours = Volunteers.hours,
                UserId = Volunteers.UserID
            };

            if (Volunteers == null)
            {
                return NotFound();
            }
            //do not process if the (user is not an admin) and (the booking does not belong to the user)
            bool isAdmin = User.IsInRole("Admin");
            //Forbidden() isn't a natively implemented status like BadRequest()
            if (!isAdmin && (Volunteers.UserID != User.Identity.GetUserId())) return StatusCode(HttpStatusCode.Forbidden);

            return Ok(VolunteersDto);
        }

        // POST: api/VolunteersData/UpdateVolunteers/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
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

            //do not process if the (user is not an admin) and (the booking does not belong to the user)
            bool isAdmin = User.IsInRole("Admin");
            //Forbidden() isn't a natively implemented status like BadRequest()
            if (!isAdmin && (Volunteers.UserID != User.Identity.GetUserId()))
            {
                Debug.WriteLine("not allowed. booking user" + Volunteers.UserID + " user " + User.Identity.GetUserId());
                return StatusCode(HttpStatusCode.Forbidden);
            }

            db.Entry(Volunteers).State = EntityState.Modified;
            db.Entry(Volunteers).Property(v => v.UserID).IsModified = false;

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
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult AddVolunteers(Volunteers Volunteers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Volunteers.UserID = User.Identity.GetUserId();

            db.Volunteers.Add(Volunteers);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Volunteers.volunteer_id }, Volunteers);
        }

        // POST: api/VolunteersData/DeleteVolunteers/5
        [ResponseType(typeof(Volunteers))]
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult DeleteVolunteers(int id)
        {
            Volunteers Volunteers = db.Volunteers.Find(id);
            if (Volunteers == null)
            {
                return NotFound();
            }

            //do not process if the (user is not an admin) and (the booking does not belong to the user)
            bool isAdmin = User.IsInRole("Admin");
            //Forbidden() isn't a natively implemented status like BadRequest()
            if (!isAdmin && (Volunteers.UserID != User.Identity.GetUserId())) return StatusCode(HttpStatusCode.Forbidden);


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