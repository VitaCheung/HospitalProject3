using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class UpdateStaff
    {
        public StaffsDto SelectedStaff { get; set; }

        public IEnumerable<DepartmentsDto> DepartmentOptions { get; set; }
    }
}