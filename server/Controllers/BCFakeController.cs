using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace test_api.Controllers;

[ApiController]
[Route("[controller]")]
public class BCFakeController : ControllerBase
{
    private readonly ILogger<BCFakeController> _logger;

    public BCFakeController(ILogger<BCFakeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation($"Get called: {Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
        return Ok("Hello World!");
    }
}
