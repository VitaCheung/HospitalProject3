using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class UpdateVolunteers
    {
        public VolunteersDto SelectedVolunteer { get; set; }

        public IEnumerable<ProgramsDto> ProgramOptions { get; set; }

    }
}