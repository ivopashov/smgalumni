using System;
using System.Collections.Generic;
using System.Messaging;
using Core.Settings;

namespace Core.Messaging
{
    public class MsmQueue<T> : MsmQueueBase, IMsmQueue<T> where T : class, IMsmqMessage
    {

        private MsmRetryQueue<T> _retryQueue;
        public MsmRetryQueue<T> RetryQueue
        {
            get
            {
                return _retryQueue ?? (_retryQueue = new MsmRetryQueue<T>(_appSettings));
            }
        }

        public MsmQueue(AppSettings appSettings)
            : base(appSettings)
        {
            SetupQueue();
        }

        protected void SetupQueue()
        {
            var typeName = typeof(T).Name;
            string queuePath = String.Concat(_appSettings.Messaging.QueuePathPrefix, typeName);
            CreateQueuePathIfNotExists(queuePath);
            _messageQueue = new MessageQueue(queuePath) { Formatter = new BinaryMessageFormatter() };
        }

        public new T Receive()
        {
            var message = _messageQueue.Receive();
            if (message == null)
            {
                return null;
            }

            return (T)message.Body;
        }

        public IEnumerable<T> ReceiveAll()
        {
            while (this.HasMessages)
            {
                yield return Receive();
            }
        }

        public void Enqueue(T message)
        {
            var msg = new Message
            {
                Body = message,
                Formatter = new BinaryMessageFormatter()
            };

            _messageQueue.Send(msg);
        }
    }

    public interface IMsmQueue<T> where T : class, IMsmqMessage
    {
        MsmRetryQueue<T> RetryQueue { get; }
        T Receive();
        IEnumerable<T> ReceiveAll();
        void Enqueue(T message);
        bool HasMessages { get; }
        void Purge();
    }
}
