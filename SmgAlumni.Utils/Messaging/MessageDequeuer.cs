using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.Messaging
{
    /// <summary>
    /// Dequeues messages from MSMQ
    /// </summary>
    public class MessageDequeuer<T> : MessageQueueBase where T : class
    {
        private readonly MessageQueue _messageQueue;

        public MessageDequeuer()
        {

            var typeName = typeof(T).Name;
            var queuePath = GetQueuePath(typeName);
            CreateIfNotExists(queuePath);
            _messageQueue = new MessageQueue(queuePath) { Formatter = new BinaryMessageFormatter()};
            _messageQueue.MessageReadPropertyFilter.SetAll();
        }

        public T Receive()
        {
            var message = _messageQueue.Receive();
            if (message == null)
            {
                return null;
            }
            return (T)message.Body;
        }

        public List<T> GetAll()
        {
            return _messageQueue.GetAllMessages().Select(x => x.Body).Cast<T>().ToList();
        }

        public List<T> GetAllAndPurge()
        {
            var messages = GetAll();
            _messageQueue.Purge();
            return messages;
        }
    }
}
