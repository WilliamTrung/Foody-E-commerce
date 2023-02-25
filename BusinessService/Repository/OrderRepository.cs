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
using System.Threading.Tasks;

namespace BusinessService.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderService
    {
        public OrderRepository(FoodyContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public override async Task<IEnumerable<Order>> Get(Expression<Func<Order, bool>>? expression = null, params string[] includeProperties)
        {
            var list = await base.Get(expression, includeProperties);
            foreach (var item in list)
            {
                var details = await _unitOfWork.OrderDetailService.Get(c => c.OrderId == item.OrderId, includeProperties: "Product");
                item.OrderDetails = details.ToList();
            }
            return list;
        }
    }
}
