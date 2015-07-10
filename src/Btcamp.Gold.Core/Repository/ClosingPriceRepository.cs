using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Repository
{
    public class ClosingPriceRepository : RepositoryBase<ClosingPrice>, IRepository<ClosingPrice>
    {
        public ClosingPriceRepository(IDbContextFactory dbfactory)
            : base(dbfactory)
        {

        }
    }
}
