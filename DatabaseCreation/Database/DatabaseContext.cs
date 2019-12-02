using DatabaseCreation.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCreation.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TicketDetailsEntity> TicketDetails { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<ProvidersEntity> Providers { get; set; }
    }
}
