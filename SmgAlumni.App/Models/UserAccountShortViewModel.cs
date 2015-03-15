using SmgAlumni.EF.Models;
using SmgAlumni.Utils.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmgAlumni.App.Models
{
    public class UserAccountShortViewModel :IHaveCustomMappings
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<User, UserAccountShortViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                ;
        }
    }
}