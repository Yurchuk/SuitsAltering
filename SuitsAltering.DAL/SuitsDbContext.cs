using Microsoft.EntityFrameworkCore;
using SuitsAltering.DAL.Entities;

namespace SuitsAltering.DAL;

public class SuitsDbContext: DbContext
{
    public SuitsDbContext(DbContextOptions<SuitsDbContext> options)
        : base((DbContextOptions)options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SuitsDbContext).Assembly);
    }

    public DbSet<Altering> Alterings { get; set; }
}