using ServiceDeskTickets.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ServiceDeskTickets.MessageHandlers.Registry
{
    [Binding(BindingType.Singleton, typeof(IMessageHandlerRegistry))]
    public class MessageHandlerRegistry : IMessageHandlerRegistry
    {
        private readonly IDictionary<string, Type> _messageTypeHandler = new Dictionary<string, Type>();

        public void RegisterMessageTypeHandler(string messageType, Type messageHandlerType)
        {
            _messageTypeHandler.Add(messageType, messageHandlerType);
        }

        public Type GetMessageTypeHandler(string messageType)
        {
            if (_messageTypeHandler.TryGetValue(messageType, out var commandType))
            {
                return commandType;
            }

            throw new MessageHandlerNotRegisteredException(messageType);
        }

    }
}
