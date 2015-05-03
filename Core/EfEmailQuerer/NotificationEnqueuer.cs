using NLog;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.Settings;
using System;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmgAlumni.Utils.EfEmailQuerer
{
    public class NotificationEnqueuer : INotificationEnqueuer
    {
        private readonly AppSettings _appSettings;
        private readonly NotificationRepository _repo;

        public NotificationEnqueuer(AppSettings appSettings, NotificationRepository repo)
        {
            _appSettings = appSettings;
            _repo = repo;
        }

        public void EnqueueNotification(EmailNotificationOptions options, NotificationKind kind)
        {
            var message = ComposeEmail(options);
            var binaryFormattedMessage = ObjectToByteArray(message);

            var notification = new Notification()
            {
                CreatedOn = DateTime.Now,
                Kind = kind,
                Sent = false,
                Message = binaryFormattedMessage
            };

            _repo.Add(notification);

        }

        private SerializeableMailMessage ComposeEmail(EmailNotificationOptions options)
        {
            var email = new FluentEmail.Email
            {
                Message = { From = new MailAddress(_appSettings.Email.FromAddress) }
            };
            email.To(options.To)
                .Subject(options.Template.Subject)
                .UsingTemplate(options.Template.Template, options.Template.Data);

            var queueMessage = new SerializeableMailMessage(email.Message);

            return queueMessage;
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
