using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.DomainEvents.Interfaces
{
    public class ListingDomainEvent :IDomainEvent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Heading { get; set; }
    }
}
