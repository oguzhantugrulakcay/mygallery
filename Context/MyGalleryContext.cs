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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=213.238.168.71\\MSSQLSERVER2017;Database=myGalerry;User ID=libertycars;Password=Art2356.!;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("libertycars");

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK_BRAND");

            entity.Property(e => e.BrandName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<BuyRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK_BuyRequest");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExtraExtension).HasMaxLength(1000);
            entity.Property(e => e.FistName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.FuelType).HasMaxLength(25);
            entity.Property(e => e.GearType)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.GsmNo)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

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

            entity.Property(e => e.ExtensionName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK_MODEL");

            entity.Property(e => e.ModelName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.Models)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ModelMustHaveBrand");
        });

        modelBuilder.Entity<RequestDamageInfo>(entity =>
        {
            entity.HasKey(e => e.InfoId).HasName("PK_RequestDamageInfo");

            entity.Property(e => e.Damage)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PartName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Request).WithMany(p => p.RequestDamageInfos)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DamageInfoMustHaveRequest");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LoginName)
                .IsRequired()
                .HasMaxLength(25);
            entity.Property(e => e.LoginPassword).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
