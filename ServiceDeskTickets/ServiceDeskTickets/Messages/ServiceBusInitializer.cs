using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using ServiceDeskTickets.DependencyInjection;
using ServiceDeskTickets.Settings;
using System.Threading.Tasks;

namespace ServiceDeskTickets.Messages
{
    [Binding(BindingType.Transient, typeof(IRunAtStartup))]
    public class ServiceBusInitializer : IRunAtStartup
    {
        public async Task RunAsync()
        {
            var con = new GetSecrets();
            var serviceBusConnectionString = await con.GetServiceBusConnection();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

            await EnsureQueueExists(namespaceManager, "manager");
            await EnsureQueueExists(namespaceManager, "worker");
        }

        private static async Task EnsureQueueExists(NamespaceManager namespaceManager, string queueName)
        {
            var queueExists = await namespaceManager.QueueExistsAsync(queueName);

            if (!queueExists)
            {
                await namespaceManager.CreateQueueAsync(
                    new QueueDescription(queueName)
                    {
                        SupportOrdering = false
                    });
            }
        }
    }
}
