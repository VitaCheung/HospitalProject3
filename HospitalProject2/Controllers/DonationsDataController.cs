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
    public class DonationsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DonationsData
        [HttpGet]
        public IEnumerable<DonationsDto> ListDonations()
        {
            List<Donations> Donations = db.Donations.ToList();
            List<DonationsDto> DonationsDto = new List<DonationsDto>();

            Donations.ForEach(a => DonationsDto.Add(new DonationsDto()
            {
                donation_id = a.donation_id,
                name = a.name,
                email = a.email,
                department_id = a.department_id,
                amount = a.amount

            }));

            return DonationsDto;
        }


        // GET: api/DonationsData/5
        [ResponseType(typeof(Donations))]
        [HttpGet]

        public IHttpActionResult FindDonations(int id)

        {
            Donations Donations = db.Donations.Find(id);
            DonationsDto DonationsDto = new DonationsDto()
            {
                donation_id = Donations.donation_id,
                name = Donations.name,
                email = Donations.email,
                department_id = Donations.department_id,
                amount = Donations.amount
            };
            if (Donations == null)
            {
                return NotFound();
            }

            return Ok(DonationsDto);
        }


        // PUT: api/DonationsData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDonation(int id, Donations donations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donations.donation_id)
            {
                return BadRequest();
            }

            db.Entry(donations).State = EntityState.Modified;

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

        // POST: api/DonationsData
        [ResponseType(typeof(Donations))]
        [HttpPost]
        public IHttpActionResult AddDonation(Donations donations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donations.Add(donations);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donations.donation_id }, donations);
        }


        // DELETE: api/DonationsData/5
        [ResponseType(typeof(Donations))]
        [HttpPost]
        //[System.Web.Http.Authorize]
        public IHttpActionResult DeleteDonation(int id)
        {
            Donations donations = db.Donations.Find(id);
            if (donations == null)
            {
                return NotFound();
            }

            db.Donations.Remove(donations);
            db.SaveChanges();

            return Ok(donations);
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
            return db.Donations.Count(e => e.donation_id == id) > 0;
        }


    }
}