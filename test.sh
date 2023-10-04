function Invoke-TestApi {
    param(
        [Parameter(Mandatory = $true)]
        [string]$IPAddress,
        [Parameter(Mandatory = $true)]
        [int]$DurationInMinutes,
        [Parameter(Mandatory = $true)]
        [int]$HealthfailStartInMinutes,
        [Parameter(Mandatory = $true)]
        [int]$HealthfailDurationInMinutes,
        [Parameter(Mandatory = $true)]
        [string]$Backend
    )

    Write-Host "$DurationInMinutes minutes total with $HealthfailDurationInMinutes minutes unhealthy after $HealthfailStartInMinutes minutes ($Backend backend)"

    $DurationInSeconds = $DurationInMinutes * 60
    $HealthfailStartInSeconds = $HealthfailStartInMinutes * 60
    $HealthfailDurationInSeconds = $HealthfailDurationInMinutes * 60
    $url = "http://$IPAddress/timeout?durationInSeconds=$DurationInSeconds&healthfailStartInSeconds=$HealthfailStartInSeconds&healthfailDurationInSeconds=$HealthfailDurationInSeconds"
    Write-Host "$(Get-Date -Format "HH:mm:ss") Invoking $url"
    $result = Invoke-WebRequest -Uri $url -Method GET -Verbose -TimeoutSec ($DurationInSeconds + 10)
    Write-Host "$(Get-Date -Format "HH:mm:ss") Result: $($result.StatusCode) - $($result.Content)"
}

$IPAddress = "20.67.132.84"
$Backend = "Linux"
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 4 -HealthfailStartInMinutes 1 -HealthfailDurationInMinutes 2 -Backend $Backend
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 8 -HealthfailStartInMinutes 2 -HealthfailDurationInMinutes 4 -Backend $Backend
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 12 -HealthfailStartInMinutes 3 -HealthfailDurationInMinutes 6 -Backend $Backend
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 16 -HealthfailStartInMinutes 4 -HealthfailDurationInMinutes 8 -Backend $Backend

$IPAddress = "20.82.173.109"
$Backend = "Windows"
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 4 -HealthfailStartInMinutes 1 -HealthfailDurationInMinutes 2 -Backend $Backend
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 8 -HealthfailStartInMinutes 2 -HealthfailDurationInMinutes 4 -Backend $Backend
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 12 -HealthfailStartInMinutes 3 -HealthfailDurationInMinutes 6 -Backend $Backend
Invoke-TestApi -IPAddress $IPAddress -DurationInMinutes 16 -HealthfailStartInMinutes 4 -HealthfailDurationInMinutes 8 -Backend $Backend
