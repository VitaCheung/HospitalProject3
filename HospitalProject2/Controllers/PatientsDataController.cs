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
using System.Web.Mvc;
using HospitalProject2.Models;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace HospitalProject2.Controllers
{
    public class PatientsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        /// <summary>
        /// List all information about patients
        /// </summary>
        /// <example>
        /// localhost/api/PatientsData/ListPatients -> patient_id: 1, health_num: 123, f_name: Emily, l_name: Smith, bday: 1999-01-01, address: 123 Jones Street, phone: 111-222-3333
        /// </example>
        /// <returns>
        /// List all information about patients
        /// </returns>

        // GET: api/PatientsData/ListPatients
        [HttpGet]
        public IEnumerable<PatientsDto> ListPatients()
        {
            List<Patients> Patients = db.Patients.ToList();
            List<PatientsDto> PatientsDtos = new List<PatientsDto>();

            Patients.ForEach(a => PatientsDtos.Add(new PatientsDto()
            {
                patient_id = a.patient_id,
                health_num = a.health_num,
                f_name = a.f_name,
                l_name = a.l_name,
                bday = a.bday,
                address = a.address,
                phone = a.phone

            }));
            return PatientsDtos;
        }
        
        /// <summary>
        /// Find patients based on their patient_is and displays their information
        /// </summary>
        /// <example>
        /// localhost/api/PatientsData/FindPatient/3 -> patient_id: 1, health_num: 123, f_name: Emily, l_name: Smith, bday: 1999-01-01, address: 123 Jones Street, phone: 111-222-3333
        /// </example>
        /// <returns>
        /// Returns all information on artist based on their patient_id
        /// </returns>

        // GET: api/PatientsData/FindPatient/5
        [ResponseType(typeof(Patients))]
        [HttpGet]
        public IHttpActionResult FindPatients(int id)
        {
            Patients Patients = db.Patients.Find(id);
            PatientsDto PatientsDto = new PatientsDto()
            {
                patient_id= Patients.patient_id,
                health_num= Patients.health_num,
                f_name= Patients.f_name,
                l_name= Patients.l_name,
                bday= Patients.bday,
                address= Patients.address,
                phone= Patients.phone
            };
            if (Patients == null)
            {
                return NotFound();
            }

            return Ok(PatientsDto);
        }
        
        /// <summary>
        /// Update artist information based on their patient_id
        /// </summary>
        /// <example>
        /// using a curl request: curl -d @Patient.json -H "Content-type:application/json" localhost/api/PatientData/UpdatePatient/5
        /// Before Update:
        /// patient_id: 1,
        /// health_num: 123, 
        /// f_name: Emily, 
        /// l_name: Smith, 
        /// bday: 1999-01-01, 
        /// address: 123 Jones Street, 
        /// phone: 111-222-3333
        ///
        /// After Update:
        /// patient_id: 1,
        /// health_num: 123, 
        /// f_name: Emily, 
        /// l_name: Smith, 
        /// bday: 1999-01-01, 
        /// address: 1 James Road, 
        /// phone: 111-111-2222
        /// </example>
        /// <returns>
        /// Returns the updated information of patient based on their patient_id
        /// </returns>

        // POST: api/PatientsData/UpdatePatients/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatients(int id, Patients patients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patients.patient_id)
            {
                return BadRequest();
            }

            db.Entry(patients).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientsExists(id))
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
        /// Add a patient to the Patients table 
        /// using a curl request: curl -d @Patient.json -H "Content-type:application/json" localhost/api/ArtistData/AddPatients/
        /// </summary>
        /// <returns>
        /// Adds patient information to the Patients table 
        /// </returns>

        // POST: api/PatientsData/AddPatients
        [ResponseType(typeof(Patients))]
        [HttpPost]
        public IHttpActionResult AddPatients(Patients patients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patients);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patients.patient_id }, patients);
        }
        
        /// <summary>
        /// Deletes an patient from the Patients table based on their patient_id
        /// curl -d "" localhost/api/PatientData/DeletePatients/5
        /// </summary>
        /// <returns>
        /// Deletes the Patient from the Patients table based on the patient_id specified in the POST request
        /// </returns>

        // POST: api/PatientsData/DeletePatients/5
        [ResponseType(typeof(Patients))]
        [HttpPost]
        public IHttpActionResult DeletePatients(int id)
        {
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patients);
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

        private bool PatientsExists(int id)
        {
            return db.Patients.Count(e => e.patient_id == id) > 0;
        }
    }
}
