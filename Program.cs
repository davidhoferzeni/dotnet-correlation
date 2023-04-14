using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId} {RequestId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICorrelationProvider, HttpCorrelationProvider>();
var connectionString = "Server=RATV-NBHOF21;Initial Catalog=WeatherDb;Persist Security Info=False;trusted_connection=yes;TrustServerCertificate=True;Connection Timeout=30;";
builder.Services.AddDbContext<WeatherContext>(options =>
    options.UseSqlServer(connectionString));

builder.Host.UseSerilog();


var app = builder.Build();

app.UseMiddleware<CorrelationIdHeaderMiddleware>();

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
