using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.TableMappings;

public class VoyagerGroupAssignmentTable : IEntityTypeConfiguration<VoyagerGroupAssignment>
{
    public void Configure(EntityTypeBuilder<VoyagerGroupAssignment> builder)
    {
        builder
            .HasIndex(x => new { x.VoyagerGroupId, x.VoyagerAgentId })
            .IsUnique();
    }
}
