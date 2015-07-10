using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Services.Interface
{
    public interface IMT4Service
    {
        Task<decimal> GetGoldPrice(double price = 0);

        Task<bool> Buy(string loginId, int volume);

        Task<decimal> GetAllGold(string loginId);

        Task<List<Models.TradingModel>> GetTradingByLoginId(string loginId);

        /// <summary>
        /// 卖出 不是空单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Sell(List<int> ids);

        Task<List<Models.TradingModel>> GetTradeLogs(string loginId);
    }
}
