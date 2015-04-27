using SmgAlumni.EF.Models.enums;

namespace SmgAlumni.App.Models
{
    public class SearchByDivisionAndYearViewModel
    {
        public ClassDivision Division { get; set; }
        public int YearOfGraduation { get; set; }
    }
}