using SmgAlumni.EF.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmgAlumni.App.Models
{
    public class SearchByDivisionAndYearViewModel
    {
        public ClassDivision Division { get; set; }
        public int YearOfGraduation { get; set; }
    }
}