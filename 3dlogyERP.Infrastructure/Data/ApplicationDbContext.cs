using _3dlogyERP.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3dlogyERP.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderService> OrderServices { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<MaterialTransaction> MaterialTransactions { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<StockCategory> StockCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Phone).IsUnique();
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.Property(e => e.TotalCost).HasPrecision(18, 2);
                entity.Property(e => e.FinalPrice).HasPrecision(18, 2);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderService configuration
            modelBuilder.Entity<OrderService>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.MaterialCost).HasPrecision(18, 2);
                entity.Property(e => e.LaborCost).HasPrecision(18, 2);
                entity.Property(e => e.EquipmentCost).HasPrecision(18, 2);
                entity.Property(e => e.TotalCost).HasPrecision(18, 2);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.MaterialQuantity).HasPrecision(18, 3);
                entity.Property(e => e.Price).HasPrecision(18, 2);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.OrderServices)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Equipment configuration
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
                entity.Property(e => e.HourlyRate).HasPrecision(18, 2);
                entity.Property(e => e.MaintenanceCostPerHour).HasPrecision(18, 2);
                entity.Property(e => e.ElectricityConsumptionPerHour).HasPrecision(18, 2);
                entity.HasIndex(e => e.SerialNumber).IsUnique();

                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.Equipments)
                    .HasForeignKey(d => d.EquipmentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // EquipmentType configuration
            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Material configuration
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.UnitCost).HasPrecision(18, 2);
                entity.Property(e => e.CurrentStock).HasPrecision(18, 3);
                entity.Property(e => e.MinimumStock).HasPrecision(18, 3);
                entity.Property(e => e.ReorderPoint).HasPrecision(18, 3);
                entity.Property(e => e.SKU).HasMaxLength(50);
                entity.Property(e => e.BatchNumber).HasMaxLength(50);
                entity.Property(e => e.WeightPerUnit).HasPrecision(18, 3);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.Specifications).HasMaxLength(500);

                entity.HasOne(d => d.MaterialType)
                    .WithMany(p => p.Materials)
                    .HasForeignKey(d => d.MaterialTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // StockCategory iliþkisi eklendi
                entity.HasOne(d => d.StockCategory)
                    .WithMany(p => p.Materials)
                    .HasForeignKey(d => d.StockCategoryCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // MaterialType configuration
            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.StockCategory).IsRequired();
                entity.Property(e => e.Unit).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // MaterialTransaction configuration
            modelBuilder.Entity<MaterialTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.TransactionDate).IsRequired();
                entity.Property(e => e.Quantity).HasPrecision(18, 3).IsRequired();
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ServiceType configuration
            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.BasePrice).HasPrecision(18, 2);
                entity.Property(e => e.IsActive);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

        }
    }
}