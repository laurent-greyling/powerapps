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

### 3. Create Tables

To create the relevant tables you need to run/create the migration with the `DatabaseCreation` tool. To do this you have to:

1. In the `appsettings.json` replace the `<keyvaultname>` with your keyvault name created in step 1. This will fetch your server connection string from your keyvault
    - make sure your firewall rules allow connection to sql
    - make sure you grant yourself or have grants to Secret Management access to the Key Vault
        - Search for your Key Vault in “Search Resources dialog box” in Azure Portal.
        - Select "Overview", and click on Access policies
        - Click on "Add New", select "Secret Management" from the dropdown for "Configure from template"
        - Click on "Select Principal", add your account
        - Click on "OK" to add the new Access Policy, then click "Save" to save the Access Policies
2. If you still need to create the migration either:
    - `PM> add-migration YourMigrationName` in visual studion 
    - `> dotnet ef migrations add YourMigrationName`
3. If the migration was already created run:
    - `PM> update-database –verbose`
    - `dotnet ef database update`
4. Once the migration was run you should have the following tables in database:
    - Departments
    - Providers
    - TicketDetails

### Populate Providers and Department Tables

To fill the Providers and the Department tables, run the main console app `DatabaseCreation.exe`

This will fill the tables with fake information for later use.