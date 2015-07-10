using Btcamp.Gold.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Repository
{
    public class AccountRepository : Infrastructure.RepositoryBase<Account>, Infrastructure.IRepository<Account>
    {
        public AccountRepository(Infrastructure.IDbContextFactory dbfactory)
            : base(dbfactory)
        {

        }
    }
}
