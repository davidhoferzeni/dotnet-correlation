using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var  corsPolicy = "_AnyOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                            .WithExposedHeaders("x-correlation-id");
                      });
});

var connectionString = "Server=RATV-NBHOF21;Initial Catalog=WeatherDb;Persist Security Info=False;trusted_connection=yes;TrustServerCertificate=True;Connection Timeout=30;";
builder.Services.AddDbContext<WeatherContext>(options =>
    options.UseSqlServer(connectionString));

builder.Host.UseSerilog();
builder.Host.UseCorrelationProvider();
var app = builder.Build();
app.UseRequestCorrealtion();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(corsPolicy);

app.MapControllers();
app.Run();
