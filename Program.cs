using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    //.Enrich.WithCorrelationIdHeader()
    .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId} {RequestId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICorrelationProvider, HttpCorrelationProvider>();

builder.Host.UseSerilog();

Log.Logger.Information("Starting db migration");

var dbContext = new WeatherContext();
dbContext.Database.Migrate();

var app = builder.Build();

app.UseMiddleware<CorrelationIdHeaderMiddleware>();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseHttpLogging();
app.Run();
