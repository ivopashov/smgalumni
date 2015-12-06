using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;

namespace SmgAlumni.ServiceLayer.Models
{
    [Serializable]
    public class BiMonthlyNewsLetterDto
    {
        public List<NewsLetterCandidateDto> Causes;
        public List<NewsLetterCandidateDto> News;
        public List<NewsLetterCandidateDto> Listings;  
        public List<NewsLetterCandidateDto> AddedUsers;

        public BiMonthlyNewsLetterDto()
        {
            Causes = new List<NewsLetterCandidateDto>();
            News = new List<NewsLetterCandidateDto>();
            AddedUsers = new List<NewsLetterCandidateDto>();
            Listings = new List<NewsLetterCandidateDto>();
        }
    }
}
