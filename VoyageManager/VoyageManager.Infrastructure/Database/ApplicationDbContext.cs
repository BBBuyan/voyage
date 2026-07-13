using Microsoft.EntityFrameworkCore;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<Worker> Workers { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<GroupAssignment> GroupAssignments { get; set; }

    public DbSet<CommandAssignment> CommandAssignments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("voyage");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
