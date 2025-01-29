using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FaixariT.HRMS.DBModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppMenu> AppMenu { get; set; }

    public virtual DbSet<AppModule> AppModule { get; set; }

    public virtual DbSet<AppSubMenu> AppSubMenu { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__App_Menu__C99ED23095A82D30");

            entity.ToTable("App_Menu");

            entity.Property(e => e.MenuName).HasMaxLength(100);
            entity.Property(e => e.Url).HasMaxLength(200);

            entity.HasOne(d => d.Module).WithMany(p => p.AppMenu)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__App_Menu__Module__267ABA7A");
        });

        modelBuilder.Entity<AppModule>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__App_Modu__2B7477A7AEA60543");

            entity.ToTable("App_Module");

            entity.Property(e => e.ModuleName).HasMaxLength(100);
        });

        modelBuilder.Entity<AppSubMenu>(entity =>
        {
            entity.HasKey(e => e.SubMenuId).HasName("PK__App_SubM__EA065CF992144CFA");

            entity.ToTable("App_SubMenu");

            entity.Property(e => e.SubMenuName).HasMaxLength(100);
            entity.Property(e => e.Url).HasMaxLength(200);

            entity.HasOne(d => d.Menu).WithMany(p => p.AppSubMenu)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__App_SubMe__MenuI__29572725");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC2B36EED6");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534741CF2BA").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DateJoined)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
