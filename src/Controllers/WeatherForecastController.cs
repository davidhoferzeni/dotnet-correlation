using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LoggingApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    public const string CorrelationIdHeaderConstant = "x-correlation-id";
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherContext _dbContext;
    private readonly ICorrelationProvider _correlationProvider;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ICorrelationProvider correlationProvider, WeatherContext dbContext)
    {
        _logger = logger;
        _correlationProvider = correlationProvider;
        _dbContext = dbContext;
    }

    [HttpGet]

    public IEnumerable<string> Get()
    {
        var correlationId = _correlationProvider.GetCorrelationId();
        return _dbContext.Weathers.TagWith(correlationId ?? "none").Select(w => w.Name);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var correlationId = _correlationProvider.GetCorrelationId();
        try
        {
            var weather = _dbContext.Weathers.TagWith(correlationId ?? "none").First(w => w.Id == id)?.Name;
            return weather != null ? Ok(weather) : NotFound(); ;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, "Error occured, please look at the correlation id!");
        }
    }
}
