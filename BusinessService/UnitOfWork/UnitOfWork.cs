using ApplicationCore;
using BusinessService.Repository;
using BusinessService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountService AccountService { get; private set; } = null!;

        public ICategoryService CategoryService { get; private set; } = null!;

        public IOrderDetailService OrderDetailService { get; private set; } = null!;

        public IOrderService OrderService { get; private set; } = null!;

        public IProductService ProductService { get; private set; } = null!;

        public ISupplierService SupplierService { get; private set; } = null!;

        public IRoleService RoleService { get; private set; } = null!;

        private readonly FoodyContext _context;

        public UnitOfWork(FoodyContext context)
        {
            _context = context;
            InitRepositories();
        }

        private void InitRepositories()
        {
            AccountService = new AccountRepository(_context, this);
            CategoryService = new CategoryRepository(_context, this);
            OrderDetailService = new OrderDetailRepository(_context, this);
            OrderService = new OrderRepository(_context, this);
            ProductService = new ProductRepository(_context, this);
            SupplierService = new SupplierRepository(_context, this);
            RoleService = new RoleRepository(_context, this);
        }
    }
}
