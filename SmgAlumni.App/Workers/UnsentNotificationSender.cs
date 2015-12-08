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
using System.Net;
using SmgAlumni.ServiceLayer;
using RestSharp;

[assembly: PostApplicationStartMethod(typeof(UnsentNotificationSender), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public class UnsentNotificationSender : IRegisteredObject
    {
        private const int CheckForMailIntervalSeconds = 120;

        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static UnsentNotificationSender _jobHost = new UnsentNotificationSender();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static AccountNotificationRepository _notificationRepository = new AccountNotificationRepository(_context);
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static RequestSender _requestSender = new RequestSender(_appSettings);

        public static void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(CheckForMailIntervalSeconds));
        }

        private static void OnTimerElapsed(object sender)
        {

            _jobHost.DoWork();
        }

        public UnsentNotificationSender()
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
                    var unsentNotifications = _notificationRepository.GetUnSentNotifications();
                    Send(unsentNotifications);
                }
                catch (Exception e)
                {
                    _logger.Error("Error while sending email: " + e.Message + "\n Inner Exception: " + e.InnerException);
                }
            }
        }

        private void Send(IEnumerable<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                var result = _requestSender.InitializeClient()
                .AddParameter("domain", "www.smg-alumni.com", ParameterType.UrlSegment)
                         .SetResource("{domain}/messages")
                         .AddParameter("from", _appSettings.MailgunSettings.From)
                         .AddParameter("subject", "Забравена парола")
                         .AddParameter("html", notification.HtmlMessage)
                         .AddParameter("to", notification.To)
                         .SetMethod(Method.POST)
                         .Execute()
                         .StatusCode;

                if (result == HttpStatusCode.OK || result == HttpStatusCode.Accepted || result == HttpStatusCode.Created)
                {
                    MarkItemsAsSent(notification);
                }

            }
        }

        private void MarkItemsAsSent(Notification notification)
        {
            notification.Sent = true;
            notification.SentOn = DateTime.Now;
            _notificationRepository.Update(notification);
        }
    }
}