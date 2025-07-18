using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Mapping
{
    public class ActivityLogMapping : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.ToTable("ACTIVITY_LOGS");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(x => x.EventType)
                .HasColumnName("event_type")
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.Entity)
                .HasColumnName("entity")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(x => x.EntityId)
                .HasColumnName("entity_id")
                .IsRequired();

            builder.Property(x => x.EventTime)
                .HasColumnName("event_time")
                .HasConversion(v => DateTime.SpecifyKind(v, DateTimeKind.Utc), v => v)
                .IsRequired();
        }
    }
}
