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
    public class RoleRepository : GenericRepository<Role>, IRoleService
    {
        public RoleRepository(FoodyContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
