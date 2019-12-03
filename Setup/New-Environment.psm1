function Initialize-Environment
{
    param(
        [Parameter(Mandatory)]
        [string]
        $resourceGroupName,
        [Parameter(Mandatory)]
        [ValidateSet(
        "northeurope",
        "westeurope",
        "centralus",
        "eastus", 
        "westus")]
        [string]
        $location
    )

    $signedIn = SignIn

    if($signedIn)
    {        
        $basicName = $resourceGroupName -replace '[^a-zA-Z0-9]', ''

        CreateResourceGroup `
        -ResourceGroupName $resourceGroupName
        
        CreateKeyvault `
        -resourceGroupName $resourceGroupName `
        -basicName $basicName `
        -location $location

        $storageName = CreateStorage `
        -resourceGroupName $resourceGroupName `
        -basicName $basicName `
        -location $location
        
        CreateFunction `
        -resourceGroupName $resourceGroupName `
        -basicName $basicName `
        -location $location `
        -storageName $storageName

        CreateServiceBus `
        -resourceGroupName $resourceGroupName `
        -basicName $basicName `
        -location $location
        
        CreateSqlServer -resourceGroupName $resourceGroupName `
        -basicName $basicName `
        -location $location
    }
}

#Add secrets to keyvault
function Set-KeyvaultSecrets
{
    param(
        [Parameter(Mandatory)]
        [string]
        $resourceGroupName,
        [Parameter(Mandatory)]
        [string]
        $dbConnectionStringValue
    )

    $signedIn = SignIn

    if($signedIn)
    {        
        $basicName = $resourceGroupName -replace '[^a-zA-Z0-9]', ''
        $kvName = $basicName.ToLower() + "kv"
        $storageKVName = "StorageConnectionString"
        $dBKVName = "DbConnectionString"
        $servicebusKVName = "SbConnectionString"
        $appInsightsKVName = "InstrumentationKey"

        Write-Host "Start adding secrets to Keyvault, be patient..." -ForegroundColor DarkYellow

        $storageKvExists = az keyvault secret show `
        --name $storageKVName `
        --vault-name $kvName

        if(!$storageKvExists)
        {
            Write-Host "Set storage details to Keyvault" -ForegroundColor Yellow
            $storageName = $basicName.ToLower() + "storage"
            $storageConnectionstring = az storage account show-connection-string `
            -g $resourceGroupName `
            -n $storageName `
            --query connectionString
        
            az keyvault secret set `
            --name $storageKVName `
            --vault-name $kvName `
            --value $storageConnectionstring
            
            Write-Host "Storage Done" -ForegroundColor Green
        }  
        
        $dBKvExists = az keyvault secret show `
        --name $dBKVName `
        --vault-name $kvName
        
        if(!$dBKvExists)
        {
            Write-Host "Set DB details to Keyvault" -ForegroundColor Yellow
            az keyvault secret set `
            --name $dBKVName `
            --vault-name $kvName `
            --value $dbConnectionStringValue

            Write-Host "Sql DB Done" -ForegroundColor Green
        }   
        
        $sbKvExists = az keyvault secret show `
        --name $servicebusKVName `
        --vault-name $kvName  
         
        if(!$sbKvExists)
        {
            Write-Host "Set Servicebus details to Keyvault" -ForegroundColor Yellow
            $nameSpaceName = $basicName.ToLower() + "servicebus"
            $sfConnectionString = az servicebus namespace authorization-rule keys list `
            --name RootManageSharedAccessKey `
            --namespace-name $nameSpaceName `
            --resource-group $resourceGroupName `
            --query primaryConnectionString
            
            az keyvault secret set `
            --name $servicebusKVName `
            --vault-name $kvName `
            --value $sfConnectionString

            Write-Host "Servicebus Done" -ForegroundColor Green
        }

        #Install extention else App Insights wont work - this is in preview...
        #az extension add --name application-insights

        $appInsightsKvExists = az keyvault secret show `
        --name $appInsightsKVName `
        --vault-name $kvName

        if(!$appInsightsKvExists)
        {
            #when function was created the app insights were created as function uses it, has the same name
            $functionName = $basicName.ToLower() + "func"

            Write-Host "Set Application Insights details to Keyvault" -ForegroundColor Yellow

            $instrumentationKey = az monitor app-insights component show `
            -g $resourceGroupName `
            --app $functionName `
            --query instrumentationKey

            az keyvault secret set `
            --name $appInsightsKVName `
            --vault-name $kvName `
            --value $instrumentationKey

            Write-Host "App Insights Done" -ForegroundColor Green
        }
    }
}

#SignIn to Azure
function SignIn
{    
    $signedIn = az account show
    if(!$signedIn)
    {
        az login --use-device-code
        $signedIn = az account show
    }

    Return $signedIn
}

#Create Resource Group if Not Exist
function CreateResourceGroup
{
    param([string]$resourceGroupName)

    Write-Host "Checking Resource Group $resourceGroupName" -ForegroundColor Yellow
    $groupExists = az group exists -n $resourceGroupName

    if($groupExists -eq $false)
    {
       Write-Host "Creating Resource Group $resourceGroupName" -ForegroundColor Green
       az group create -n $resourceGroupName -l $location
    }
}

#Create Storage if Not Exist   
function CreateStorage
{    
    param(
        [string]$resourceGroupName,
        [string]$basicName,
        [string]$location
    )         

    $storageName = $basicName.ToLower() + "storage"
    Write-Host "Checking Storage Account $storageName" -ForegroundColor Yellow
    $storageExists = az storage account show `
    -g $resourceGroupName `
    -n $storageName

    if(!$storageExists)
    {
        Write-Host "Creating Storage Account $storageName" -ForegroundColor Green
        az storage account create `
        --name $storageName `
        --resource-group $resourceGroupName `
        --location $location `
        --sku Standard_GRS
    }

    return $storageName
}

#Create Azure Function if Not Exist
function CreateFunction
{
    param(
        [string]$resourceGroupName,
        [string]$basicName,
        [string]$location,
        [string]$storageName
    )    

    $functionName = $basicName.ToLower() + "func"
    Write-Host "Checking Function $functionName" -ForegroundColor Yellow
    $functionExists = az functionapp show `
    --name $functionName `
    --resource-group $resourceGroupName        

    if(!$functionExists)
    {
        Write-Host "Creating Function $functionName" -ForegroundColor Green
        az functionapp create `
        -g $resourceGroupName `
        -n $functionName `
        -s $storageName `
        --consumption-plan-location $location 
    }
}

#Create ServiceBus if Not Exist
function CreateServiceBus
{
    param(
        [string]$resourceGroupName,
        [string]$basicName,
        [string]$location
    ) 
    
    $serviceBusNameSpace = $basicName.ToLower() + "servicebus"
    Write-Host "Checking Service Bus Namespace $serviceBusNameSpace" -ForegroundColor Yellow
    $serviceBusExist = az servicebus namespace show `
    --resource-group $resourceGroupName `
    --name $serviceBusNameSpace

    if(!$serviceBusExist)
    {
        Write-Host "Creating Service Bus Namespace $serviceBusNameSpace" -ForegroundColor Green
        az servicebus namespace create `
        --resource-group $resourceGroupName `
        --name $serviceBusNameSpace `
        --location $location `
        --sku Basic

        az servicebus queue create `
        --resource-group $resourceGroupName `
        --namespace-name $serviceBusNameSpace `
        --name "management"

        az servicebus queue create `
        --resource-group $resourceGroupName `
        --namespace-name $serviceBusNameSpace `
        --name "worker"
    }
}

#Create SQL if Not Exist
function CreateSqlServer
{
    param(
        [string]$resourceGroupName,
        [string]$basicName,
        [string]$location
    ) 
    
    $sqlServerName = $basicName.ToLower() + "sqlserver"
    Write-Host "Checking sql server $sqlServerName" -ForegroundColor Yellow

    $sqlServerExists = az sql server show `
    --name $sqlServerName `
    --resource-group $resourceGroupName

    if(!$sqlServerExists)
    {

        Write-Host "Creating sql server $sqlServerName" -ForegroundColor Green
        az sql server create `
        --admin-password "Sql@server12user" `
        --admin-user "sqlserveruser" `
        --name $sqlServerName `
        --resource-group $resourceGroupName

        az sql db create `
        -g $resourceGroupName `
        -s $sqlServerName `
        -n "ServiceDesk"
    }
}

#Create Application Insights if not exists
function CreateKeyvault
{
    param(
        [string]$resourceGroupName,
        [string]$basicName,
        [string]$location
    ) 

    $kvName = $basicName.ToLower() + "kv"
    Write-Host "Checking Keyvault $kvName" -ForegroundColor Yellow

    $keyvaultExists = az keyvault show --name $kvName

    if(!$keyvaultExists)
    {
        Write-Host "Creating keyvault $kvName" -ForegroundColor Green
        az keyvault create `
        --location $location `
        --name $kvName `
        --resource-group $resourceGroupName
    }
}

Export-ModuleMember Initialize-Environment
Export-ModuleMember Set-KeyvaultSecrets