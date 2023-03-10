using ApplicationCore;
using ApplicationCore.Models;
using BusinessService.Generic;
using BusinessService.Service;
using BusinessService.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessService.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductService
    {
        public ProductRepository(FoodyContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        private async Task<bool> CheckDuplicateAsync(Product product)
        {
            bool result = true;
            var findByName =  await Get(expression: p => p.ProductName.ToLower() == product.ProductName.ToLower());
            if (findByName != null && findByName.Count() > 0)
            {
                result = false;
            }
            return result;
        }
        public override async Task Add(Product entity)
        {
            if(await CheckDuplicateAsync(entity))
            {
                await base.Add(entity);
            } else
            {
                throw new Exception("ProductName-Đã có sản phẩm cùng tên!");
            }
        }
        public override async Task<IEnumerable<Product>> Get(Expression<Func<Product, bool>>? expression = null, params string[] includeProperties)
        {
            var list = (await base.Get(expression, includeProperties)).OrderBy(c => c.IsDeleted).ThenBy(c=>c.ProductName);
            return list;
        }
    }
}
