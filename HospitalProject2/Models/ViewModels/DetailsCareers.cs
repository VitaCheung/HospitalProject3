using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsCareers
    {
       
        public CareersDto SelectedCareer { get; set; }

        public DepartmentsDto RelatedDepartment { get; set; }

        public bool IsAdmin { get; set; }
    }
}