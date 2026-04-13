using EFCore.BulkExtensions;
using GoodsApi.Infrastructure.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Models.Database;

public class DataComponent(string connectionString)
{
    public IQueryable<Notification> Notifications => new AppDbContext(connectionString).Notifications;
    public IQueryable<User> Users => new AppDbContext(connectionString).Users;
    public IQueryable<Role> Roles => new AppDbContext(connectionString).Roles;
    public IQueryable<UserRole> UserRoles => new AppDbContext(connectionString).UserRoles;
    public IQueryable<Product> Products => new AppDbContext(connectionString).Products;
    public IQueryable<ProductInfo> ProductInfos => new AppDbContext(connectionString).ProductInfos;
    public IQueryable<Provider> Providers => new AppDbContext(connectionString).Providers;
    public IQueryable<InventoryOperation> InventoryOperations => new AppDbContext(connectionString).InventoryOperations;
    public IQueryable<RefundOperation> RefundOperations => new AppDbContext(connectionString).RefundOperations;
    public IQueryable<SaleOperation> SaleOperations => new AppDbContext(connectionString).SaleOperations;
    public IQueryable<SupplyOperation> SupplyOperations => new AppDbContext(connectionString).SupplyOperations;
    public IQueryable<WriteOffOperation> WriteOffOperations => new AppDbContext(connectionString).WriteOffOperations;
    public IQueryable<ProductMovement> ProductMovements => new AppDbContext(connectionString).ProductMovements;

    public async Task<bool> DeleteUser(int userId)
    {
        try
        {
            await using var context = new AppDbContext(connectionString);

            var userRoles = context.UserRoles
                .Where(u => u.Id == userId)
                .ToList();
            context.UserRoles.RemoveRange(userRoles);
            
            var writeOffOperations = context.WriteOffOperations
                .Where(w => w.UserId == userId)
                .ToList();
            context.WriteOffOperations.RemoveRange(writeOffOperations);
            
            var supplyOperations = context.SupplyOperations
                .Where(w => w.UserId == userId)
                .ToList();
            context.SupplyOperations.RemoveRange(supplyOperations);
            
            var saleOperations = context.SaleOperations
                .Where(w => w.UserId == userId)
                .ToList();
            context.SaleOperations.RemoveRange(saleOperations);
            
            var refundOperations = context.RefundOperations
                .Where(w => w.UserId == userId)
                .ToList();
            context.RefundOperations.RemoveRange(refundOperations);
            
            var inventoryOperations = context.InventoryOperations
                .Where(w => w.UserId == userId)
                .ToList();
            context.InventoryOperations.RemoveRange(inventoryOperations);
            
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;
            
            context.Users.Remove(user);
            
            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> Insert<T>(T entityItem) where T : class
    {
        try
        {
            await using var context = new AppDbContext(connectionString);
            await context.AddAsync(entityItem);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> BulkInsertAsync<T>(List<T> entityItem) where T : class
    {
        try
        {
            await using var context = new AppDbContext(connectionString);
            await context.BulkInsertAsync(entityItem);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> ChangeEntity<T>(T entityItem) where T : class,  IEntity
    {
        try
        {
            await using var context = new AppDbContext(connectionString);
            
            var dbSet =  context.Set<T>();
            
            var entity = await dbSet.FindAsync(entityItem.Id);
            
            if (entity == null) return false;
            
            context.Entry(entity).CurrentValues.SetValues(entityItem);
            context.Entry(entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Update<T>(T entityItem) where T : class
    {
        try
        {
            await using var context = new AppDbContext(connectionString);
            context.Entry(entityItem).State = EntityState.Modified;
            context.Update(entityItem);
            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> BulkUpdateAsync<T>(List<T> entities) where T : class
    {
        try
        {
            await using var context = new AppDbContext(connectionString);
            await context.BulkUpdateAsync(entities);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete<T>(int entityId) where T : class
    {
        try
        {
            await using var context = new AppDbContext(connectionString);
            var entity = await context.Set<T>().FindAsync(entityId);

            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}