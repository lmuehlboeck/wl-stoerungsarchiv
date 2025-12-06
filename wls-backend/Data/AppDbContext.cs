using Microsoft.EntityFrameworkCore;
using wls_backend.Models.Domain;
using wls_backend.Models.DTOs;
using wls_backend.Models.Enums;

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
                .HasKey(e => new { e.DisturbanceId, e.Text, e.CreatedAt });
            builder.Entity<DisturbanceDescription>()
                .Property(d => d.CreatedAt)
                .HasColumnType("timestamp without time zone");

            builder.Entity<Subscription>()
                .HasKey(s => new { s.DeviceId, s.LineId });
            builder.Entity<Subscription>()
                .HasOne(s => s.Device)
                .WithMany(sub => sub.Subscriptions)
                .HasForeignKey(s => s.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Subscription>()
                .HasOne(s => s.Line)
                .WithMany(l => l.Subscriptions)
                .HasForeignKey(s => s.LineId);
            builder.Entity<Device>()
                .HasIndex(s => s.Token)
                .IsUnique();
        }

        public DbSet<Disturbance> Disturbance { get; set; }
        public DbSet<DisturbanceDescription> DisturbanceDescription { get; set; }
        public DbSet<Line> Line { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public IQueryable<Disturbance> DisturbanceWithAll => Disturbance
            .Include(d => d.Descriptions.OrderBy(desc => desc.CreatedAt))
            .Include(d => d.Lines);

        public readonly Func<Line, int> LineOrderSelector = l =>
        {
            try
            {
                return int.Parse(string.Concat(l.Id.Where(char.IsDigit)));
            }
            catch
            {
                return int.MinValue;
            }
        };
        
        public IQueryable<Disturbance> DisturbanceFiltered(DisturbanceFilter filter)
        {
            var fromDate = filter.FromDate.ToDateTime(TimeOnly.MinValue);
            var toDate = filter.ToDate.ToDateTime(TimeOnly.MaxValue);
            var query = DisturbanceWithAll.Where(d => (d.StartedAt >= fromDate && d.StartedAt <= toDate)
                                                || ((d.EndedAt ?? DateTime.Now) >= fromDate &&
                                                    (d.EndedAt ?? DateTime.Now) <= toDate));

            if (!string.IsNullOrWhiteSpace(filter.Lines))
            {
                var lines = filter.Lines.Split(',').Select(l => l.Trim()).ToList();
                query = query.Where(d => d.Lines.Any(l => lines.Contains(l.Id)));
            }

            if (!string.IsNullOrWhiteSpace(filter.Types))
            {
                var types = filter.Types.Split(',')
                    .Select(t => Enum.Parse<DisturbanceType>(t.Trim())).ToList();
                query = query.Where(d => types.Contains(d.Type));
            }

            if (filter.OnlyActive)
            {
                query = query.Where(d => d.EndedAt == null);
            }

            return filter.OrderBy switch
            {
                OrderType.StartedAtAsc => query.OrderBy(d => d.StartedAt),
                OrderType.StartedAtDesc => query.OrderByDescending(d => d.StartedAt),
                OrderType.EndedAtAsc => query.OrderBy(d => d.EndedAt),
                OrderType.EndedAtDesc => query.OrderByDescending(d => d.EndedAt),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
