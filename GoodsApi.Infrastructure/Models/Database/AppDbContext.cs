using GoodsApi.Infrastructure.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Models.Database;

public class AppDbContext(string connectionString) : DbContext
{
    public AppDbContext() : this("Server=localhost;Port=5434;User Id=postgres;Password=12345;Database=xamarinDb")
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Role>()
            .ToTable("roles")
            .HasData(
                new Role { Id = 1, Name = "Admin", Description = ""},
                new Role { Id = 2, Name = "Manager", Description = ""},
                new Role { Id = 3, Name = "Storekeeper", Description = "" }
            );
        modelBuilder.Entity<UserRole>().ToTable("user_roles");
        
        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductInfos)
            .WithOne(pi => pi.Product)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<ProductInfo>().ToTable("product_information");
        modelBuilder.Entity<Provider>().ToTable("providers");
        modelBuilder.Entity<InventoryOperation>().ToTable("inventory_operations");
        modelBuilder.Entity<RefundOperation>().ToTable("refund_operations");
        modelBuilder.Entity<SaleOperation>().ToTable("sale_operations");
        modelBuilder.Entity<SupplyOperation>().ToTable("supply_operations");
        modelBuilder.Entity<WriteOffOperation>().ToTable("write_off_operations");
        modelBuilder.Entity<Notification>().ToTable("notifications");

        modelBuilder.Entity<ProductMovement>(entity =>
        {
            entity.ToView("product_movement_view");
            entity.HasNoKey();

            entity.Property(e => e.OperationDate)
                .HasColumnName("operationdate");
            
            entity.Property(e => e.OperationType)
                .HasColumnName("operationtype");
            
            entity.Property(e => e.ProductName)
                .HasColumnName("productname");
            
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");

            entity.Property(e => e.CounterAgent)
                .HasColumnName("counteragent");


        });
    }

    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductInfo> ProductInfos { get; set; }
    public virtual DbSet<Provider> Providers { get; set; }
    public virtual DbSet<InventoryOperation> InventoryOperations { get; set; }
    public virtual DbSet<RefundOperation> RefundOperations { get; set; }
    public virtual DbSet<SaleOperation> SaleOperations { get; set; }
    public virtual DbSet<SupplyOperation> SupplyOperations { get; set; }
    public virtual DbSet<WriteOffOperation> WriteOffOperations { get; set; }
    public virtual DbSet<ProductMovement> ProductMovements { get; set; }
}