using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WordShop.Data;
using WordShop.Models;

namespace WordShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    // add migrations if db does not exists
                    await context.Database.MigrateAsync();

                    await GenerateTariffs(context, loggerFactory);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migrations.");
                }
            }
            
            host.Run();
        }

        private static async Task GenerateTariffs(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Tariffs.Any())
                {
                    var tariffs = new List<Tariff>
                    {
                        new() {Name = "Bronze", NewPrice = 2, OldPrice = 4},
                        new() {Name = "Silver", NewPrice = 5, OldPrice = 9},
                        new() {Name = "Gold", NewPrice = 10, OldPrice = 18}
                    };
                
                    tariffs.ForEach(t => context.Tariffs.AddAsync(t));
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during inserting tariefs.");
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
