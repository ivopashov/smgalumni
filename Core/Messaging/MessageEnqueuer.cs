﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Core.Messaging
{

    public class MessageEnqueuer<T> : MessageQueueBase where T : class
    {
        private readonly MessageQueue _messageQueue;

        public MessageEnqueuer()
        {
            var typeName = typeof(T).Name;
            var queuePath = GetQueuePath(typeName);
            CreateIfNotExists(queuePath);
            _messageQueue = new MessageQueue(queuePath) { Formatter = new BinaryMessageFormatter() };
        }
        
        public void Enqueue(T itemToEnqueue)
        {
            if (itemToEnqueue == null) { throw new ArgumentNullException("itemToEnqueue"); }
            var msmqMessage = new Message
                              {
                                  Body = itemToEnqueue,
                                  Recoverable = true,
                                  Formatter = new BinaryMessageFormatter()
                              };
            _messageQueue.Send(msmqMessage);
        }
    }
}
