using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface IAccountNotificationRepository : IRepository<Notification>
    {
        IEnumerable<Notification> GetSentNotifications();
    }
}
