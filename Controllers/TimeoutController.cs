using Microsoft.AspNetCore.Mvc;

namespace test_api.Controllers;

[ApiController]
[Route("[controller]")]
public class TimeoutController : ControllerBase
{
    private readonly ILogger<TimeoutController> _logger;
    private DateTime _startTime;
    private static bool _healthy = true;

    public TimeoutController(ILogger<TimeoutController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get(int durationInSeconds = 60*15, int healthfailStartInSeconds = 60*5, int healthfailDurationInSeconds = 60*5)
    {
        _logger.LogInformation("Get called");

        var headers = Request.Headers;
        foreach (var header in headers)
        {
            var key = header.Key;
            var value = header.Value;
            _logger.LogInformation("Header: {key} = {value}", key, value);
        }

        var url = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
        _logger.LogInformation("URL: {url}", url);

        _startTime = DateTime.UtcNow;
        while (DateTime.UtcNow < _startTime.AddSeconds(durationInSeconds))
        {
            if (DateTime.UtcNow > _startTime.AddSeconds(healthfailStartInSeconds) && DateTime.UtcNow < _startTime.AddSeconds(healthfailStartInSeconds + healthfailDurationInSeconds))
            {
                if (_healthy)
                {
                    _logger.LogInformation("Turn unhealthy");
                }
                _healthy = false;
            }
            else
            {
                if (!_healthy)
                {
                    _logger.LogInformation("Turn healthy");
                }
                _healthy = true;
            }
            Thread.Sleep(1000);
        }

        _logger.LogInformation("Get finished for {url}", url);
        return "Done";
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        if (_healthy)
        {
            _logger.LogInformation("Health check OK");
            return Ok();
        }
        else
        {
            _logger.LogInformation("Health check failed");
            return StatusCode(500);
        }
    }
}