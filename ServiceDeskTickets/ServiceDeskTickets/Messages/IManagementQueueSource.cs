namespace ServiceDeskTickets.Messages
{
    public interface IManagementQueueSource
    {
        IMessageSender ToManagementQueue();
        IMessageSender ToWorkQueue();
    }
}
