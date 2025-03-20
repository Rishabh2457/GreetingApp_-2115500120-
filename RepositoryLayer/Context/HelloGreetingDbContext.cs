using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;

public class HelloGreetingDbContext : DbContext
{
    public DbSet<GetGreetingMessage> GetGreetingMessages { get; set; }

    public HelloGreetingDbContext(DbContextOptions<HelloGreetingDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
}

