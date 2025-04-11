using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;

namespace test_api.Controllers;

[ApiController]
[Route("[controller]")]
public class BCFakeController : ControllerBase
{
    private readonly ILogger<BCFakeController> _logger;
    private readonly IConfiguration _configuration;
    private static int _counter = 0;

    public BCFakeController(ILogger<BCFakeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("{duration}/dev/metadata")]
    [Produces("application/json")]
    public IActionResult Get(string duration)
    {
        _counter++;
        var localCounter = _counter;
        _logger.LogInformation($"Get called ({localCounter}): {Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}, identified duration: {duration}");

        var response = new
        {
            runtimeVersion = _configuration.GetValue<string>("runtimeVersion"),
            webApiVersion = _configuration.GetValue<string>("webApiVersion", "6.0"),
            debuggerVersion = _configuration.GetValue<string>("debuggerVersion", "6.0"),
            webEndpoint = $"https://{Request.Host}/{_configuration.GetValue<string>("externalBasepath")}{duration}",
            extensionAllowedTargetLevel = "Internal"
        };

        HttpContext.Response.Headers.ContentLength = Encoding.UTF8.GetByteCount(JsonSerializer.Serialize(response));

        _logger.LogInformation($"Get finished ({localCounter})");

        return Ok(response);
    }

    [HttpPost("{duration}/dev/apps")]
    public async Task<IActionResult> Post(string duration)
    {
        _counter++;
        var localCounter = _counter;
        _logger.LogInformation($"Post called ({localCounter}): {Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}, identified duration: {duration}");
        await HandleWait(duration, localCounter);
        return Ok();
    }

    [HttpGet("{duration}/dev/packages")]
    [Produces("application/octet-stream")]
    public IActionResult GetFile(string duration)
    {
        var filePath = "/app/Microsoft_Application_25.1.0.0.app";
        var stream = new FileStream(filePath, FileMode.Open);
        return File(stream, "application/octet-stream", $"BCFake-{Guid.NewGuid()}.app");
    }

    private async Task HandleWait(string duration, int localCounter)
    {
        int durationInSeconds = 0;
        if (!string.IsNullOrEmpty(duration) && int.TryParse(duration, out int result))
            durationInSeconds = result;

        _logger.LogInformation($"Waiting for {durationInSeconds} seconds(s)");
        await Task.Delay(durationInSeconds * 1000);

        _logger.LogInformation($"Done waiting for {durationInSeconds} second(s) ({localCounter})");
    }
}