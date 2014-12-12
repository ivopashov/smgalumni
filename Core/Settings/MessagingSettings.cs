using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Settings
{
    public class MessagingSettings
    {
        public string QueuePathPrefix { get { return @".\private$\app_"; } }
    }
}
