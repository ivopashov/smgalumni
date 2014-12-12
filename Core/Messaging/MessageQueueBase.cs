using System;
using System.Messaging;

namespace Core.Messaging
{
    public abstract class MessageQueueBase
    {
        protected string BasePath = @".\private$\app_";

        protected void CreateIfNotExists(string queuePath)
        {
            if (!MessageQueue.Exists(queuePath)) MessageQueue.Create(queuePath);
        }

        protected string GetQueuePath(string typeName)
        {
            return String.Concat(BasePath, typeName);
        }

    }
}
