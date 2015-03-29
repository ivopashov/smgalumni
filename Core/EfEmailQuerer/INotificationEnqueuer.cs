using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
namespace SmgAlumni.Utils.EfEmailQuerer
{
    public interface INotificationEnqueuer
    {
        void EnqueueNotification(EmailNotificationOptions options, NotificationKind kind);
    }
}
