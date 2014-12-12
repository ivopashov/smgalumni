using Core.EmailQuerer.Serialization;
namespace Core.EmailQuerer
{
    public interface INotificationSender
    {
        void SendEmailNotification(EmailNotificationOptions options);
    }
}
