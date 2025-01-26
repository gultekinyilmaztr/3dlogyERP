using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using _3dlogyERP.Infrastructure.Repositories;

namespace _3dlogyERP.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Customer> _customers;
        private IRepository<Order> _orders;
        private IRepository<OrderService> _orderServices;
        private IRepository<Equipment> _equipment;
        private IRepository<EquipmentType> _equipmentTypes;
        private IRepository<Material> _materials;
        private IRepository<MaterialType> _materialTypes;
        private IRepository<MaterialTransaction> _materialTransactions;
        private IRepository<ServiceType> _serviceTypes;
        private IRepository<User> _users;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Customer> Customers =>
            _customers ??= new Repository<Customer>(_context);

        public IRepository<Order> Orders =>
            _orders ??= new Repository<Order>(_context);

        public IRepository<OrderService> OrderServices =>
            _orderServices ??= new Repository<OrderService>(_context);

        public IRepository<Equipment> Equipment =>
            _equipment ??= new Repository<Equipment>(_context);

        public IRepository<EquipmentType> EquipmentTypes =>
            _equipmentTypes ??= new Repository<EquipmentType>(_context);

        public IRepository<Material> Materials =>
            _materials ??= new Repository<Material>(_context);

        public IRepository<MaterialType> MaterialTypes =>
            _materialTypes ??= new Repository<MaterialType>(_context);

        public IRepository<MaterialTransaction> MaterialTransactions =>
            _materialTransactions ??= new Repository<MaterialTransaction>(_context);

        public IRepository<ServiceType> ServiceTypes =>
            _serviceTypes ??= new Repository<ServiceType>(_context);

        public IRepository<User> Users =>
            _users ??= new Repository<User>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
