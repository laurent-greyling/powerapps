using System;
using System.Threading.Tasks;
using ServiceDeskTickets.DependencyInjection;
using ServiceDeskTickets.MessageHandlers.Registry;
using ServiceDeskTickets.Messages;

namespace ServiceDeskTickets.MessageHandlers
{
    [Binding(BindingType.Singleton, typeof(IMessageHandler))]
    public class MessageHandler : IMessageHandler
    {
        private readonly IMessageHandlerRegistry _messageHandlerRegistry;
        private readonly ITypeCreator _typeCreator;

        public MessageHandler(
            IMessageHandlerRegistry messageHandlerRegistry,
            ITypeCreator typeCreator
            )
        {
            _messageHandlerRegistry = messageHandlerRegistry;
            _typeCreator = typeCreator;
        }

        public async Task HandleAsync(IBrokeredMessage message)
        {
            try
            {
                var handler = CreateMessageHandler(message);
                await handler.HandleAsync(message);
            }
            catch (Exception exception)
            {
                //need to log here
                throw;
            }
        }

        private IMessageHandler CreateMessageHandler(IBrokeredMessage message)
        {
            var messageType = message.Properties[MessageProperties.MessageType].ToString();
            var messageHandlerType = _messageHandlerRegistry.GetMessageTypeHandler(messageType);
            return (IMessageHandler)_typeCreator.Create(messageHandlerType);
        }
    }
}
