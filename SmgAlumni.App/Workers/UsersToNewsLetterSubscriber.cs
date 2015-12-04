using SmgAlumni.App.Workers;
using System.Web.Hosting;
using WebActivatorEx;
using System;
using SmgAlumni.EF.DAL;
using System.Threading;
using SmgAlumni.Utils.Settings;
using NLog;
using SmgAlumni.Data.Repositories;
using SmgAlumni.ServiceLayer;

[assembly: PostApplicationStartMethod(typeof(UsersToNewsLetterSubscriber), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public static class LockProvider
    {
        public static object _lock = new object();
    }

    /// <summary>
    /// this worker searches for user that are not subscribed for the newsletter and subscribed them
    /// this is valid only for the initial subscription and will not subscribe users again once they have unsubscribed by themselves
    /// </summary>
    public class UsersToNewsLetterSubscriber : IRegisteredObject
    {
        private const int cleanIntervalInSecs = 7200; // 2 hours. why not? :)

        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static UsersToNewsLetterSubscriber _jobHost = new UsersToNewsLetterSubscriber();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static UserRepository _userRepository = new UserRepository(_context);
        private static RequestSender _requestSender = new RequestSender(_appSettings);
        private static UserService _userService = new UserService(_appSettings, _userRepository, _requestSender);

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(cleanIntervalInSecs));
        }

        public void Stop(bool immediate)
        {
            lock (LockProvider._lock)
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
            lock (LockProvider._lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                try
                {
                    SubscribeUsers();
                }
                catch (Exception EX_NAME)
                {
                    _logger.Error(EX_NAME.Message);
                }

            }
        }

        private void SubscribeUsers()
        {
            var unsubscribedUsers = _userRepository.UnSubscribedUsersToNewsLetter();
            foreach (var user in unsubscribedUsers)
            {
                _userService.AddUserToNewsLetterList(user);
            }
        }
    }
}