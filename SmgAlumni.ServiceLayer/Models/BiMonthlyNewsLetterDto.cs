using System.Collections.Generic;

namespace SmgAlumni.ServiceLayer.Models
{
    public class BiMonthlyNewsLetterDto
    {
        public IEnumerable<string> Causes;
        public IEnumerable<string> News;
        public IEnumerable<string> Listings;  
        public IEnumerable<string> AddedUsers;

        public BiMonthlyNewsLetterDto()
        {
            Causes = new List<string>();
            News = new List<string>();
            AddedUsers = new List<string>();
            Listings = new List<string>();
        }
    }
}
