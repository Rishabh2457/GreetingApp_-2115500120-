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
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

    //  Database connection
    var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<HelloGreetingDbContext>(options => options.UseSqlServer(connectionString));

    //  Configure Redis
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var redisConfig = builder.Configuration.GetSection("Redis")["ConnectionString"] ?? "localhost:6379";
        return ConnectionMultiplexer.Connect(redisConfig);
    });
    builder.Services.AddSingleton<RedisCacheService>();

    //  Configure JWT Authentication BEFORE app.Build()
    var jwt = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwt["SecretKey"]);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

    builder.Services.AddAuthorization();

    //  BUILD NOW
    var app = builder.Build();

    //  Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication(); 
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
