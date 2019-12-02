using DatabaseCreation.Entities;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

namespace DatabaseCreation.Database
{

    public class DatabaseContext : DbContext
    {
        readonly AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

        public DbSet<TicketDetailsEntity> TicketDetails { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<ProvidersEntity> Providers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();
                var keyvault = configuration.GetConnectionString("Keyvault");

                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                var secret = keyVaultClient.GetSecretAsync($"{keyvault}DbConnectionString").ConfigureAwait(false).GetAwaiter();
                optionsBuilder.UseSqlServer(secret.GetResult().Value);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
        }
    }
}
