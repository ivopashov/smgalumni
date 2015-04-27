using AutoMapper;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.Mapping;

namespace SmgAlumni.App.Models
{
    public class UserForVerifyViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int YearOfGraduation { get; set; }
        public ClassDivision Division { get; set; }
        public string Email { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserForVerifyViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.YearOfGraduation, opt => opt.MapFrom(src => src.YearOfGraduation))
                .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Division))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
        }
    }
}