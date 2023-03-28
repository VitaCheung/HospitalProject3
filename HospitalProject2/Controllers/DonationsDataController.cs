using HospitalProject2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;  
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace HospitalProject2.Controllers
{
    public class DonationsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [ResponseType(typeof(DonationsDto))]

        public IHttpActionResult ListDonors(DonationsDto donationsDto)
        {
            List<Donations> Donations = db.Donations.ToList();
            List<DonationsDto> DonorDto = new List<DonationsDto>();

            Donations.ForEach(a => DonationsDto.Add(new DonationsDto()
            {
                donation_Id = a.donation_Id,
                Name = a.Name,
                Email = a.Email,
                department_id = a.department_id,
                Phone = a.Phone,
                Amount = a.Amount,

            }));
            return Ok(donationsDto);
        }


        [HttpGet]
        [ResponseType(typeof(Donations))]


        public IHttpActionResult FindDonations(int id)

        {
            Donations Donations = db.Donations.Find(id);


            if (Donations == null)
            {
                return NotFound();
            }

            DonationsDto DonationsDto = new DonationsDto()
            {
                donation_Id = Donations.donation_Id,
                Name = Donations.Name,
                Email = Donations.Email,
                department_id = Donations.department_id,
                Phone = Donations.Phone,
                Amount = Donations.Amount,
                Departments = new DepartmentsDto() { name = Donations.Departments.name }
            };

            return Ok(DonationsDto);
        }

    
        [HttpPost]

        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDonor(int id, Donations Donations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Donations.donation_Id)
            {
                return BadRequest();
            }

            db.Entry(Donations).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationsExists(id))
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

        [HttpPost]

        [ResponseType(typeof(Donations))]
        public IHttpActionResult AddDonor(Donations Donations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donations.Add(Donations);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Donations.donation_Id }, Donations);
        }

       
        [HttpPost]

        [ResponseType(typeof(Donations))]
        [System.Web.Http.Authorize]
        public IHttpActionResult DeleteDonations(int id)
        {
            Donations Donations = db.Donations.Find(id);
            if (Donations == null)
            {
                return NotFound();
            }

            db.Donations.Remove(Donations);
            db.SaveChanges();

            return Ok(Donations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DonationsExists(int id)
        {
            return db.Donations.Count(e => e.donation_Id == id) > 0;
        }

    }

}
