using Microsoft.EntityFrameworkCore;
using Serilog;

public class WeatherContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Weather>().HasData(new Weather { Id= 1, Name= "Sunny" });
        modelBuilder.Entity<Weather>().HasData(new Weather { Id= 2, Name= "Rainy" });
        modelBuilder.Entity<Weather>().HasData(new Weather { Id= 3, Name= "Cloudy" });
    }

    public DbSet<Weather> Weathers { get; set; }
    public string ConnectionString { get; }

    public WeatherContext()
    {
        ConnectionString = "Server=RATV-NBHOF21;Initial Catalog=WeatherDb;Persist Security Info=False;trusted_connection=yes;TrustServerCertificate=True;Connection Timeout=30;";
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(ConnectionString)
        .LogTo(Log.Logger.Information, LogLevel.Information, null);
}

public class Weather
{
    public int Id { get; set; }
    public string Name { get; set; }
}