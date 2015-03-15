using SmgAlumni.Utils.EmailQuerer.Serialization;
using SmgAlumni.Utils.Settings;
using System.Net.Mail;

namespace SmgAlumni.Utils.EmailQuerer
{
    public class NotificationSender : INotificationSender
    {
        private readonly EmailQueuer _emailQueuer;
        private readonly AppSettings _appSettings;

        public NotificationSender(EmailQueuer emailQueuer, AppSettings appSettings)
        {
            _emailQueuer = emailQueuer;
            _appSettings = appSettings;
        }

        public void SendEmailNotification(EmailNotificationOptions options)
        {
            var email = new FluentEmail.Email
            {
                Message = { From = new MailAddress(_appSettings.Email.FromAddress) }
            };
            email.To(options.To)
                .Subject(options.Template.Subject)
                .UsingTemplate(options.Template.Template, options.Template.Data);
            _emailQueuer.Enqueue(email);
        }
    }
}
