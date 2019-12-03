using Microsoft.ServiceBus.Messaging;
using ServiceDeskTickets.DependencyInjection;
using System;

namespace ServiceDeskTickets.Messages
{
    [Binding(BindingType.Singleton, typeof(IMessageSenderProvider))]
    public class MessageSenderProvider : IMessageSenderProvider, IManagementQueueSource, IWorkQueueSource
    {
        private readonly Lazy<IMessageSender> _workToWorkMessageSender;
        private readonly Lazy<IMessageSender> _managementToManagementMessageSender;
        private readonly Lazy<IMessageSender> _managementToWorkMessageSender;

        public MessageSenderProvider()
        {
            var serviceBusConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");
            var messagingFactory = MessagingFactory.CreateFromConnectionString(serviceBusConnectionString);
            _workToWorkMessageSender = new Lazy<IMessageSender>(() => new ServiceBusMessageSender(messagingFactory.CreateMessageSender("worker")));
            _managementToManagementMessageSender = new Lazy<IMessageSender>(() => new ServiceBusMessageSender(messagingFactory.CreateMessageSender("management")));
            _managementToWorkMessageSender = new Lazy<IMessageSender>(() => new ServiceBusMessageSender(messagingFactory.CreateMessageSender("worker", "management")));
        }

        private IMessageSender WorkToWorkMessageSender => _workToWorkMessageSender.Value;
        private IMessageSender ManagementToManagementMessageSender => _managementToManagementMessageSender.Value;
        private IMessageSender ManagementToWorkMessageSender => _managementToWorkMessageSender.Value;

        public IManagementQueueSource FromManagementQueue()
        {
            return this;
        }

        public IWorkQueueSource FromWorkQueue()
        {
            return this;
        }

        IMessageSender IManagementQueueSource.ToManagementQueue()
        {
            return ManagementToManagementMessageSender;
        }

        IMessageSender IManagementQueueSource.ToWorkQueue()
        {
            return ManagementToWorkMessageSender;
        }

        IMessageSender IWorkQueueSource.ToWorkQueue()
        {
            return WorkToWorkMessageSender;
        }
    }
}
