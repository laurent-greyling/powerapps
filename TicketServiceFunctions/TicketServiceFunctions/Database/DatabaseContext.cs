using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using TicketServiceFunctions.Entities;

namespace TicketServiceFunctions.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TicketDetailsEntity> TicketDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("DbConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}
