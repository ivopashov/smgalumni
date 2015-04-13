using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class Listing : IListingEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public bool Enabled { get; set; }
        public int UserId { get; set; }
    }
}
