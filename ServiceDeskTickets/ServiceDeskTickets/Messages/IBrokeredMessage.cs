using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDeskTickets.Messages
{
    public interface IBrokeredMessage
    {
        string MessageId { get; set; }
        IDictionary<string, object> Properties { get; }

        Task CompleteAsync();

        DateTime ScheduledEnqueueTimeUtc { get; set; }
    }
}
