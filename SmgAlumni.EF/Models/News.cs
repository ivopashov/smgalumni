using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class News : IListingEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public bool Enabled { get; set; }
        public virtual User User { get; set; }
    }
}
