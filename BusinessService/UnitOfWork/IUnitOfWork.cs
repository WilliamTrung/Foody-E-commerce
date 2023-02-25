using BusinessService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAccountService AccountService { get; }
        public ICategoryService CategoryService { get; }
        public IOrderDetailService OrderDetailService { get; }
        public IOrderService OrderService { get; }
        public IProductService ProductService { get; }
        public ISupplierService SupplierService { get; }
        public IRoleService RoleService { get; }
    }
}
