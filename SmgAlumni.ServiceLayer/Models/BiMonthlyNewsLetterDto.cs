using System.Collections.Generic;

namespace SmgAlumni.ServiceLayer.Models
{
    public class BiMonthlyNewsLetterDto
    {
        public List<string> Causes;
        public List<string> News;
        public List<string> Listings;  
        public List<string> AddedUsers;

        public BiMonthlyNewsLetterDto()
        {
            Causes = new List<string>();
            News = new List<string>();
            AddedUsers = new List<string>();
            Listings = new List<string>();
        }
    }
}
