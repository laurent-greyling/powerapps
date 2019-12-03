using ServiceDeskTickets.Messages;
using System.Threading.Tasks;

namespace ServiceDeskTickets.MessageHandlers
{
    public interface IMessageHandler
    {
        Task HandleAsync(IBrokeredMessage message);
    }
}
