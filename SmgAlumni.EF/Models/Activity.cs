using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
