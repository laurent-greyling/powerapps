using System;
using System.Threading.Tasks;
using ServiceDeskTickets.Messages;

namespace ServiceDeskTickets.MessageHandlers
{
    public class TicketRaisedMessageHandler : IMessageHandler
    {
        private readonly IMessageSenderProvider _messageSenderFactory;

        public TicketRaisedMessageHandler(
            IMessageSenderProvider messageSenderFactory
            )
        {
            _messageSenderFactory = messageSenderFactory;
        }

        public Task HandleAsync(IBrokeredMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
