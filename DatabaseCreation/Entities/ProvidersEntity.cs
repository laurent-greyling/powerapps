using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCreation.Entities
{
    [Table("Providers")]
    public class ProvidersEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
