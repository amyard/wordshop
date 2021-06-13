using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WordShop.Models;
using WordShop.Models.CustomerInfo;
using WordShop.Models.Tariff;

namespace WordShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerInfo> CustomerInfos { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<CourseStart> CourseStarts { get; set; }
        public DbSet<TariffBenefit> TariffBenefits { get; set; }
        public DbSet<TariffBenefitOrdered> TariffBenefitOrdered { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomerInfo>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            
            builder.Entity<CustomerInfo>()
                .HasOne(c => c.Tariff)
                .WithMany(t => t.CustomerInfos)
                .HasForeignKey(c => c.TariffId)
                .HasPrincipalKey(t => t.Id);

            builder.Entity<TariffBenefitOrdered>()
                .HasOne(p => p.TariffBenefit)
                .WithMany(t => t.TariffBenefitOrdered)
                .HasForeignKey(t => t.TariffBenefitId)
                .HasPrincipalKey(t => t.Id);

            builder.Entity<TariffBenefitOrdered>()
                .HasOne(t => t.AdvantageTariff)
                .WithMany(t => t.Advantage)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<TariffBenefitOrdered>()
                .HasOne(t => t.DisadvantageTariff)
                .WithMany(t => t.Disadvantage)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
