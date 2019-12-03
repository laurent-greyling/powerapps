namespace ServiceDeskTickets.Messages
{
    public interface IMessageSenderProvider
    {
        IWorkQueueSource FromWorkQueue();
        IManagementQueueSource FromManagementQueue();
    }
}
