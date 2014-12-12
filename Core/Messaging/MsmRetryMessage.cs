using System;

namespace Core.Messaging
{
    [Serializable]
    public class MsmRetryMessage<T> where T : IMsmqMessage
    {
        public MsmRetryMessage(T innerMessage)
        {
            Retries = 0;
            InnerMessage = innerMessage;
        }
        public int Retries { get; set; }
        public T InnerMessage { get; set; }
    }
}
