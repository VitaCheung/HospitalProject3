using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models
{
    public class FAQs
    {
        [Key]
        public int FAQ_id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        [ForeignKey("Departments")]
        public int department_id { get; set; }
        public virtual Departments Departments { get; set; }

    }

    public class FAQsDto
    {
        public int FAQ_id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public int department_id { get; set; }

    }
}