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
    public class ProgramsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// api function to list all programs in database
        /// </summary>
        /// <returns>a list of all programs</returns>
        // GET: api/ProgramsData/ListPrograms
        [HttpGet]
        public IEnumerable<ProgramsDto> ListPrograms()
        {
            List<Programs> Programs = db.Programs.ToList();
            List<ProgramsDto> ProgramsDtos = new List<ProgramsDto>();

            Programs.ForEach(p => ProgramsDtos.Add(new ProgramsDto()
            {
                program_id = p.program_id,
                name = p.name,
                department_id = p.department_id,
                description = p.description
            }));

            return ProgramsDtos;
        }
        /// <summary>
        /// api function to list one program based on id
        /// </summary>
        /// <param name="id">id of program</param>
        /// <returns>one program with the id specified</returns>
        // GET: api/ProgramsData/FindProgram/5
        [ResponseType(typeof(Programs))]
        [HttpGet]
        public IHttpActionResult FindProgram(int id)
        {
            Programs Programs = db.Programs.Find(id);
            ProgramsDto ProgramsDto = new ProgramsDto()
            {
                program_id = Programs.program_id,
                name = Programs.name,
                department_id = Programs.department_id,
                description = Programs.description
            };
            if (Programs == null)
            {
                return NotFound();
            }

            return Ok(ProgramsDto);
        }


        /// <summary>
        /// api function to list all program related to one department
        /// </summary>
        /// <param name="id">department id</param>
        /// <returns>programs with the same department id</returns>
        // LIST PROGRAMS FOR DEPARTMENT
        // GET: api/ProgramsData/ListProgramsForDepartment
        [HttpGet]
        [ResponseType(typeof(ProgramsDto))]
        public IHttpActionResult ListProgamsForDepartment(int id)
        {
            List<Programs> Programs = db.Programs.Where(p => p.department_id == id).ToList();
            List<ProgramsDto> ProgramsDtos = new List<ProgramsDto>();

            Programs.ForEach(p => ProgramsDtos.Add(new ProgramsDto()
            {
                program_id = p.program_id,
                name = p.name,
                department_id = p.department_id,
                description = p.description
            }));

            return Ok(ProgramsDtos);
        }
        /// <summary>
        /// api function to apply changes to a specified program
        /// </summary>
        /// <param name="id">program id</param>
        /// <param name="programs">program model</param>
        /// <returns>updated program information for one program, saved to database</returns>
        // POST: api/ProgramsData/UpdateDepartment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProgram(int id, Programs programs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != programs.program_id)
            {
                return BadRequest();
            }

            db.Entry(programs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramsExists(id))
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
        /// api function to add a new program to the database
        /// </summary>
        /// <param name="programs">program model</param>
        /// <returns>a single new program added to the database</returns>
        // POST: api/ProgramsData/AddProgram
        [ResponseType(typeof(Programs))]
        [HttpPost]
        public IHttpActionResult AddProgram(Programs programs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Programs.Add(programs);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = programs.program_id }, programs);
        }
        /// <summary>
        /// api function to delete an existing program from the database
        /// </summary>
        /// <param name="id">program id</param>
        /// <returns>the program seletced by id is removed from the database</returns>
        // POST: api/ProgramsData/DeleteProgram/5
        [ResponseType(typeof(Programs))]
        [HttpPost]
        public IHttpActionResult DeleteProgram(int id)
        {
            Programs programs = db.Programs.Find(id);
            if (programs == null)
            {
                return NotFound();
            }

            db.Programs.Remove(programs);
            db.SaveChanges();

            return Ok(programs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProgramsExists(int id)
        {
            return db.Programs.Count(e => e.program_id == id) > 0;
        }
    }
}