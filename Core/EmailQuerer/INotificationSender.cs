using SmgAlumni.Utils.EmailQuerer.Serialization;
namespace SmgAlumni.Utils.EmailQuerer
{
    public interface INotificationSender
    {
        void SendEmailNotification(EmailNotificationOptions options);
    }
}
