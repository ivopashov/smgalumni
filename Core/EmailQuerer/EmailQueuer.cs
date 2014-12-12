using System;
using NLog;
using Core.EmailQuerer.Serialization;
using Core.Messaging;

namespace Core.EmailQuerer
{
    public class EmailQueuer
    {
        private readonly MsmQueue<SerializeableMailMessage> _emailQueue;
        private readonly Logger _logger;

        public EmailQueuer(MsmQueue<SerializeableMailMessage> emailQueue)
        {
            _emailQueue = emailQueue;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Enqueue(FluentEmail.Email email)
        {
            var queueMessage = new SerializeableMailMessage(email.Message);
            try
            {
                _emailQueue.Enqueue(queueMessage);
                _logger.Info("Email to: "+email.Message.To+" was sent to the queue.");
            }
            catch (Exception e)
            {
                _logger.Info(e.Message);
            }
        }
    }
}
