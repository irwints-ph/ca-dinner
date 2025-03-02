# EF Core DDD and Clean Architecture  Mapping Aggregates to Relational Databases

[Back][1]

### DDD & EF Core - Enforcing DDD Principles

### 3 Steps for mapping an aggregate to a relational database
1. Add EF Core Package
```bash
  dotnet add Apps\01-Apps.Infrastructure package Microsoft.EntityFrameworkCore --version 8.0.11
```
2. Create [DBContext Class]
```cs
using Apps.Domain.MenuAggregate;
using Microsoft.EntityFrameworkCore;

namespace Apps.Infrastructure.Persistence;

public class AppsDBContext : DbContext
{
  public AppsDBContext(DbContextOptions<AppsDBContext> opt)
      :base(opt)
  {
  }
  public DbSet<Menu> Menus { get; set; } = null!;
  
}

```

3. Add DB Driver
```bash
  dotnet add Apps\01-Apps.Infrastructure package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.11
```

4. Write the DB Context to [DI of Infrastructure Project][33]
```csharp
  public static IServiceCollection AddPersistance(this IServiceCollection services)
  {
    services.AddDbContext<AppsDBContext>(opt =>
      opt.UseSqlite()
    );
    ...
  }
```
5. Create Repository folder and move the Repository Class to that folder of the Infrastructure Project
6. Change in memory to DB on the [Menu Repository][3]
```csharp
    private readonly AppsDBContext _dbContext;

    public MenuRepository(AppsDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Menu menu)
    {
        _dbContext.Add(menu);
        _dbContext.SaveChanges();
    }

```
#### Configuring the Mapping

1. Create Menu Configuration Class[4]
```csharp
using System.Data;
using System.Data.Common;
using Apps.Domain.HostAggregate.ValueObjects;
using Apps.Domain.MenuAggregate;
using Apps.Domain.MenuAggregate.Entities;
using Apps.Domain.MenuAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apps.Infrastructure.Persistence.Configurations;
public class MenuConfigurations : IEntityTypeConfiguration<Menu>
{
  public void Configure(EntityTypeBuilder<Menu> builder)
  {
    ConfigureMenuTable(builder);
    ConfigureMenuSectionsTable(builder);
    ConfigureMenuDinnerIdsTable(builder);
    ConfigureMenuReviewIdsTable(builder);
  }

    private void ConfigureMenuDinnerIdsTable(EntityTypeBuilder<Menu> builder)
    {
      builder.OwnsMany(m => m.DinnerIds, d => {
        d.ToTable("MenusDinnerIds");
        d.WithOwner().HasForeignKey("MenuId");
        d.HasKey("Id");
        d.Property(dv => dv.Value)
          .HasColumnName("DinnerId")
          .ValueGeneratedNever();
      });
      //Set the actual Values for the DinnerIds - To Populate under lying List
      builder.Metadata.FindNavigation(nameof(Menu.DinnerIds))!
        .SetPropertyAccessMode(PropertyAccessMode.Field);

    }

    private void ConfigureMenuReviewIdsTable(EntityTypeBuilder<Menu> builder)
    {
      builder.OwnsMany(m => m.MenuReviewIds, r => {
        r.ToTable("MenusReviewIds");
        r.WithOwner().HasForeignKey("MenuId");
        r.HasKey("Id");
        r.Property(dv => dv.Value)
          .HasColumnName("ReviewId")
          .ValueGeneratedNever();
      });
      //Set the actual Values for the MenuReviewIds - To Populate under lying List
      builder.Metadata.FindNavigation(nameof(Menu.MenuReviewIds))!
        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

  private void ConfigureMenuSectionsTable(EntityTypeBuilder<Menu> builder)
  {
    builder.OwnsMany(m => m.Sections, sb => {
      sb.ToTable("MenuSections");

      //Foregn Key
      sb.WithOwner().HasForeignKey("MenuId");

      //Primary Key: Used Constant Since MenuId is not accesisble thru type
      sb.HasKey("Id", "MenuId");
      
      sb.Property(m => m.Id)
        .HasColumnName("MenuSectionId")  //DB Column Name
        .ValueGeneratedNever()          //To for not creating id (AutoKey)
        .HasConversion(
          id => id.Value,               //Going to the DB
          value => MenuSectionId.Create(value) //From DB
        );

      sb.Property(m => m.Name)
        .HasMaxLength(100);
      sb.Property(m => m.Description)
        .HasMaxLength(100);

      sb.OwnsMany(m => m.Items, mi =>{
        //Foregn Key
        mi.WithOwner().HasForeignKey("MenuSectionId", "MenuId");
        mi.HasKey(nameof(MenuItem.Id), "MenuId", "MenuSectionId");

        mi.Property(m => m.Id)
          .HasColumnName("MenuItemId")  //DB Column Name
          .ValueGeneratedNever()               //To for not creating id (AutoKey)
          .HasConversion(
            id => id.Value,                     //Going to the DB
            value => MenuItemId.Create(value) //From DB
          );

        mi.Property(m => m.Name)
          .HasMaxLength(100);
        mi.Property(m => m.Description)
          .HasMaxLength(100);

        //Set the actual Values for the items
        sb.Navigation(x => x.Items).Metadata.SetField("_items");  //From MenuSection Class
        sb.Navigation(x => x.Items).UsePropertyAccessMode(PropertyAccessMode.Field);

      });
      
      //Set the actual Values for the Sections - To Populate under lying List
      builder.Navigation(x => x.Sections).Metadata.SetField("_menuSection");  //From MenuSection Class
      builder.Navigation(x => x.Sections).UsePropertyAccessMode(PropertyAccessMode.Field);


      // //Set the actual Values for the Sections - To Populate under lying List
      // builder.Metadata.FindNavigation(nameof(Menu.Sections))!
      //   .SetPropertyAccessMode(PropertyAccessMode.Field);
    });
  }

  private void ConfigureMenuTable(EntityTypeBuilder<Menu> builder)
  {
    //Default Plural of Type T. This can be omitted
    builder.ToTable("Menus");

    //Key (Default is Id)
    builder.HasKey(m => m.Id);
    builder.Property(m => m.Id)
      .ValueGeneratedNever()          //To for not creating id (AutoKey)
      .HasConversion(
        id => id.Value,               //Going to the DB
        value => MenuId.Create(value) //From DB
      );
    //Property are from Type T. In this case Menu
    builder.Property(m => m.Name)
      .HasMaxLength(100);
    builder.Property(m => m.Description)
      .HasMaxLength(100);

    //This will create 2 field (as definded in ValueProperty of AverageRating)
    //Default DBName is AverageRating_Value, AverageRating_NumRatings
    //Which are the properties of AverageRating
    builder.OwnsOne(m => m.AverageRating);
    // // can be chaged by specifing the Property
    // builder.OwnsOne(m => m.AverageRating, ab => {
    //   ab.Property(a => a.Value)
    //     .HasColumnName("AverageValue");
    //   ab.Property(a => a.NumRatings)
    //     .HasColumnName("AverageRating");
    // });
    builder.Property(m => m.HostId)
      .ValueGeneratedNever()          //To for not creating id (AutoKey)
      .HasConversion(
        id => id.Value,               //Going to the DB
        value => HostId.Create(value) //From DB
      );
      

  }
}
```

#### Creating the table
1. Add Designer tool
```bash  
  dotnet add Apps\01-Apps.Api package Microsoft.EntityFrameworkCore.Design --version 8.0.11
```
2. Check if ef is intalled
```bash
  dotnet tool list --global
```
3. Create the migration
```bash
  dotnet ef migrations add InitCreate --project Apps\01-Apps.Infrastructure\Apps.Infrastructure.csproj --startup-project Apps\01-Apps.Api\Apps.Api.csproj
  dotnet ef migrations add InitCreate -p Apps\01-Apps.Infrastructure\Apps.Infrastructure.csproj -s Apps\01-Apps.Api\Apps.Api.csproj
```
4. Create the Tables
```bash
dotnet ef database update -p Apps\01-Apps.Infrastructure -s Apps\01-Apps.Api --connection "Data Source=AppsDb.db;"
```


### EF Core's Fluent API & DDD

### SQL Server on a docker container

### Migrations and more the using EF Core CLI tool

### VSCode + SQL Server = 💙


[Top][0] | [Back to main][1]

[0]:#ef-core-ddd-and-clean-architecture--mapping-aggregates-to-relational-databases
[1]:../../readme.md#contents
[2]:../../Apps/01-Apps.Infrastructure/Persistence/AppsDBContext.cs
[3]:../../Apps/01-Apps.Infrastructure/Persistence/Repositories/MenuRepository.cs
[4]:../../Apps/01-Apps.Infrastructure/Persistence/Configurations/MenuConfigurations.cs
[33]:../../Apps/01-Apps.Infrastructure/DependencyInjection.cs