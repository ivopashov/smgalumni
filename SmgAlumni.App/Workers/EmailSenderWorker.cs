using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web.Hosting;
using SmgAlumni.App.Workers;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.Settings;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(EmailSenderWorker), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public class EmailSenderWorker : IRegisteredObject
    {
        private const int CheckForMailIntervalSeconds = 600;

        private readonly object _lock = new object();
        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static EmailSenderWorker _jobHost = new EmailSenderWorker();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static NotificationRepository _notificationRepository = new NotificationRepository(_context);


        public static void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(CheckForMailIntervalSeconds));
        }

        private static void OnTimerElapsed(object sender)
        {

            _jobHost.DoWork();
        }

        public EmailSenderWorker()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public void DoWork()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                var unsentNotification = _notificationRepository.GetAll().Where(a => !a.Sent).ToList();
                SendEmailsViaSmtpClient(unsentNotification);

            }
        }

        private static void SendEmailsViaSmtpClient(List<Notification> notifications)
        {
            var emailSettings = new EmailSettings(new EFSettingsRetriever(new SettingRepository(_context)));

            using (var client = emailSettings.GetSmtpClient())
            {
                client.EnableSsl = emailSettings.UseSecureConnection;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                foreach (var notification in notifications)
                {
                    var mailMessage = DeserializeEmail(notification.Message);
                    client.Send(mailMessage);
                    notification.Sent = true;
                    _notificationRepository.Update(notification);
                }
            }
        }

        private static MailMessage DeserializeEmail(byte[] obj)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(obj))
            {
                var set = (SerializeableMailMessage)formatter.Deserialize(ms);
                return set.GetMailMessage();
            }
        }
    }
}