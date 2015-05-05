using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.DomainEvents.Interfaces
{
    public class VerifyUserEvent : IDomainEvent
    {
        public string UserName { get; set; }
        public User User{ get; set; }
    }
}
