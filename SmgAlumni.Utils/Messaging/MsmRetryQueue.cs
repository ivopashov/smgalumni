using SmgAlumni.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Messaging;

namespace SmgAlumni.Utils.Messaging
{
    public class MsmRetryQueue<T> : MsmQueueBase where T : class, IMsmqMessage
    {
        public MsmRetryQueue(AppSettings appSettings)
            : base(appSettings)
        {
            SetupQueue();
        }

        private void SetupQueue()
        {
            var typeName = typeof(T).Name;
            string queuePath = String.Concat(_appSettings.Messaging.QueuePathPrefix, typeName, "_retry");
            CreateQueuePathIfNotExists(queuePath);
            _messageQueue = new MessageQueue(queuePath) { Formatter = new BinaryMessageFormatter() };
        }

        public void Enqueue(T message)
        {
            Enqueue(new MsmRetryMessage<T>(message));
        }

        public void Enqueue(MsmRetryMessage<T> message)
        {
            var msg = new Message
            {
                Body = message,
                Formatter = new BinaryMessageFormatter()
            };

            _messageQueue.Send(msg);
        }

        public new MsmRetryMessage<T> Receive()
        {
            var message = _messageQueue.Receive();
            if (message == null)
            {
                return null;
            }

            return (MsmRetryMessage<T>)message.Body;
        }

        public IEnumerable<MsmRetryMessage<T>> ReceiveAll()
        {
            while (this.HasMessages)
            {
                yield return Receive();
            }
        }
    }
}
