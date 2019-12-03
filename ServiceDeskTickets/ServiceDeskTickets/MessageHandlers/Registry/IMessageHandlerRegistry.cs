using System;

namespace ServiceDeskTickets.MessageHandlers.Registry
{
    public interface IMessageHandlerRegistry
    {
        void RegisterMessageTypeHandler(string messageType, Type messageHandlerType);

        Type GetMessageTypeHandler(string messageType);
    }
}
