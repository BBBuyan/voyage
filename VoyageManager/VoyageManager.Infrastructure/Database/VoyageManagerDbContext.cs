using Microsoft.EntityFrameworkCore;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database;

public class VoyageManagerDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<VoyagerAgent> VoyagerAgents { get; set; }

    public DbSet<VoyagerGroup> VoyagerGroups { get; set; }

    public DbSet<VoyagerCommand> VoyagerCommands { get; set; }

    public DbSet<VoyagerGroupAssignment> VoyagerGroupAssignments { get; set; }

    public DbSet<VoyagerCommandAssignment> VoyagerCommandAssignments { get; set; }

    public VoyageManagerDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("voyage");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoyageManagerDbContext).Assembly);
    }
}
