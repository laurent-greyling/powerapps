using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus.Messaging;
using ServiceDeskTickets.Messages;
using System.Threading.Tasks;

namespace ServiceDeskTickets
{
    public static class ServiceDeskFunction
    {
        [FunctionName("ServiceDeskFunction")]
        public static async Task Run([ServiceBusTrigger("management")]BrokeredMessage message, ILogger log)
        {
            var messageHandler = await StartUp.GetMessageHandlerAsync();
            await messageHandler.HandleAsync(new BrokeredMessageWrapper(message));
        }
    }
}
