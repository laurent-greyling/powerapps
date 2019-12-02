using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCreation.Entities
{
    [Table("TicketDetails")]
    public class TicketDetailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ClosedDate { get; set; }

        public string Priority { get; set; }
        public string Department { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }

    }
}
