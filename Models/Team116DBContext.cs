
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DroneWorks.Models
{
    public partial class Team116DBContext : DbContext
    {
        public Team116DBContext()
        {
        }

        public Team116DBContext(DbContextOptions<Team116DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<WorksOrder> WorksOrder { get; set; }
        public virtual DbSet<WorksUser> WorksUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=buscissql1601\\cisweb;Database=Team116DB; User ID=project;Password=assist;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatPk);

                entity.Property(e => e.CatPk)
                    .HasColumnName("CatPK")
                    .ValueGeneratedNever();

                entity.Property(e => e.CatImage).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemPk);

                entity.Property(e => e.OrderItemPk).HasColumnName("OrderItemPK");

                entity.Property(e => e.OrderFk).HasColumnName("OrderFK");

                entity.Property(e => e.ProdFk).HasColumnName("ProdFK");

                entity.HasOne(d => d.OrderFkNavigation)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderFk)
                    .HasConstraintName("FK_OrderItem_Order");

                entity.HasOne(d => d.ProdFkNavigation)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.ProdFk)
                    .HasConstraintName("FK_OrderItem_Products");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProdPk);

                entity.Property(e => e.ProdPk)
                    .HasColumnName("ProdPK")
                    .ValueGeneratedNever();

                entity.Property(e => e.CatFk).HasColumnName("CatFK");

                entity.Property(e => e.ImageName).HasMaxLength(50);

                entity.Property(e => e.ProdName).HasMaxLength(50);

                entity.Property(e => e.ProdPrice).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.CatFkNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatFk)
                    .HasConstraintName("FK_Products_Category");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.UserRolePk);

                entity.Property(e => e.UserRolePk)
                    .HasColumnName("UserRolePK")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleFunction).HasMaxLength(50);

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<WorksOrder>(entity =>
            {
                entity.HasKey(e => e.OrderPk)
                    .HasName("PK_Order");

                entity.Property(e => e.OrderPk)
                    .HasColumnName("OrderPK")
                    .ValueGeneratedNever();

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderStatus).HasMaxLength(50);

                entity.Property(e => e.ShipAddress).HasMaxLength(50);

                entity.Property(e => e.ShipCity).HasMaxLength(50);

                entity.Property(e => e.ShipCountry).HasMaxLength(50);

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.ShipState).HasMaxLength(50);

                entity.Property(e => e.UserFk).HasColumnName("UserFK");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithMany(p => p.WorksOrder)
                    .HasForeignKey(d => d.UserFk)
                    .HasConstraintName("FK_Order_WorksUser");
            });

            modelBuilder.Entity<WorksUser>(entity =>
            {
                entity.HasKey(e => e.UserPk)
                    .HasName("PK_User");

                entity.Property(e => e.UserPk)
                    .HasColumnName("UserPK")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RoleFk).HasColumnName("RoleFK");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.RoleFkNavigation)
                    .WithMany(p => p.WorksUser)
                    .HasForeignKey(d => d.RoleFk)
                    .HasConstraintName("FK_User_UserRole");
            });
        }
    }
}
