using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace LoggingApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{


    public const string CorrelationIdHeaderConstant = "x-correlation-id";
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherContext _dbContext = new WeatherContext();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]

    public IEnumerable<string> Get()
    {
        var correlationId = GetCorrelationId();
        return _dbContext.Weathers.TagWith(correlationId).Select(w => w.Name);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var correlationId = GetCorrelationId();
        try
        {
            var weather = _dbContext.Weathers.TagWith(correlationId).First(w => w.Id == id)?.Name;
            return weather != null ? Ok(weather) : NotFound(); ;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return StatusCode(500, "Error occured, please look at the correlation id!");
        }
    }

    private string GetCorrelationId()
    {
        var correlationHeader = new StringValues();
        _httpContextAccessor.HttpContext?.Response.Headers.TryGetValue(CorrelationIdHeaderConstant, out correlationHeader);
        return correlationHeader.FirstOrDefault() ?? string.Empty;
    }
}
