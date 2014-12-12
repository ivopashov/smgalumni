using EmailSender;
using Ninject;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace EmailService
{
    public partial class EmailService : ServiceBase
    {
        private System.Timers.Timer _timer;
        private Logger _logger;
        private EmailManager _manager;
        private StandardKernel _kernel;
        private bool _working;

        public EmailService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _working = true;
            _timer = new System.Timers.Timer { Interval = 20000 };
            _timer.Elapsed += ProcessNotifications;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void ProcessNotifications(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            InitializeNinject();

            while (_working)
            {
                _manager.SendAll();
                Thread.Sleep(10000);
            }
        }

        private void InitializeNinject()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<Logger>().ToMethod(x => LogManager.GetLogger(x.Request.ParentRequest.Service.FullName));
            _manager = _kernel.Get<EmailManager>();
            _logger = LogManager.GetCurrentClassLogger();

        }

        protected override void OnStop()
        {
            _working = false;
        }
    }
}
