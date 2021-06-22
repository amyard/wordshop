using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordShop.Data;
using WordShop.Data.Interfaces;
using WordShop.Data.Repositories;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerInfoRepository, CustomerInfoRepository>();
            services.AddScoped<ITariffRepository, TariffRepository>();
            services.AddScoped<ITelegramRepository, TelegramRepository>();
            services.AddScoped<ICourseStartRepository, CourseStartRepository>();
            services.AddScoped<ITariffBenefitRepository, TariffBenefitRepository>();
            services.AddScoped<ITariffBenefitOrderedRepository, TariffBenefitOrderedRepository>();
            services.AddScoped<IDayInfoRepository, DayInfoRepository>();
            services.AddScoped<IDayInfoBlockRepository, DayInfoBlockRepository>();
            services.AddScoped<IDayInfoSequenceItemRepository, DayInfoSequenceItemRepository>();

            services.Configure<TelegramSettingsModel>(Configuration.GetSection("TelegramSettings"));
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole(AppRoles.Admin.ToString()));
                opt.AddPolicy("RequireModeratorPlusRole", policy => policy.RequireRole(AppRoles.Admin.ToString(), AppRoles.Moderator.ToString()));
                opt.AddPolicy("RequireWordShopBeginnerRole", policy => policy.RequireRole(AppRoles.WordShopBeginner.ToString()));
            });
            
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
