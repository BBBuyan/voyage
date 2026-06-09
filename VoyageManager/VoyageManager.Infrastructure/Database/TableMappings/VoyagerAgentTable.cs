using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.TableMappings;

public class VoyagerAgentTable : IEntityTypeConfiguration<VoyagerAgent>
{
    public void Configure(EntityTypeBuilder<VoyagerAgent> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(100);

        builder
            .Property(x => x.PasswordHash)
            .HasMaxLength(200);

        builder
            .HasMany(x => x.GroupAssignments)
            .WithOne(x => x.VoyagerAgent)
            .HasForeignKey(x => x.VoyagerAgentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.CommandAssignments)
            .WithOne(x => x.VoyagerAgent)
            .HasForeignKey(x => x.VoyagerAgentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
