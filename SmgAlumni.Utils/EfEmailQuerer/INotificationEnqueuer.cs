using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using System;

namespace SmgAlumni.Utils.EfEmailQuerer
{
    [Obsolete]
    public interface INotificationEnqueuer
    {
        void EnqueueNotification(EmailNotificationOptions options, NotificationKind kind);
    }
}
