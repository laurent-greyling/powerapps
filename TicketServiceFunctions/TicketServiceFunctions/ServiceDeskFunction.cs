using System;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TicketServiceFunctions.Database;
using TicketServiceFunctions.Entities;
using TicketServiceFunctions.MapEntities;
using TicketServiceFunctions.Models;

namespace TicketServiceFunctions
{
    public static class ServiceDeskFunction
    {
        [FunctionName("ServiceDeskFunction")]
        public static void Run([ServiceBusTrigger("management", Connection = "AzureWebJobsServiceBus")]string message)
        {
            using (var context = new DatabaseContext())
            {
                var entity = new MapEntity();
                context.TicketDetails.Add(entity.MapTicketDetailsEntity(message));

                context.SaveChanges();
            }
        }
        
    }
}
