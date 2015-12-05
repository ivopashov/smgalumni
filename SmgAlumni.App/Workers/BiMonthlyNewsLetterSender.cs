
using NLog;
using SmgAlumni.App.Workers;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.ServiceLayer;
using SmgAlumni.ServiceLayer.Models;
using SmgAlumni.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(BiMonthlyNewsLetterSender), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public class BiMonthlyNewsLetterSender : IRegisteredObject
    {
        private const int CheckForMailIntervalSeconds = 14400;

        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static BiMonthlyNewsLetterSender _jobHost = new BiMonthlyNewsLetterSender();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static NewsLetterCandidateRepository _newsLetterCandidateRepository = new NewsLetterCandidateRepository(_context);
        private static NewsLetterGenerator _newsLetterGenerator = new NewsLetterGenerator(_appSettings);
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(CheckForMailIntervalSeconds));
        }

        private static void OnTimerElapsed(object sender)
        {
            //bimonthly
            if (DateTime.Now.Day % 14 != 0)
            {
                return;
            }
            else
            {
                _jobHost.DoWork();
            }
        }

        public BiMonthlyNewsLetterSender()
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
                    var model = new BiMonthlyNewsLetterDto()
                    {
                        AddedUsers = GetRegisteredUsers(),
                        Causes = GetCauses(),
                        Listings = GetListings(),
                        News = GetNews()
                    };

                    _newsLetterGenerator.GenerateNewsLetter(model);

                }
                catch (Exception e)
                {
                    _logger.Error("Error while sending email: " + e.Message + "\n Inner Exception: " + e.InnerException);
                }
            }
        }

        public IEnumerable<string> GetNews()
        {
            var events = _newsLetterCandidateRepository.GetUnsentOfType(EF.Models.enums.NewsLetterItemType.NewsAdded).Select(a => a.HtmlBody);
            return events;
        }

        public IEnumerable<string> GetCauses()
        {
            var events = _newsLetterCandidateRepository
                .GetOfType(EF.Models.enums.NewsLetterItemType.CauseAdded)
                .OrderByDescending(a => a.CreatedOn)
                .Take(4)
                .Select(a => a.HtmlBody);
            return events;
        }

        public IEnumerable<string> GetListings()
        {
            var events = _newsLetterCandidateRepository.GetUnsentOfType(EF.Models.enums.NewsLetterItemType.ListingAdded).Select(a => a.HtmlBody);
            return events;
        }

        public IEnumerable<string> GetRegisteredUsers()
        {
            var events = _newsLetterCandidateRepository.GetUnsentOfType(EF.Models.enums.NewsLetterItemType.UserRegistered).Select(a => a.HtmlBody);
            return events;
        }
    }
}