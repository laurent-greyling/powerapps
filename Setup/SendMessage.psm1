Function New-SendTestMessage {

  $RestApiUri = 'https://<namespace>.servicebus.windows.net/management/messages'
  $sasToken = New-SaSToken -ResourceUri $RestApiUri -SasKeyName RootManageSharedAccessKey -SasKey "<key>"
  $headers = @{'Authorization'= $sasToken}

  $json = @{
    "CreatedBy"="John Wick";
    "Priority"="Medium";
    "Department"="HR";
    "Subject"="Things are wrong";
    "Description"="My PC Blew up"
  }

  $body = $json | ConvertTo-Json

  $contentType = 'application/atom+xml;type=entry;charset=utf-8'
  

  Invoke-RestMethod -Method Post -Uri $RestApiUri -Headers $headers -ContentType $contentType -Body $body
}

Function New-SaSToken {
  param (
    [Parameter(Mandatory=$true)][String]$ResourceUri,
    [Parameter(Mandatory=$true)][String]$SasKeyName,
    [Parameter(Mandatory=$true)][String]$SasKey
  )

  $dateTime = (Get-Date).ToUniversalTime() - ([datetime]'1/1/1970')
  $weekInSeconds = 7 * 24 * 60 * 60
  $expiry = [System.Convert]::ToString([int]($dateTime.TotalSeconds) + $weekInSeconds)

  $encodedResourceUri = [System.Web.HttpUtility]::UrlEncode($ResourceUri)

  $stringToSign = $encodedResourceUri + "`n" + $expiry
  $stringToSignBytes = [System.Text.Encoding]::UTF8.GetBytes($stringToSign)
  $keyBytes = [System.Text.Encoding]::UTF8.GetBytes($SasKey)
  $hmac = [System.Security.Cryptography.HMACSHA256]::new($keyBytes)
  $hashOfStringToSign = $hmac.ComputeHash($stringToSignBytes)
  $signature = [System.Convert]::ToBase64String($hashOfStringToSign)
  $encodedSignature = [System.Web.HttpUtility]::UrlEncode($signature) 

  $sasToken = "SharedAccessSignature sr=$encodedResourceUri&sig=$encodedSignature&se=$expiry&skn=$SasKeyName" 

  return $sasToken
}

Export-ModuleMember New-SendTestMessage