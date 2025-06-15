using Microsoft.EntityFrameworkCore;
using wls_backend.Models.Domain;

namespace wls_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Disturbance>()
                .Property(d => d.StartedAt)
                .HasColumnType("timestamp without time zone");
            builder.Entity<Disturbance>()
                .Property(d => d.EndedAt)
                .HasColumnType("timestamp without time zone");

            builder.Entity<DisturbanceDescription>()
                .HasKey(e => new { e.DisturbanceId, e.Text });
            builder.Entity<DisturbanceDescription>()
                .Property(d => d.CreatedAt)
                .HasColumnType("timestamp without time zone");
        }

        public DbSet<Disturbance> Disturbance { get; set; }
        public DbSet<DisturbanceDescription> DisturbanceDescription { get; set; }
        public DbSet<Line> Line { get; set; }

        public IQueryable<Disturbance> DisturbanceWithAll => Disturbance
            .Include(d => d.Descriptions.OrderBy(desc => desc.CreatedAt))
            .Include(d => d.Lines);
    }
}
