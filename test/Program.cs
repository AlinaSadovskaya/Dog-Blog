using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using test.Domain.Core;
using test.Services.BusinessLogic;
using test.Domain.Interfaces;
using test.Infrastructure.Data;
using test.ViewModels;
using Serilog;
using Serilog.Events;


namespace test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var config = services.GetRequiredService<IConfiguration>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.File(config["AllLogs"])
                    .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                    .WriteTo.File(config["ErrorLogs"]))
                    .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                    .WriteTo.Console())
                    .CreateLogger();

                    await test.Infrastructure.Data.RoleInitializer.InitializeAsync(userManager, rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
            .UseSerilog();
    }
}