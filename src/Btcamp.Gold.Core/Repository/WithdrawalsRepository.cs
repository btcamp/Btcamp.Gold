using Btcamp.Gold.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Repository
{
    public class WithdrawalsRepository : Infrastructure.RepositoryBase<Withdrawals>, Infrastructure.IRepository<Withdrawals>
    {
        public WithdrawalsRepository(Infrastructure.IDbContextFactory dbfactory)
            : base(dbfactory)
        {

        }
    }
}
