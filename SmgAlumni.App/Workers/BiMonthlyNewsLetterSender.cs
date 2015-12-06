
using NLog;
using RestSharp;
using SmgAlumni.App.Workers;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using SmgAlumni.ServiceLayer;
using SmgAlumni.ServiceLayer.Models;
using SmgAlumni.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using WebActivatorEx;
using AutoMapper;

[assembly: PostApplicationStartMethod(typeof(BiMonthlyNewsLetterSender), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public class BiMonthlyNewsLetterSender : IRegisteredObject
    {
        private const int CheckForMailIntervalSeconds = 60;

        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static BiMonthlyNewsLetterSender _jobHost = new BiMonthlyNewsLetterSender();
        private static readonly AppSettings _appSettings = new AppSettings(new EFSettingsRetriever(new SettingRepository(_context)));
        private static NewsLetterCandidateRepository _newsLetterCandidateRepository = new NewsLetterCandidateRepository(_context);
        private static NewsLetterGenerator _newsLetterGenerator = new NewsLetterGenerator(_appSettings);
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static RequestSender _requestSender = new RequestSender(_appSettings);

        public static void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(CheckForMailIntervalSeconds));
        }

        private static void OnTimerElapsed(object sender)
        {
            //bimonthly
            if (false)
            //if (DateTime.Now.Day % 14 != 0)
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
                    if (_newsLetterCandidateRepository.AnyItemsSentToday())
                    {
                        return;
                    }

                    var model = new BiMonthlyNewsLetterDto()
                    {
                        AddedUsers = Mapper.Map<List<NewsLetterCandidate>, List<NewsLetterCandidateDto>>(GetRegisteredUsers()),
                        Causes = Mapper.Map<List<NewsLetterCandidate>, List<NewsLetterCandidateDto>>(GetCauses()),
                        Listings = Mapper.Map<List<NewsLetterCandidate>, List<NewsLetterCandidateDto>>(GetListings()),
                        News = Mapper.Map<List<NewsLetterCandidate>, List<NewsLetterCandidateDto>>(GetNews())
                    };

                    var newsLetterBody = _newsLetterGenerator.GenerateNewsLetter(model, HttpRuntime.AppDomainAppPath);
                    var result = _requestSender.InitializeClient()
                        .AddParameter("domain", "www.smg-alumni.com", ParameterType.UrlSegment)
                        .SetResource("{domain}/messages")
                        .AddParameter("from", _appSettings.MailgunSettings.From)
                        .AddParameter("subject", _appSettings.MailgunSettings.Subject)
                        .AddParameter("html", newsLetterBody)
                        .AddParameter("to", _appSettings.MailgunSettings.NewsLetterMailingList)
                        .SetMethod(Method.POST)
                        .Execute()
                        .StatusCode;

                    if (result == HttpStatusCode.OK || result == HttpStatusCode.Accepted || result == HttpStatusCode.Created)
                    {
                        MarkItemsAsSent(model);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("Error while sending email: " + e.Message + "\n Inner Exception: " + e.InnerException);
                }
            }
        }

        private void MarkItemsAsSent(BiMonthlyNewsLetterDto model)
        {
            var items = model.News.Concat(model.Listings).Concat(model.Causes).Concat(model.AddedUsers);
            foreach (var item in items)
            {
                var nlc = _newsLetterCandidateRepository.GetById(item.Id);
                nlc.Sent = true;
                nlc.SentOn = DateTime.Now;
                _newsLetterCandidateRepository.Update(nlc, false);
            }
            _newsLetterCandidateRepository.Save();
        }

        public List<NewsLetterCandidate> GetNews()
        {
            var events = _newsLetterCandidateRepository.GetUnsentOfType(EF.Models.enums.NewsLetterItemType.NewsAdded);
            return events.ToList();
        }

        public List<NewsLetterCandidate> GetCauses()
        {
            var events = _newsLetterCandidateRepository
                .GetOfType(EF.Models.enums.NewsLetterItemType.CauseAdded)
                .OrderByDescending(a => a.CreatedOn)
                .Take(4);
            return events.ToList();
        }

        public List<NewsLetterCandidate> GetListings()
        {
            var events = _newsLetterCandidateRepository.GetUnsentOfType(EF.Models.enums.NewsLetterItemType.ListingAdded);
            return events.ToList();
        }

        public List<NewsLetterCandidate> GetRegisteredUsers()
        {
            var events = _newsLetterCandidateRepository.GetUnsentOfType(EF.Models.enums.NewsLetterItemType.UserRegistered);
            return events.ToList();
        }
    }
}