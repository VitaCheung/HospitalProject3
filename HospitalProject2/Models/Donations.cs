using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject2.Models
{
    public class Donations
    {
        [Key]
        public int donation_id { get; set; }
        public string name { get; set; }

        // each program belongs to one department, but one department can have many programs
        [ForeignKey("Department")]
        public int department_id { get; set; }
        public virtual Departments Department { get; set; }
        public string email { get; set; }
        public decimal amount { get; set; }
    }

    public class DonationsDto
    {
        public int donation_id { get; set; }
        public string name { get; set; }
        public int department_id { get; set; }
        public string email { get; set; }
        public decimal amount { get; set; }
    }
}

