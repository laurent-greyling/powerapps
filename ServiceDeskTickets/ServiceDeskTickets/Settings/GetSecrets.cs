using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ServiceDeskTickets.Settings
{
    public class GetSecrets
    {
        readonly AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

        public async Task<string> GetServiceBusConnection()
        {
            var connectionString = await GetSecret("SbConnectionString");

            return connectionString.Value;
        }        

        private async Task<SecretBundle> GetSecret(string secretName)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var keyvault = configuration.GetConnectionString("Keyvault");

            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            return await keyVaultClient.GetSecretAsync($"{keyvault}{secretName}").ConfigureAwait(false);
        }
    }
}
