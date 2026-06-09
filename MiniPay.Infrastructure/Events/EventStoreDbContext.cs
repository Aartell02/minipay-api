using Microsoft.EntityFrameworkCore;

namespace MiniPay.Infrastructure.Events
{
    public class EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : DbContext(options)
    {
        public DbSet<EventEntity> Events => Set<EventEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.EventType).HasMaxLength(200);
                e.HasIndex(x => x.TransactionId);
            });
        }
    }
}
