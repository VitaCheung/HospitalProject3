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
    public class ServicesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ServicesData/ListServices
        [HttpGet]
        public IEnumerable<ServicesDto> ListServices()
        {
            List<Services> Services = db.Services.ToList();
            List<ServicesDto> ServicesDtos = new List<ServicesDto>();

            Services.ForEach(s => ServicesDtos.Add(new ServicesDto()
            {
                service_id = s.service_id,
                name = s.name,
                program_id = s.program_id,
                description = s.description,
                location = s.location
            }));

            return ServicesDtos;
        }

        // GET: api/ServicesData/FindService/5
        [ResponseType(typeof(Services))]
        [HttpGet]
        public IHttpActionResult FindService(int id)
        {
            Services services = db.Services.Find(id);
            ServicesDto ServicesDto = new ServicesDto()
            {
                service_id = services.service_id,
                name = services.name,
                program_id = services.program_id,
                description = services.description,
                location = services.location
            };
            if (services == null)
            {
                return NotFound();
            }

            return Ok(ServicesDto);
        }

        // LIST SERVICES FOR PROGRAM
        // GET: api/ServicesData/ListServicesForProgram
        [HttpGet]
        [ResponseType(typeof(ServicesDto))]
        public IHttpActionResult ListServicesForProgram(int id)
        {
            List<Services> Services = db.Services.Where(s => s.program_id == id).ToList();
            List<ServicesDto> ServicesDtos = new List<ServicesDto>();

            Services.ForEach(s => ServicesDtos.Add(new ServicesDto()
            {
                service_id = s.service_id,
                name = s.name,
                program_id = s.program_id,
                description = s.description,
                location = s.location
            }));

            return Ok(ServicesDtos);
        }

        // POST: api/ServicesData/UpdateService/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateService(int id, Services services)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != services.service_id)
            {
                return BadRequest();
            }

            db.Entry(services).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesExists(id))
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

        // POST: api/ServicesData/AddService
        [ResponseType(typeof(Services))]
        [HttpPost]
        public IHttpActionResult AddService(Services services)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(services);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = services.service_id }, services);
        }

        // POST: api/ServicesData/DeleteService/5
        [ResponseType(typeof(Services))]
        [HttpPost]
        public IHttpActionResult DeleteService(int id)
        {
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return NotFound();
            }

            db.Services.Remove(services);
            db.SaveChanges();

            return Ok(services);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServicesExists(int id)
        {
            return db.Services.Count(e => e.service_id == id) > 0;
        }
    }
}