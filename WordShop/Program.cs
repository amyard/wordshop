using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WordShop.Data;
using WordShop.Enums;
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
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                    // add migrations if db does not exists
                    await context.Database.MigrateAsync();

                    await GenerateTariffs(context, loggerFactory);
                    await GenerateNewRoles(roleManager, loggerFactory);
                    await GenerateNewUsers(userManager, roleManager, loggerFactory);
                    
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

        private static async Task GenerateNewUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var admin = new IdentityUser {UserName = "delme", Email = "blackgurder1987@gmail.com"};
                    var moderator = new IdentityUser {UserName = "delme_2", Email = "blackgurder1987_2@gmail.com"};
                    var user = new IdentityUser {UserName = "delme_3", Email = "blackgurder1987_3@gmail.com"};
                    
                    await userManager.CreateAsync(admin, "Admin123*");
                    await userManager.AddToRolesAsync(admin, new[] {AppRoles.Admin.ToString(), AppRoles.Moderator.ToString()});
                    
                    await userManager.CreateAsync(moderator, "Admin123*");
                    await userManager.AddToRolesAsync(moderator, new[] {AppRoles.Moderator.ToString()});
                    
                    await userManager.CreateAsync(user, "Admin123*");
                    await userManager.AddToRolesAsync(user, new[] {AppRoles.WordShopBeginner.ToString()});
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during inserting roles.");
            }
        }

        private static async Task GenerateNewRoles(RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!roleManager.Roles.Any())
                {
                    var roles = new List<IdentityRole>
                    {
                        new () {Name = AppRoles.Admin.ToString(), NormalizedName = AppRoles.Admin.ToString()},
                        new () {Name = AppRoles.Moderator.ToString(), NormalizedName = AppRoles.Moderator.ToString()},
                        new () {Name = AppRoles.WordShopBeginner.ToString(), NormalizedName = AppRoles.WordShopBeginner.ToString()},
                        new () {Name = AppRoles.Default.ToString(), NormalizedName = AppRoles.Default.ToString()}
                    };
                    
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during inserting roles.");
            }
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
