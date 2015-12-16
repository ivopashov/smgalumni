using SmgAlumni.EF.Models;
using SmgAlumni.Utils.Mapping;

namespace SmgAlumni.App.Models
{
    public class TagDto : IHaveCustomMappings
    {
        public string Text { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<TagDto, Tag>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Text));
        }
    }
}