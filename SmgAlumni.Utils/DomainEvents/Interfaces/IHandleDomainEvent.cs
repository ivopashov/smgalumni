using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.DomainEvents.Interfaces
{
    public interface IHandleDomainEvent<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}
