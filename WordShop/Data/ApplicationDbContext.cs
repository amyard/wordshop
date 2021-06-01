using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WordShop.Models;

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
        }
    }
}
