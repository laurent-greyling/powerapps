using Microsoft.ServiceBus.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDeskTickets.Messages
{
    public class ServiceBusMessageSender : IMessageSender
    {
        private readonly MessageSender _messageSender;

        public ServiceBusMessageSender(MessageSender messageSender)
        {
            _messageSender = messageSender;
        }
        public Task SendAsync(BrokeredMessage message)
        {
            return _messageSender.SendAsync(message);
        }

        public Task SendBatchAsync(IEnumerable<BrokeredMessage> messages)
        {
            return _messageSender.SendBatchAsync(messages);
        }
    }
}
