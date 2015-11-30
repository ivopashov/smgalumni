using AutoMapper;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.Mapping;
using System.Linq;

namespace SmgAlumni.App.Models
{
    public class UserAccountViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int YearOfGraduation { get; set; }
        public ClassDivision Division { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        //optional
        public string DwellingCountry { get; set; }
        public string UniversityGraduated { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }

        //metadata
        public bool HasPicture { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserAccountViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.YearOfGraduation, opt => opt.MapFrom(src => src.YearOfGraduation))
                .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Division))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DwellingCountry, opt => opt.MapFrom(src => src.DwellingCountry))
                .ForMember(dest => dest.UniversityGraduated, opt => opt.MapFrom(src => src.UniversityGraduated))
                .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Profession))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.HasPicture, opt => opt.MapFrom(src => DoesUserHasPicture(src)))
                ;

            configuration.CreateMap<UserAccountViewModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.YearOfGraduation, opt => opt.MapFrom(src => src.YearOfGraduation))
                .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Division))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DwellingCountry, opt => opt.MapFrom(src => src.DwellingCountry))
                .ForMember(dest => dest.UniversityGraduated, opt => opt.MapFrom(src => src.UniversityGraduated))
                .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Profession))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                ;


        }

        private bool DoesUserHasPicture(User user)
        {
            if (user.AvatarImage != null && user.AvatarImage.Length > 0)
            {
                return true;
            }

            return false;
        }
    }
}