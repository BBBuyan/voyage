using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.TableMappings;

public class VoyagerCommandAssignmentTable : IEntityTypeConfiguration<CommandAssignment>
{
    public void Configure(EntityTypeBuilder<CommandAssignment> builder)
    {
        builder
            .HasIndex(x => new { x.WorkerId })
            .IsUnique();

        builder
            .HasIndex(x => new { x.WorkerId, x.Status });

        builder
            .Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired(true);

        builder
            .Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();
    }
}
