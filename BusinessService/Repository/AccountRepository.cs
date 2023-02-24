using ApplicationCore;
using ApplicationCore.Models;
using ApplicationService.UnitOfWork;
using BusinessService.Generic;
using BusinessService.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountService
    {
        public AccountRepository(PizzaStoreContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public Task<Account> Login(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
