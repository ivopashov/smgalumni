using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.ServiceLayer.Models
{
    public class BiMonthlyNewsLetterDto
    {
        public IEnumerable<NewsLetterCandidate> Causes;
        public IEnumerable<NewsLetterCandidate> News;
        public IEnumerable<NewsLetterCandidate> Listings;  
        public IEnumerable<NewsLetterCandidate> AddedUsers;

        public BiMonthlyNewsLetterDto()
        {
            Causes = new List<NewsLetterCandidate>();
            News = new List<NewsLetterCandidate>();
            AddedUsers = new List<NewsLetterCandidate>();
            Listings = new List<NewsLetterCandidate>();
        }
    }
}
