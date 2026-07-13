using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.TableMappings;

public class WorkerTable : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100);

        builder.Property(x => x.PasswordHash).HasMaxLength(200);

        builder
            .HasMany(x => x.GroupAssignments)
            .WithOne(x => x.AssignedWorker)
            .HasForeignKey(x => x.WorkerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.CommandAssignments)
            .WithOne(x => x.AssignedWorker)
            .HasForeignKey(x => x.WorkerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
