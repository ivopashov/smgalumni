using AutoMapper;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.Mapping;
using System;

namespace SmgAlumni.ServiceLayer.Models
{
    public class NewsLetterCandidateDto : IHaveCustomMappings
    {
        public string HtmlBody { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int Id { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<NewsLetterCandidate, NewsLetterCandidateDto>()
                .ForMember(dest => dest.HtmlBody, opt => opt.MapFrom(src => src.HtmlBody))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.UserName));
        }
    }
}
