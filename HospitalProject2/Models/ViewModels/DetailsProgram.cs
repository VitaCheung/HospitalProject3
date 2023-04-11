using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsProgram
    {
        public ProgramsDto SelectedProgram { get; set; }

        public IEnumerable<ServicesDto> RelatedServices { get; set; }

    }
}