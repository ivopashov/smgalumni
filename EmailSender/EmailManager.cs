using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using SmgAlumni.Utils.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class EmailManager
    {
        private readonly MsmQueue<SerializeableMailMessage> _emailQueue;
        private readonly EmailSender _emailSender;

        public EmailManager(MsmQueue<SerializeableMailMessage> emailQueue, EmailSender emailSender)
        {
            if (emailQueue == null) throw new ArgumentNullException("emailQueue");
            if (emailSender == null) throw new ArgumentNullException("emailSender");

            _emailQueue = emailQueue;
            _emailSender = emailSender;
        }

        public void SendAll()
        {
            while (_emailQueue.HasMessages)
            {
                SendNextEmail();
            }
        }

        private void SendNextEmail()
        {
            var message = _emailQueue.Receive();
            if (!_emailSender.Send(message))
            {
                _emailQueue.RetryQueue.Enqueue(message);
            }
        }
    }
}
