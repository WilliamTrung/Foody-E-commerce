using ApplicationCore;
using ApplicationCore.Models;
using BusinessService.Generic;
using BusinessService.Service;
using BusinessService.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductService
    {
        public ProductRepository(FoodyContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
