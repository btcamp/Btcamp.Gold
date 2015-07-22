using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Services
{
    public class WithdrawalsService : ServiceBase<Withdrawals>, IWithdrawalsService
    {
        public WithdrawalsService(IRepository<Withdrawals> _repository)
            : base(_repository)
        {

        }
    }
}
