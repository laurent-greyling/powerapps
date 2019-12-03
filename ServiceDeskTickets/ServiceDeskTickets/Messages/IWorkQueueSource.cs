namespace ServiceDeskTickets.Messages
{
    public interface IWorkQueueSource
    {
        IMessageSender ToWorkQueue();
    }
}
