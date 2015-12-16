using System;
using System.Collections.Generic;

namespace SmgAlumni.EF.Models
{
    public class Listing : IListingEntity, IEntity
    {
        public Listing()
        {
            Attachments = new List<Attachment>();
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public bool Enabled { get; set; }
        public virtual User User { get; set; }
        public virtual List<Attachment> Attachments { get; set; }
    }
}
