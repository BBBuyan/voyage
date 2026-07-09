using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.TableMappings;

public class VoyagerCommandTable : IEntityTypeConfiguration<VoyagerCommand>
{
    public void Configure(EntityTypeBuilder<VoyagerCommand> builder)
    {
        builder
            .Property(x => x.CommandType)
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasMany(x => x.CommandAssigments)
            .WithOne(x => x.VoyagerCommand)
            .HasForeignKey(x => x.VoyagerCommandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
