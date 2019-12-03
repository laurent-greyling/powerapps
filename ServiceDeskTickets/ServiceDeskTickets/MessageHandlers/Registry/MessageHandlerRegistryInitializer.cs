using ServiceDeskTickets.DependencyInjection;
using ServiceDeskTickets.Messages;
using System.Threading.Tasks;

namespace ServiceDeskTickets.MessageHandlers.Registry
{
    [Binding(BindingType.Transient, typeof(IRunAtStartup))]
    public class MessageHandlerRegistryInitializer : IRunAtStartup
    {
        private readonly IMessageHandlerRegistry _commandRegistry;

        public MessageHandlerRegistryInitializer(
            IMessageHandlerRegistry commandFactory
            )
        {
            _commandRegistry = commandFactory;
        }

        public Task RunAsync()
        {
            _commandRegistry.RegisterMessageTypeHandler(MessageTypes.TicketRaised, typeof(TicketRaisedMessageHandler));

            return Task.CompletedTask;
        }
    }
}
