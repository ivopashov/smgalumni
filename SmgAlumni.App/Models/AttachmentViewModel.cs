using AutoMapper;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.Mapping;
using System;

namespace SmgAlumni.App.Models
{
    public class AttachmentViewModel : IHaveCustomMappings
    {
        public string Name { get; set; }
        public Guid TempKey { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Attachment, AttachmentViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TempKey, opt => opt.MapFrom(src => src.TempKey))
                ;
        }
    }
}