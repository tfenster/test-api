param(
    [Parameter(Mandatory=$true)]
    [string]$IPAddress
)

function Invoke-TestApi {
    param(
        [Parameter(Mandatory=$true)]
        [string]$IPAddress,
        [Parameter(Mandatory=$true)]
        [int]$DurationInSeconds,
        [Parameter(Mandatory=$true)]
        [int]$HealthfailStartInSeconds,
        [Parameter(Mandatory=$true)]
        [int]$HealthfailDurationInSeconds
    )
    $url = "http://$IPAddress/timeout?durationInSeconds=$DurationInSeconds&healthfailStartInSeconds=$HealthfailStartInSeconds&healthfailDurationInSeconds=$HealthfailDurationInSeconds"
    Write-Host "Invoking $url"
    $result = Invoke-WebRequest -Uri $url -Method GET -Verbose -TimeoutSec ($DurationInSeconds + 10)
    Write-Host "Result: $($result.StatusCode) - $($result.Content)"
}

Write-Host "15 sec total, always healthy"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds 15 -HealthfailStartInSeconds 20 -HealthfailDurationInSeconds 5

Write-Host "15 sec total with 5 sec unhealthy"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds 15 -HealthfailStartInSeconds 5 -HealthfailDurationInSeconds 5

Write-Host "4 minutes total, always healthy"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (4*60) -HealthfailStartInSeconds (5*60) -HealthfailDurationInSeconds (2*60)

Write-Host "4 minutes total with 2 minute unhealthy after 1 minute"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (4*60) -HealthfailStartInSeconds (1*60) -HealthfailDurationInSeconds (2*60)

Write-Host "5 minutes total, always healthy"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (5*60) -HealthfailStartInSeconds (6*60) -HealthfailDurationInSeconds (2*60)

Write-Host "5 minutes total with 3 minute unhealthy after 1 minute"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (5*60) -HealthfailStartInSeconds (1*60) -HealthfailDurationInSeconds (3*60)

Write-Host "6 minutes total, always healthy"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (6*60) -HealthfailStartInSeconds (7*60) -HealthfailDurationInSeconds (2*60)

Write-Host "6 minutes total with 4 minute unhealthy after 1 minute"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (6*60) -HealthfailStartInSeconds (1*60) -HealthfailDurationInSeconds (4*60)

Write-Host "8 minutes total, always healthy"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (8*60) -HealthfailStartInSeconds (9*60) -HealthfailDurationInSeconds (2*60)

Write-Host "8 minutes total with 4 minute unhealthy after 2 minutes"
Invoke-TestApi -IPAddress $IPAddress -DurationInSeconds (8*60) -HealthfailStartInSeconds (2*60) -HealthfailDurationInSeconds (4*60)