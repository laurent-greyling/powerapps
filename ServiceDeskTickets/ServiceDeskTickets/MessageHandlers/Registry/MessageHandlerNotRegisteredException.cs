using System;

namespace ServiceDeskTickets.MessageHandlers.Registry
{
    public class MessageHandlerNotRegisteredException : Exception
    {
        private readonly string _messageType;

        public MessageHandlerNotRegisteredException(string messageType)
        {
            _messageType = messageType;
        }

        public override string Message => $"No handler registered for message type: {_messageType}";
    }
}
