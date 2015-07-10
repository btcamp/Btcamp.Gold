using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Services.Interface
{
    public interface IAccountService : IService<Account>
    {
        Account AccountLoginByPhoneNumber(string phoneNumber, string pwd);

        void RegisterAccount(Account account);
    }
}
