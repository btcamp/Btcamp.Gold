using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Services
{
    public class ClosingPriceService : ServiceBase<ClosingPrice>, Interface.IClosingPriceService
    {
        public ClosingPriceService(IRepository<ClosingPrice> _repository)
            : base(_repository)
        {

        }
    }
}
