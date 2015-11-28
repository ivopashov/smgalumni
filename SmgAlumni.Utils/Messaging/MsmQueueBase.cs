using SmgAlumni.Utils.Settings;
using System;
using System.IO;
using System.Messaging;

namespace SmgAlumni.Utils.Messaging
{
    public class MsmQueueBase
    {
        protected readonly IAppSettings _appSettings;

        public MsmQueueBase(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        protected MessageQueue _messageQueue;

        public void SetQueue(string queueName)
        {
            string queuePath = String.Concat(_appSettings.Messaging.QueuePathPrefix, queueName);
            CreateQueuePathIfNotExists(queuePath);
            _messageQueue = new MessageQueue(queuePath) { Formatter = new BinaryMessageFormatter() };
        }

        public bool HasMessages
        {
            get
            {
                try
                {
                    _messageQueue.Peek(new TimeSpan(0));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public virtual string Receive()
        {
            var message = _messageQueue.Receive();
            if (message == null) return string.Empty;
            var reader = new StreamReader(message.BodyStream);
            return reader.ReadToEnd();
        }

        public void Purge()
        {
            _messageQueue.Purge();
        }

        protected void CreateQueuePathIfNotExists(string queuePath)
        {
            if (!MessageQueue.Exists(queuePath)) MessageQueue.Create(queuePath);
        }
    }
}
