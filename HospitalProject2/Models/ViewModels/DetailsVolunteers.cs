﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsVolunteers
    {
        public VolunteersDto SelectedVolunteer { get; set; }

        public ProgramsDto RelatedProgram { get; set; }

        public bool IsAdmin { get; set; }
    }
}