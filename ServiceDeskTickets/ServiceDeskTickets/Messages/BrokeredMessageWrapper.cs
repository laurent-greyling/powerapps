using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDeskTickets.Messages
{
    public class BrokeredMessageWrapper : IBrokeredMessage
    {

        public BrokeredMessageWrapper(BrokeredMessage message)
        {
            Message = message;
        }

        internal BrokeredMessage Message { get; }

        public IDictionary<string, object> Properties => Message.Properties;

        public string MessageId { get => Message.MessageId; set => Message.MessageId = value; }

        public DateTime ScheduledEnqueueTimeUtc { get => Message.ScheduledEnqueueTimeUtc; set => Message.ScheduledEnqueueTimeUtc = value; }

        public Task CompleteAsync()
        {
            return Message.CompleteAsync();
        }
    }
}
