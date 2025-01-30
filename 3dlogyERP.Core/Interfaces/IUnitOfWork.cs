using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Customer> Customers { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderService> OrderServices { get; }
    IRepository<Equipment> Equipment { get; }
    IRepository<EquipmentType> EquipmentTypes { get; }
    IRepository<Material> Materials { get; }
    IRepository<MaterialType> MaterialTypes { get; }
    IRepository<MaterialTransaction> MaterialTransactions { get; }
    IRepository<ServiceType> ServiceTypes { get; }
    IRepository<User> Users { get; }
    IRepository<Expense> Expenses { get; }
    IRepository<ExpenseCategory> ExpenseCategories { get; }

    Task<int> SaveChangesAsync();
    Task<int> CompleteAsync();
}