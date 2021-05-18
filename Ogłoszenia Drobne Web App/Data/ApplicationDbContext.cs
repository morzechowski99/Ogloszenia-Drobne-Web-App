using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ogłoszenia_Drobne_Web_App.Models;
using System.Linq;

namespace Ogłoszenia_Drobne_Web_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(builder);
            builder.Entity<Offer>(entity =>
            {
                entity.Property(e => e.Wage).HasColumnType("decimal(19,2)");
            });
            builder.Entity<OfferAtribute>(entity =>
            {
                entity.HasKey(e => new { e.AtributeId, e.OfferId });
            });
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Atribute> Atributes { get; set; }
        public virtual DbSet<BlackWord> BlackWords { get; set; }
        public virtual DbSet<OfferAtribute> OfferAtributes { get; set; }
        public virtual DbSet<OfferReport> OfferReports { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Alert> Alerts { get; set; }
        public virtual DbSet<DateTimeAtribute> DateTimeAtributes { get; set; }
        public virtual DbSet<DoubleAtribute> DoubleAtributes { get; set; }
        public virtual DbSet<NumberAtribute> NumberAtributes { get; set; }
        public virtual DbSet<TextAtribute> TextAtributes { get; set; }

    }
}