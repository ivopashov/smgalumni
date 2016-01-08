using NLog;
using SmgAlumni.App.Workers;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using System;
using System.Threading;
using System.Web.Hosting;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(DeleteAbandonedAttachments), "StartTimer")]

namespace SmgAlumni.App.Workers
{
    public class DeleteAbandonedAttachments : IRegisteredObject
    {
        private const int deleteTempKeyAttachmentsInterval = 3600;

        private bool _shuttingDown;
        private static SmgAlumniContext _context = new SmgAlumniContext();
        private static Timer _timer = new Timer(OnTimerElapsed);
        private static DeleteAbandonedAttachments _jobHost = new DeleteAbandonedAttachments();
        private static IAttachmentRepository _attachmentRepository = new AttachmentRepository(_context);
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void StartTimer()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(deleteTempKeyAttachmentsInterval));
        }

        private static void OnTimerElapsed(object sender)
        {
                _jobHost.DoWork();
        }

        public void Stop(bool immediate)
        {
            lock (LockProvider._lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public DeleteAbandonedAttachments()
        {
            HostingEnvironment.RegisterObject(this);
        }

        private void DoWork()
        {
            lock (LockProvider._lock)
            {
                if (_shuttingDown)
                {
                    return;
                }

                try
                {
                    var deleteCandidates = _attachmentRepository.AttachmentsWithoutParentAndAtLeastOneHourOld();
                    foreach (var item in deleteCandidates)
                    {
                        _attachmentRepository.Delete(item);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("Error while trying to delete attachments: " + e.Message + "\n Inner Exception: " + e.InnerException);
                }
            }
        }
    }
}