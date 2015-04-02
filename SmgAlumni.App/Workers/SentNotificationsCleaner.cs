using SmgAlumni.App.Workers;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(SentNotificationsCleaner), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public class SentNotificationsCleaner : IRegisteredObject
    {
        private const int cleanIntervalInSecs = 30;

        private readonly object _lock = new object();
        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static SentNotificationsCleaner _jobHost = new SentNotificationsCleaner();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static NotificationRepository _notificationRepository = new NotificationRepository(_context);

        public SentNotificationsCleaner()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public static void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(cleanIntervalInSecs));
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        private static void OnTimerElapsed(object state)
        {
            _jobHost.DoWork();
        }

        public void DoWork()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                DeleteSentNotifications();

            }
        }

        private void DeleteSentNotifications()
        {
            var sentNotifications = _notificationRepository.GetAll().Where(a => a.Sent).ToList();
            foreach (var item in sentNotifications)     
            {
                _notificationRepository.Delete(item);
            }
        }
    }
}