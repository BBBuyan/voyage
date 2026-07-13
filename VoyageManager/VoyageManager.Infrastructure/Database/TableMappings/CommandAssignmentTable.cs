using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.TableMappings;

public class CommandAssignmentTable : IEntityTypeConfiguration<CommandAssignment>
{
    public void Configure(EntityTypeBuilder<CommandAssignment> builder)
    {
        builder.HasIndex(x => new { x.WorkerId, x.State });

        builder.Property(x => x.State).HasConversion<string>().HasMaxLength(100).IsRequired(true);

        builder.Property(x => x.CommandType).HasConversion<string>().HasMaxLength(100).IsRequired();
    }
}
