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