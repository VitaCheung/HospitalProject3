﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using HospitalProject2.Models;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace HospitalProject2.Controllers
{
    public class AppointmentsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        
        /// <summary>
        /// List all information about appointments
        /// </summary>
        /// <example>
        /// localhost/api/AppointmentsData/ListAppointments -> appointment_id: 1, health_num: 123, date_time: 2023-01-01 9:00:00, symptoms: cough and fever, patient_id: 1, staff_id: 1
        /// </example>
        /// <returns>
        /// List all information about appointments
        /// </returns>

        // GET: api/AppointmentsData/ListAppointments
        [HttpGet]
        public IEnumerable<AppointmentsDto> ListAppointments()
        {
            List<Appointments> Appointments = db.Appointments.ToList();
            List<AppointmentsDto> AppointmentsDtos = new List<AppointmentsDto>();

            Appointments.ForEach(a => AppointmentsDtos.Add(new AppointmentsDto()
            {
                appointment_id = a.appointment_id,
                health_num = a.health_num,
                date_time = a.date_time,
                symptoms = a.symptoms,
                patient_id = a.patient_id,
                staff_id = a.staff_id

            }));
            return AppointmentsDtos;
        }
        
        /// <summary>
        /// Find artist based on their appointment_id and displays their information
        /// </summary>
        /// <example>
        /// localhost/api/AppointmentsData/FindAppointments/3 -> appointment_id: 1, health_num: 123, date_time: 2023-01-01 9:00:00, symptoms: cough and fever, patient_id: 1, staff_id: 1
        /// </example>
        /// <returns>
        /// Returns all information on appointments based on their appointment_id
        /// </returns>

        // GET: api/AppointmentsData/FindAppointments/5
        [HttpGet]
        [ResponseType(typeof(Appointments))]
        public IHttpActionResult FindAppointments(int id)
        {
            Appointments Appointments = db.Appointments.Find(id);
            AppointmentsDto AppointmentsDto = new AppointmentsDto()
            {
                appointment_id = Appointments.appointment_id,
                health_num = Appointments.health_num,
                date_time = Appointments.date_time,
                symptoms = Appointments.symptoms,
                patient_id = Appointments.patient_id,
                staff_id = Appointments.staff_id

            };
            if (Appointments == null)
            {
                return NotFound();
            }

            return Ok(AppointmentsDto);
        }
        
        
        // LIST Appointments for patients
        // GET: api/AppointmentsData/ListAppointmentsForPatients
        [HttpGet]
        [ResponseType(typeof(AppointmentsDto))]
        public IHttpActionResult ListAppointmentsForPatients(int id)
        {
            List<Appointments> Appointments = db.Appointments.Where(p => p.patient_id == id).ToList();
            List<AppointmentsDto> AppointmentsDtos = new List<AppointmentsDto>();

            Appointments.ForEach(p => AppointmentsDtos.Add(new AppointmentsDto()
            {
                appointment_id = p.appointment_id,
                health_num = p.health_num,
                date_time = p.date_time,
                symptoms = p.symptoms,
                patient_id = p.patient_id,
                staff_id = p.staff_id
            }));

            return Ok(AppointmentsDtos);
        }
        
        /// <summary>
        /// Update appointment information based on their appointment_id. 
        /// </summary>
        /// <example>
        /// using a curl request: curl -d @Appointment.json -H "Content-type:application/json" localhost/api/AppointmentsData/UpdateAppointments/5
        /// Before Update:
        /// appointment_id: 1, 
        /// health_num: 123, 
        /// date_time: 2023-01-01 9:00:00, 
        /// symptoms: cough and fever, 
        /// patient_id: 1, 
        /// staff_id: 1
        /// 
        /// After Update:
        /// appointment_id: 1, 
        /// health_num: 123, 
        /// date_time: 2023-01-02 10:00:00, 
        /// symptoms: cough and fever, 
        /// patient_id: 1, 
        /// staff_id: 1
        /// </example>
        /// <returns>
        /// Returns the updated information of appointments based on their appointment_id
        /// </returns>

        // POST: api/AppointmentsData/UpdateAppointment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointments(int id, Appointments appointments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointments.appointment_id)
            {
                return BadRequest();
            }

            db.Entry(appointments).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentsExists(id))
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
        /// Add an artist to the Appointments table 
        /// using a curl request: curl -d @Appointment.json -H "Content-type:application/json" localhost/api/AppointmentsData/AddAppointment/
        /// </summary>
        /// <returns>
        /// Adds appointment information to the Appointments table 
        /// </returns>

        // POST: api/AppointmentsData/AddAppointment
        [ResponseType(typeof(Appointments))]
        [HttpPost]
        public IHttpActionResult AddAppointments(Appointments appointments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointments);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointments.appointment_id }, appointments);
        }
        
        /// <summary>
        /// Deletes an appointments from the appointments table based on their appointment_id
        /// curl -d "" localhost/api/AppointmentsData/DeleteAppointment/5
        /// </summary>
        /// <returns>
        /// Deletes the appointment from the appointments table based on the appointment_id specified in the POST request
        /// </returns>

        // POST: api/AppointmentsData/DeleteAppointment/5
        [ResponseType(typeof(Appointments))]
        [HttpPost]
        public IHttpActionResult DeleteAppointments(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointments);
            db.SaveChanges();

            return Ok(appointments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentsExists(int id)
        {
            return db.Appointments.Count(e => e.appointment_id == id) > 0;
        }
    }
}
