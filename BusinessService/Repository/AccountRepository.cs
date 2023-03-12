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
    public class AccountRepository : GenericRepository<Account>, IAccountService
    {
        public AccountRepository(FoodyContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public Task<Account> Login(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<Account?> Register(Account account)
        {
            //account.RoleId = 2;//Member
            
            var match_username = _unitOfWork.AccountService.GetFirst(c => c.Username.Trim().ToLower() == account.Username.ToLower()).Result;
            if(match_username == null)
            {
                var role = _unitOfWork.RoleService.GetFirst(c => c.Name == "Member").Result;
                if (role != null)
                {
                    account.RoleId = role.Id;
                    account.IsDeleted = false;
                    await _unitOfWork.AccountService.Add(account);
                    return account;
                }
            } else
            {
                throw new Exception("DUPLICATE USERNAME");
            }
            
            return null;
        }
    }
}
