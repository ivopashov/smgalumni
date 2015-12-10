
namespace SmgAlumni.EF.Models
{
    public class Tag : IEntity
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }

        public int Id
        {
            get;
            set;
        }
    }
}
