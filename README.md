# Powerapps
See how to connect power apps with azure services like queues, functions and and and

## Setup Environment
To setup your environment, you need to `Import-Module New-Environment.psm1 -Force` in folder called `Setup`. This will make available the powershell commands `Initialize-Environment` and `Set-KeyvaultSecrets`

Install latest [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)

Install Application Insights extention for Azure cli by running `az extension add --name application-insights` in powershell

### 1. Initialise Environment

When initialising your environment the following resources will be created:

- Resource Group
- Keyvault
- Azure Storage
- Azure Function
    - will create application Insights with same name
- Servicebus
    - queue users
    - queue providers
- Sql server
    - Db ServiceDesk

Run: `Initialize-Environment -resourceGroupName <name of resource group> -location <location of resource>`

- location you can use tab to select a region.

This command will check if you are signed in or not and if not tell you how to signin.

### 2. Set secrets into Keyvault
For later things like the different connection strings will be needed, for this we set them as secrets into keyvault for later use.

Run: `Set-KeyvaultSecrets -resourceGroupName <resource group name> -dbConnectionStringValue <sql server connection string>`

- dbConnectionStringValue is the only connection string you will have to supply yourself.

Rest of the resources will be checked and values copied to keyvault
- Storage
- Servicebus
- Application Insights InstrumentationKey