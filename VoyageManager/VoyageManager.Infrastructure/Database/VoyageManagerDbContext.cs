using Microsoft.EntityFrameworkCore;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database;

public class VoyageManagerDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<Worker> Workers { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<GroupAssignment> GroupAssignments { get; set; }

    public DbSet<CommandAssignment> CommandAssignments { get; set; }

    public VoyageManagerDbContext(DbContextOptions<VoyageManagerDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("voyage");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoyageManagerDbContext).Assembly);
    }
}
