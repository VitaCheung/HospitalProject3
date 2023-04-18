using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class CareerList
    {
        public bool IsAdmin { get; set; }
        public IEnumerable<CareersDto> Careers { get; set; }
    }
}