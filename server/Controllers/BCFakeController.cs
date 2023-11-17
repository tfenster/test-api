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

    public BCFakeController(ILogger<BCFakeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("dev/metadata")]
    [Produces("application/json")]
    public IActionResult Get()
    {
        _logger.LogInformation($"Get called: {Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");

        var response = new
        {
            runtimeVersion = "11.0",
            webApiVersion = "6.0",
            debuggerVersion = "6.0",
            webEndpoint = $"https://{Request.Host}/{_configuration.GetValue<string>("externalBasepath")}",
            extensionAllowedTargetLevel = "Internal"
        };

        HttpContext.Response.Headers.ContentLength = Encoding.UTF8.GetByteCount(JsonSerializer.Serialize(response));

        return Ok(response);
    }

    [HttpPost("dev/apps")]
    public async Task<IActionResult> Post()
    {
        _logger.LogInformation($"Post called: {Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
        _logger.LogInformation($"Waiting for {_configuration.GetValue<int>("waitTimeInMinutes")} minute(s)");
        await Task.Delay(_configuration.GetValue<int>("waitTimeInMinutes") * 60 * 1000);
        _logger.LogInformation($"Done waiting for {_configuration.GetValue<int>("waitTimeInMinutes")} minute(s)");
        return Ok();
    }

    [HttpGet("dev/packages")]
    [Produces("application/octet-stream")]
    public IActionResult GetFile()
    {
        var filePath = "/app/Microsoft_Application_22.2.0.0.app";
        var stream = new FileStream(filePath, FileMode.Open);
        return File(stream, "application/octet-stream", $"BCFake-{Guid.NewGuid()}.app");
    }
}