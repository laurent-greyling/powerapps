using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TicketServiceFunctions.Entities;
using TicketServiceFunctions.Models;

namespace TicketServiceFunctions.MapEntities
{
    public class MapEntity
    {
        public TicketDetailsEntity MapTicketDetailsEntity(string message)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TicketDetailsModel, TicketDetailsEntity>();
            });

            var mapper = config.CreateMapper();

            var openTicketModel = JsonConvert.DeserializeObject<TicketDetailsModel>(message);
            openTicketModel.CreatedDate = DateTimeOffset.UtcNow;
            openTicketModel.Status = TicketStatus.InProgress.ToString();
            var entity = mapper.Map<TicketDetailsModel, TicketDetailsEntity>(openTicketModel);

            return entity;
        }
    }
}
