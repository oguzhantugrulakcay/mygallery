using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace mygallery.Context;

public partial class MyGalleryContext : DbContext
{
    public MyGalleryContext()
    {
    }

    public MyGalleryContext(DbContextOptions<MyGalleryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<BuyRequest> BuyRequests { get; set; }

    public virtual DbSet<BuyRequestExtension> BuyRequestExtensions { get; set; }

    public virtual DbSet<CarExtension> CarExtensions { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<RequestDamageInfo> RequestDamageInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies(); // Lazy Loading'i etkinleştir
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("mygallery_admin");

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK_BRAND");

            entity.Property(e => e.BrandName).HasMaxLength(100);
        });

        modelBuilder.Entity<BuyRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK_BuyRequest");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExtraExtension).HasMaxLength(1000);
            entity.Property(e => e.FistName).HasMaxLength(100);
            entity.Property(e => e.FuelType).HasMaxLength(10);
            entity.Property(e => e.GearType).HasMaxLength(10);
            entity.Property(e => e.GsmNo).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);

            entity.HasOne(d => d.Model).WithMany(p => p.BuyRequests)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BuyRequestMustHaveModel");
        });

        modelBuilder.Entity<BuyRequestExtension>(entity =>
        {
            entity.HasKey(e => e.RequestExtensionId).HasName("PK_BuyRequestExtension");

            entity.HasOne(d => d.Extension).WithMany(p => p.BuyRequestExtensions)
                .HasForeignKey(d => d.ExtensionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BuyReqeustExtensionMustHaveExtension");

            entity.HasOne(d => d.Request).WithMany(p => p.BuyRequestExtensions)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BuyRequestExtensionMustHaveRequest");
        });

        modelBuilder.Entity<CarExtension>(entity =>
        {
            entity.HasKey(e => e.ExtensionId).HasName("PK_CarEKtension");

            entity.Property(e => e.ExtensionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK_MODEL");

            entity.Property(e => e.ModelName).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.Models)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ModelMustHaveBrand");
        });

        modelBuilder.Entity<RequestDamageInfo>(entity =>
        {
            entity.HasKey(e => e.InfoId).HasName("PK_RequestDamageInfo");

            entity.Property(e => e.Damage).HasMaxLength(50);
            entity.Property(e => e.PartName).HasMaxLength(50);

            entity.HasOne(d => d.Request).WithMany(p => p.RequestDamageInfos)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DamageInfoMustHaveRequest");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LoginName).HasMaxLength(25);
            entity.Property(e => e.LoginPassword).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
