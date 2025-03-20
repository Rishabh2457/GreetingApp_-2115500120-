using NLog;
using NLog.Web;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using StackExchange.Redis;
using BusinessLayer.Services;
using RepositoryLayer.Services;
using RepositoryLayer.Hashing;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Application Starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // 🔹 Configure NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // 🔹 Add Controllers & Swagger
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // 🔹 Register Repository and Business Layer
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();
    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<IUserRL, UserRL>();
    builder.Services.AddScoped<Password_Hash>();

    // 🔹 Database connection
    var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<HelloGreetingDbContext>(options => options.UseSqlServer(connectionString));

    // 🔹 Configure Redis (remove duplicate registration)
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        try
        {
            var redisConfig = builder.Configuration.GetSection("Redis")["ConnectionString"] ?? "localhost:6379";
            return ConnectionMultiplexer.Connect(redisConfig);
        }
        catch (Exception e)
        {
            Console.WriteLine("Redis connection failed: " + e.Message);
            throw;
        }
    });
    builder.Services.AddSingleton<RedisCacheService>();

    var app = builder.Build();

    // 🔹 Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to an exception.");
    throw;
}
finally
{
    LogManager.Shutdown();
}