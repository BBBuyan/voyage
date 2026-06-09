using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VoyageManager.Infrastructure.Database;

internal class DesignTimeDbContext : IDesignTimeDbContextFactory<VoyageManagerDbContext>
{
    public VoyageManagerDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder opts = new();
        opts.UseNpgsql(
            "host=localhost;port=5432;database=voyager;username=postgres;password=asdf"
        );

        return new(opts.Options);
    }
}
