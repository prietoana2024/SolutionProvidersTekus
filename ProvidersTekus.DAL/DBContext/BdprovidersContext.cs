using Microsoft.EntityFrameworkCore;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;

namespace ProvidersTekus.DAL.DBContext;

public partial class BdprovidersContext : DbContext
{
    public BdprovidersContext()
    {
    }

    public BdprovidersContext(DbContextOptions<BdprovidersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CamposPersonalizado> CamposPersonalizados { get; set; }

    public virtual DbSet<ProveedorCamposValore> ProveedorCamposValores { get; set; }

    public virtual DbSet<ProveedorServicio> ProveedorServicios { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<VistaProveedoresCompleta> VistaProveedoresCompleta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CamposPersonalizado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CamposPe__3214EC077C6B82DE");

            entity.HasIndex(e => e.NombreCampo, "UQ__CamposPe__B351D0BC8A21C0E6").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Etiqueta).HasMaxLength(200);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.NombreCampo).HasMaxLength(100);
            entity.Property(e => e.TipoDato).HasMaxLength(50);
        });

        modelBuilder.Entity<ProveedorCamposValore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Proveedo__3214EC07221F7FFB");

            entity.HasIndex(e => e.CampoPersonalizadoId, "IX_ProveedorCamposValores_CampoPersonalizadoId");

            entity.HasIndex(e => e.ProveedorId, "IX_ProveedorCamposValores_ProveedorId");

            entity.HasIndex(e => new { e.ProveedorId, e.CampoPersonalizadoId }, "UQ_ProveedorCampo").IsUnique();

            entity.HasOne(d => d.CampoPersonalizado).WithMany(p => p.ProveedorCamposValores)
                .HasForeignKey(d => d.CampoPersonalizadoId)
                .HasConstraintName("FK_ProveedorCamposValores_CamposPersonalizados");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.ProveedorCamposValores)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK_ProveedorCamposValores_Proveedores");
        });

        modelBuilder.Entity<ProveedorServicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Proveedo__3214EC07CF5308A7");

            entity.HasIndex(e => new { e.ProveedorId, e.ServicioId }, "UQ_ProveedorServicio").IsUnique();

            entity.HasOne(d => d.Proveedor).WithMany(p => p.ProveedorServicios)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProveedorServicios_Proveedor");

            entity.HasOne(d => d.Servicio).WithMany(p => p.ProveedorServicios)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProveedorServicios_Servicio");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Proveedo__3214EC078313310D");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Nit).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC07F3D9F922");

            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Paises).HasMaxLength(200);
            entity.Property(e => e.ValorHora).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VistaProveedoresCompleta>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaProveedoresCompleta");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Etiqueta).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.NombreCampo).HasMaxLength(100);
            entity.Property(e => e.TipoDato).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
