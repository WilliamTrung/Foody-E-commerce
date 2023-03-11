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
        public override async Task Add(Order entity)
        {

            entity.OrderDate = DateTime.Now;
            entity.RequiredDate = DateTime.Now.AddDays(10);
            entity.ShippedDate = DateTime.Now.AddDays(7);
            try
            {
                await base.Add(entity);
                foreach(var detail in entity.OrderDetails)
                {
                    var product = _unitOfWork.ProductService.GetFirst(c => c.ProductId == detail.ProductId).Result;
                    if(product != null)
                    {
                        product.QuantityPerUnit -= detail.Quantity;
                        await _unitOfWork.ProductService.Update(product);
                    }
                    
                }
            } catch {
                throw new Exception("ADD FAILED");
            }            
        }
        public override async Task Delete(Order entity)
        {

            //var today = DateTime.Today;
            //bool canDelete = true;
            //if(entity.ShippedDate != null)
            //{
            //    //shipped date is 3 day later from today
            //    if (!(entity.ShippedDate.Value.Date.CompareTo(today) >= 3)) { 
            //        canDelete= false;
            //    }
            //}
            ////incase shippeddate is unknown
            //if(entity.RequiredDate.Date.CompareTo(today) >= 3)
            //{                
            //    canDelete= false;
            //}
            //if(canDelete)
            //{
            //    await base.Delete(entity);
            //} else
            //{
            //    throw new Exception("OVERDUE");
            //}
        }
    }
}
