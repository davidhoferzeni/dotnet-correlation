using Microsoft.EntityFrameworkCore;

public class WeatherContext : DbContext
{
     public WeatherContext(DbContextOptions<WeatherContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Weather>().HasData(new Weather { Id= 1, Name= "Sunny" });
        modelBuilder.Entity<Weather>().HasData(new Weather { Id= 2, Name= "Rainy" });
        modelBuilder.Entity<Weather>().HasData(new Weather { Id= 3, Name= "Cloudy" });
    }

    public DbSet<Weather> Weathers { get; set; }

}

public class Weather
{
    public int Id { get; set; }
    public string Name { get; set; }
}