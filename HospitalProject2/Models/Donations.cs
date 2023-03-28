using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models
{
    public class Donations
    {
        [Key]
        public int donation_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }

        [ForeignKey("Departments")]
        public int department_id { get; set; }
        public virtual Departments Departments { get; set; }
        public string phone { get; set; }
        public decimal amount { get; set; }
    }

    public class DonationsDto
    {
        public int donation_Id { get; set; }

        public string name { get; set; }
        public string email { get; set; }
        public int department_id { get; set; }
        public string phone { get; set; }
        public decimal amount { get; set; }



    }
}
