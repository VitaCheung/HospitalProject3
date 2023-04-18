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

        /// <summary>
        /// Returns all Volunteers in the system for the Admin, return the own volunteer record for the guest
        /// </summary>
        /// <returns>
        /// Content: all Volunteers information in the database, including the related program names
        /// </returns>
        /// <example> GET: api/VolunteersData/ListVolunteers </example>   
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
                UserId = v.UserID,
                name = v.Programs.name

            }));
      
            return Ok(VolunteersDtos);
        }

        /// <summary>
        /// Returns all Volunteers in the system related to a particular program
        /// </summary>
        /// <param name="id">Program Primary key</param>
        /// <returns>CONTENT: all volunteers in the database under a particular program</returns>
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

        /// <summary>
        /// Find specific volunteer in the system
        /// </summary>
        /// <param name="id">The primary key of the Volunteers</param>
        /// <returns>CONTENT: A volunteer in the system matching up to the volunteer_id primary key</returns>
        /// <example> GET: api/VolunteersData/FindVolunteers/5 </example>
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
                UserId = Volunteers.UserID,
                name = Volunteers.Programs.name

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

        /// <summary>
        /// Updates a particular Volunteer in the system with POST data input
        /// </summary>
        /// <param name="id">The volunteer_id which represents the volunteer</param>
        /// <param name="Volunteers">JSON form data of Volunteers</param>
        /// <returns></returns>
        /// <example> POST: api/VolunteersData/UpdateVolunteers/5 </example> 
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

        /// <summary>
        /// Add a volunteer to the system
        /// </summary>
        /// <param name="Volunteers">JSON form data of Volunteers</param>
        /// <returns>CONTENT: volunteer_id, Volunteer data</returns>
        /// <example> POST: api/VolunteersData/AddVolunteers </example> 
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

        /// <summary>
        /// Delete a volunteer from the system by its id
        /// </summary>
        /// <param name="id">The primary key of the Volunteers</param>
        /// <returns></returns>
        /// <example> POST: api/VolunteersData/DeleteVolunteers/5 </example>
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