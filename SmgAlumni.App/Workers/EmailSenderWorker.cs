using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web.Hosting;
using NLog;
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

        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static EmailSenderWorker _jobHost = new EmailSenderWorker();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static AccountNotificationRepository _notificationRepository = new AccountNotificationRepository(_context);
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
            lock (LockProvider._lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public void DoWork()
        {
            lock (LockProvider._lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                try
                {
                    var unsentNotifications = _notificationRepository.GetSentNotifications();
                    SendEmailsViaSmtpClient(unsentNotifications);
                }
                catch (Exception e)
                {
                    _logger.Error("Error while sending email: " + e.Message + "\n Inner Exception: " + e.InnerException);
                }
            }
        }

        private void SendEmailsViaSmtpClient(IEnumerable<Notification> notifications)
        {
            var emailSettings = new EmailSettings(new EFSettingsRetriever(new SettingRepository(_context)));
            _logger.Info("Starting email send");

            using (var client = emailSettings.GetSmtpClient())
            {
                client.EnableSsl = emailSettings.UseSecureConnection;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                foreach (var notification in notifications)
                {
                    try
                    {
                        var mailMessage = DeserializeEmail(notification.Message);
                        _logger.Info("Sending email to: " + mailMessage.To);
                        client.Send(mailMessage);
                        notification.Sent = true;
                        notification.SentOn = DateTime.Now;
                    }
                    catch (Exception)
                    {
                        notification.Sent = false;
                        notification.Retries++;
                        throw;
                    }
                    finally
                    {
                        _notificationRepository.Update(notification);
                    }
                   
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