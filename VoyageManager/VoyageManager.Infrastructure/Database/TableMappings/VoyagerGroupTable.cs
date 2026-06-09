using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.TableMappings;

public class VoyagerGroupTable : IEntityTypeConfiguration<VoyagerGroup>
{
    public void Configure(EntityTypeBuilder<VoyagerGroup> builder)
    {
        builder
            .HasMany(x => x.GroupAssignments)
            .WithOne(x => x.VoyagerGroup)
            .HasForeignKey(x => x.VoyagerGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.Name)
            .HasMaxLength(100);
    }
}
