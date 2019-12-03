using System;   

namespace TicketServiceFunctions.Models
{
    public class TicketDetailsModel
    {
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public string Priority { get; set; }
        public string Department { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }
}
