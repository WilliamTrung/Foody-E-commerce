using ApplicationCore.Models;
using BusinessService.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.IService
{
    public interface IOrderDetailService : IGenericRepository<OrderDetail>
    {
    }
}
