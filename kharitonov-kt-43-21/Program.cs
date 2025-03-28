using kharitonov_kt_43_21.Database;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();  // Ensure NLog is used

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<StudentDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
}
finally
{
    LogManager.Shutdown();
}
