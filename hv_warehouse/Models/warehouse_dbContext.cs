﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hv_warehouse.Models
{
    public partial class warehouse_dbContext : DbContext
    {
        public warehouse_dbContext()
        {
        }

        public warehouse_dbContext(DbContextOptions<warehouse_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Feeds> Feeds { get; set; }
        public virtual DbSet<Parts> Parts { get; set; }
        public virtual DbSet<Shipments> Shipments { get; set; }
        public virtual DbSet<Warehouses> Warehouses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=warehouse_db;Username=postgres;Password=1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("customer_key");

                entity.ToTable("customers");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.CustomerAddress)
                    .HasColumnName("customer_address")
                    .HasMaxLength(200);

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(50);

                entity.Property(e => e.CustomerPhone)
                    .HasColumnName("customer_phone")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Feeds>(entity =>
            {
                entity.HasKey(e => e.FeedId)
                    .HasName("feed_key");

                entity.ToTable("feeds");

                entity.Property(e => e.FeedId).HasColumnName("feed_id");

                entity.Property(e => e.FeedDate)
                    .HasColumnName("feed_date")
                    .HasColumnType("date");

                entity.Property(e => e.PartId)
                    .HasColumnName("part_id")
                    .HasMaxLength(10);

                entity.Property(e => e.PartQty).HasColumnName("part_qty");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.Feeds)
                    .HasForeignKey(d => d.PartId)
                    .HasConstraintName("feeds_part_id_fkey");
            });

            modelBuilder.Entity<Parts>(entity =>
            {
                entity.HasKey(e => e.PartId)
                    .HasName("part_key");

                entity.ToTable("parts");

                entity.Property(e => e.PartId)
                    .HasColumnName("part_id")
                    .HasMaxLength(10);

                entity.Property(e => e.PartDesc)
                    .HasColumnName("part_desc")
                    .HasMaxLength(200);

                entity.Property(e => e.PartMfr)
                    .HasColumnName("part_mfr")
                    .HasMaxLength(50);

                entity.Property(e => e.PartName)
                    .IsRequired()
                    .HasColumnName("part_name")
                    .HasMaxLength(20);

                entity.Property(e => e.PartPrice)
                    .HasColumnName("part_price")
                    .HasColumnType("numeric(10,2)");
            });

            modelBuilder.Entity<Shipments>(entity =>
            {
                entity.HasKey(e => e.ShipmentId)
                    .HasName("shipment_key");

                entity.ToTable("shipments");

                entity.Property(e => e.ShipmentId).HasColumnName("shipment_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.PartId)
                    .HasColumnName("part_id")
                    .HasMaxLength(10);

                entity.Property(e => e.ShipmentDate)
                    .HasColumnName("shipment_date")
                    .HasColumnType("date");

                entity.Property(e => e.ShipmentQty).HasColumnName("shipment_qty");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("shipments_customer_id_fkey");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.PartId)
                    .HasConstraintName("shipments_part_id_fkey");
            });

            modelBuilder.Entity<Warehouses>(entity =>
            {
                entity.HasKey(e => e.WarehouseId)
                    .HasName("warehouse_key");

                entity.ToTable("warehouses");

                entity.Property(e => e.WarehouseId)
                    .HasColumnName("warehouse_id")
                    .HasMaxLength(10);

                entity.Property(e => e.PartId)
                    .HasColumnName("part_id")
                    .HasMaxLength(10);

                entity.Property(e => e.PartQty).HasColumnName("part_qty");

                entity.Property(e => e.WarehouseAddress)
                    .IsRequired()
                    .HasColumnName("warehouse_address")
                    .HasMaxLength(100);

                entity.Property(e => e.WarehouseLocation)
                    .IsRequired()
                    .HasColumnName("warehouse_location")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.Warehouses)
                    .HasForeignKey(d => d.PartId)
                    .HasConstraintName("warehouses_part_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}