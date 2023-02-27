using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApplicationCore
{
    public partial class FoodyContext : DbContext
    {
        public FoodyContext()
        {
        }

        public FoodyContext(DbContextOptions<FoodyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=.;Database=FoodyStore;User ID=sa;Password=123;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasKey(k => new { k.OrderId, k.ProductId });

            modelBuilder.Entity<Role>().HasData(new Role()
            {
                Id = 1,
                Name = "Administrator"
            },
                new Role()
                {
                    Id = 2,
                    Name = "Member"
                },
                new Role()
                {
                    Id = 3,
                    Name = "Seller"
                }
            );
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                AccountId = 2,
                Address = "",
                Password = "123",
                Phone = "0908123456",
                RoleId = 2,
                Username = "Member"
            },
                new Account()
                {
                    AccountId = 1,
                    Address = "",
                    Password = "123",
                    Phone = "0908123456",
                    RoleId = 1,
                    Username = "Administrator"
                },
                new Account()
                {
                    AccountId = 3,
                    Address = "",
                    Password = "123",
                    Phone = "0908123456",
                    RoleId = 3,
                    Username = "Seller"
                });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
