using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.DomainEvents.Interfaces
{
    public class ForgotPasswordEvent : IDomainEvent
    {
        public string Email { get; set; }
        public string RequestScheme { get; set; }
        public string RequestAuthority { get; set; }
    }
}
