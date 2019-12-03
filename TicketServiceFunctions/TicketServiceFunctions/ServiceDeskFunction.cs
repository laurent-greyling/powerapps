using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TicketServiceFunctions.Models;

namespace TicketServiceFunctions
{
    public static class ServiceDeskFunction
    {
        [FunctionName("ServiceDeskFunction")]
        public static void Run([ServiceBusTrigger("management", Connection = "AzureWebJobsServiceBus")]string myQueueItem, ILogger log)
        {
            var openTicketModel = JsonConvert.DeserializeObject<TicketDetailsModel>(myQueueItem);
            openTicketModel.CreatedDate = DateTimeOffset.UtcNow;
            openTicketModel.Status = TicketStatus.InProgress.ToString();
        }
    }
}
