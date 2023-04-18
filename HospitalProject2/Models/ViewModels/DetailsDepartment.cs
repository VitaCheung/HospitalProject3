using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsDepartment
    {
        public DepartmentsDto SelectedDepartment { get; set; }

        public IEnumerable<CareersDto> RelatedCareers { get; set; }

        public IEnumerable<ProgramsDto> RelatedPrograms { get; set; }

        public IEnumerable<StaffsDto> RelatedStaffs { get; set; }
    }
}