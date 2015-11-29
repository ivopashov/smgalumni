using System;
using SmgAlumni.EF.Models.enums;

namespace SmgAlumni.EF.Models
{
    public class Activity : IEntity
    {
        public int Id { get; set; }
        public ActivityType ActivityType { get; set; }
        public virtual User User { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
