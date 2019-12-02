using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCreation.Entities
{
    [Table("Departments")]
    public class DepartmentEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
