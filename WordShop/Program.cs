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
using WordShop.Models.Tariff;

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

                    await GenerateCourseStartDate(context, loggerFactory);
                    await GenerateTariffs(context, loggerFactory);
                    await GenerateTariffBenefits(context, loggerFactory);
                    await GenerateOrderedTariffBenefits(context, loggerFactory);
                    
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

        private static async Task GenerateCourseStartDate(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.CourseStarts.Any())
                {
                    await context.CourseStarts.AddAsync(new CourseStart {Courses = Courses.WordShop, CourseStartDate = DateTime.Now.AddDays(14)});
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during inserting tariff benefits.");
            }
        }

        private static async Task GenerateOrderedTariffBenefits(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.TariffBenefitOrdered.Any())
                {
                    var tariffBenefitOrdered = new List<TariffBenefitOrdered>
                    {
                        new() {OrderPosition = 1, TariffBenefitId = 1, AdvantageTariffId = 2, DisadvantageTariffId = 1},
                        new() {OrderPosition = 2, TariffBenefitId = 2, AdvantageTariffId = 2, DisadvantageTariffId = 1},
                        new() {OrderPosition = 3, TariffBenefitId = 3, AdvantageTariffId = 2, DisadvantageTariffId = 1},
                        new() {OrderPosition = 4, TariffBenefitId = 4, AdvantageTariffId = 1, DisadvantageTariffId = 2},
                        new() {OrderPosition = 5, TariffBenefitId = 5, AdvantageTariffId = 1, DisadvantageTariffId = 2},
                        new() {OrderPosition = 6, TariffBenefitId = 6, AdvantageTariffId = 1, DisadvantageTariffId = 2},
                        new() {OrderPosition = 7, TariffBenefitId = 7, AdvantageTariffId = 1, DisadvantageTariffId = 2},
                        new() {OrderPosition = 8, TariffBenefitId = 8, AdvantageTariffId = 1, DisadvantageTariffId = 2},
                        
                        new() {OrderPosition = 1, TariffBenefitId = 1, AdvantageTariffId = 3, DisadvantageTariffId = 1},
                        new() {OrderPosition = 2, TariffBenefitId = 2, AdvantageTariffId = 3, DisadvantageTariffId = 1},
                        new() {OrderPosition = 3, TariffBenefitId = 3, AdvantageTariffId = 3, DisadvantageTariffId = 1},
                        new() {OrderPosition = 4, TariffBenefitId = 4, AdvantageTariffId = 3, DisadvantageTariffId = 1},
                        new() {OrderPosition = 5, TariffBenefitId = 5, AdvantageTariffId = 3, DisadvantageTariffId = 1},
                        new() {OrderPosition = 6, TariffBenefitId = 6, AdvantageTariffId = 3, DisadvantageTariffId = 1},
                        new() {OrderPosition = 7, TariffBenefitId = 7, AdvantageTariffId = 3, DisadvantageTariffId = 1},
                        new() {OrderPosition = 8, TariffBenefitId = 8, AdvantageTariffId = 1, DisadvantageTariffId = 3},
                        
                        new() {OrderPosition = 1, TariffBenefitId = 1, AdvantageTariffId = 4, DisadvantageTariffId = 1},
                        new() {OrderPosition = 2, TariffBenefitId = 2, AdvantageTariffId = 4, DisadvantageTariffId = 1},
                        new() {OrderPosition = 3, TariffBenefitId = 3, AdvantageTariffId = 4, DisadvantageTariffId = 1},
                        new() {OrderPosition = 4, TariffBenefitId = 4, AdvantageTariffId = 4, DisadvantageTariffId = 1},
                        new() {OrderPosition = 5, TariffBenefitId = 5, AdvantageTariffId = 4, DisadvantageTariffId = 1},
                        new() {OrderPosition = 6, TariffBenefitId = 6, AdvantageTariffId = 4, DisadvantageTariffId = 1},
                        new() {OrderPosition = 7, TariffBenefitId = 7, AdvantageTariffId = 4, DisadvantageTariffId = 1},
                        new() {OrderPosition = 8, TariffBenefitId = 8, AdvantageTariffId = 4, DisadvantageTariffId = 1}
                    };
                
                    tariffBenefitOrdered.ForEach(t => context.TariffBenefitOrdered.AddAsync(t));
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during inserting ordered tariff benefits.");
            }
        }

        private static async Task GenerateTariffBenefits(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.TariffBenefits.Any())
                {
                    var tariffBenefits = new List<TariffBenefit>
                    {
                        new() { Benefit = "Аудио и pdf-файлы с текстом  рассказа"},
                        new() { Benefit = "Видеоуроки с грамматикой"},
                        new() { Benefit = "Интерактивные игры со словами"},
                        new() { Benefit = "Доступ к специальному чату с участниками"},
                        new() { Benefit = "Ответы на вопросы в чате от Анны"},
                        new() { Benefit = "Проверка домашних заданий"},
                        new() { Benefit = "Возможность получить бонусный урок про знакомство с иностранцами"},
                        new() { Benefit = "1,5 часовое индивидуальное занятие с Анной"}
                    };
                
                    tariffBenefits.ForEach(t => context.TariffBenefits.AddAsync(t));
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during inserting tariff benefits.");
            }
        }

        private static async Task GenerateNewUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var admin = new IdentityUser {UserName = "delme", Email = "blackguarder1987@gmail.com", EmailConfirmed = true};
                    var moderator = new IdentityUser {UserName = "delme_2", Email = "blackguarder1987_2@gmail.com", EmailConfirmed = true};
                    var user = new IdentityUser {UserName = "delme_3", Email = "blackguarder1987_3@gmail.com", EmailConfirmed = true};
                    
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
                        new() {Name = "Default", NewPrice = 1, OldPrice = 4, Courses = 0, Level = 0},
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
