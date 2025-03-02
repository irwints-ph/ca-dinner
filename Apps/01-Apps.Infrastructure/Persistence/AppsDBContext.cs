using Apps.Domain.MenuAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Apps.Infrastructure.Persistence;

public class AppsDBContext : DbContext
{
  public AppsDBContext(DbContextOptions<AppsDBContext> opt)
      :base(opt)
  {
  }
  public DbSet<Menu> Menus { get; set; } = null!;
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
      .ApplyConfigurationsFromAssembly(typeof(AppsDBContext).Assembly);

    // modelBuilder.Model.GetEntityTypes()
    //   .SelectMany(e => e.GetProperties())
    //   .Where(p => p.IsPrimaryKey())
    //   .ToList()
    //   .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

    base.OnModelCreating(modelBuilder);
  }
}

