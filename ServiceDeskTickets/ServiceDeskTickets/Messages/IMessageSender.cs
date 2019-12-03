using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace ServiceDeskTickets.Messages
{
    public interface IMessageSender
    {
        Task SendAsync(BrokeredMessage message);
    }
}
