using System;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using NLog;
using SmgAlumni.App.Workers;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.Utils.Settings;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(SentNotificationsCleaner), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public class SentNotificationsCleaner : IRegisteredObject
    {
        private const int cleanIntervalInSecs = 43200;

        private readonly object _lock = new object();
        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static SentNotificationsCleaner _jobHost = new SentNotificationsCleaner();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static NotificationRepository _notificationRepository = new NotificationRepository(_context);
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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

                try
                {
                    DeleteSentNotifications();
                }
                catch (Exception EX_NAME)
                {
                    _logger.Error(EX_NAME.Message);
                }

            }
        }

        private void DeleteSentNotifications()
        {
            _logger.Info("Starting clean of notification repo");
            var sentNotifications = _notificationRepository.GetSentNotifications();
            foreach (var item in sentNotifications)     
            {
                _notificationRepository.Delete(item);
            }
        }
    }
}