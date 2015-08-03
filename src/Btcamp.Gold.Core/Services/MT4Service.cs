using Btcamp.Gold.Core.Common;
using Btcamp.Gold.Core.MT4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Services
{
    public class MT4Service : Interface.IMT4Service
    {
        /// <summary>
        /// 非单利对象；
        /// </summary>
        internal static IPumpService Instance
        {
            get
            {
                return new PumpServiceClient("BasicHttpBinding_IPumpService");
            }
        }

        public async Task<decimal> GetGoldPrice(double price = 0)
        {
            var symbol = await MT4Service.Instance.GetSymbolInfoAsync(ApplicationCommon.SymbolName);
            if (symbol != null)
            {
                decimal angsirate = (decimal)28.3495231;
                #region RMB
                //string url = "http://apis.baidu.com/apistore/currencyservice/currency";
                //string param = "fromCurrency=USD&toCurrency=CNY&amount=1";
                //string result = ApplicationCommon.Request(url, param);
                //Rate rate = Newtonsoft.Json.JsonConvert.DeserializeObject<Rate>(result);
                //if (rate.errNum == 0)
                //{
                //    
                //    price = price == 0 ? symbol.Bid : price;
                //    return ((decimal)price * (decimal)rate.retData.currency) / angsirate;
                //} 
                #endregion

                #region USD
                price = price == 0 ? symbol.Bid : price;
                return ((decimal)price) / angsirate;
                #endregion
            }
            return 0;
        }

        public async Task<bool> Buy(string loginId, int volume)
        {
            SymbolInfoSE symbol = null;
            while (symbol == null)
            {
                symbol = await MT4Service.Instance.GetSymbolInfoAsync(ApplicationCommon.SymbolName);
            }
            var trade = new TradeTransInfoSE()
            {
                OrderBy = int.Parse(loginId),
                Volume = volume * 100,
                StopLoss = 0,
                Cmd = 0,
                TakeProfit = 0,
                Order = 0,
                IeDeviation = 0,
                Symbol = ApplicationCommon.SymbolName,
                Price = symbol.Ask
            };
            MT4OperResult result = await Instance.TradeTranscationOpenAsync(trade);
            return result.ErrorCode != -1;
        }


        public async Task<decimal> GetAllGold(string loginId)
        {
            TradeRecordSE[] trades = await Instance.GetOpenTradeByLoginAsync(int.Parse(loginId));
            double sumGold = trades.Sum(e => e.Volume) / 100;
            sumGold = Math.Round(sumGold, 2);
            return (decimal)sumGold;
        }


        public async Task<List<Models.TradingModel>> GetTradingByLoginId(string loginId)
        {
            TradeRecordSE[] trades = await Instance.GetOpenTradeByLoginAsync(int.Parse(loginId));
            List<Models.TradingModel> list = new List<Models.TradingModel>();
            foreach (TradeRecordSE item in trades)
            {
                decimal price = await GetGoldPrice(item.OpenPrice);
                list.Add(new Models.TradingModel(item.OrderId, item.OpenTime, price, ((item.Volume / 100) * 0.5), item.Profit));
            }
            return list;
        }


        public async Task<bool> Sell(List<int> ids)
        {
            List<TradeCloseModel> closes = new List<TradeCloseModel>();
            SymbolInfoSE symbol = null;
            while (symbol == null)
            {
                symbol = await MT4Service.Instance.GetSymbolInfoAsync(ApplicationCommon.SymbolName);
            }
            foreach (var item in ids)
            {
                closes.Add(new TradeCloseModel() { OrderTicket = item, Price = symbol.Ask });
            }
            TradeCloseResponseModel[] responses = await Instance.TradeCloseAllAsync(closes.ToArray());
            foreach (var item in responses)
            {
                if (item.ErrorCode == -1)
                {
                    return false;
                }
            }
            return true;
        }


        public async Task<List<Models.TradingModel>> GetTradeLogs(string loginId)
        {
            TradeRecordSE[] tradelogs = await Instance.GetTradesRecordHistoryAsync(int.Parse(loginId), DateTime.Now.AddYears(-10), DateTime.Now.AddDays(1));
            List<Models.TradingModel> list = new List<Models.TradingModel>();
            foreach (TradeRecordSE item in tradelogs)
            {
                if (item.Cmd == 6)
                {
                    continue;
                }
                decimal price = await GetGoldPrice(item.OpenPrice);
                list.Add(new Models.TradingModel(item.OrderId, item.OpenTime, price, ((item.Volume / 100) * 0.5), item.Profit));
            }
            return list;
        }


        public async Task<bool> ModifyBalance(string loginId, double amount)
        {
            MT4OperResult result = await Instance.TradeTranscationBalanceAsync(int.Parse(loginId), amount);
            return result.ErrorCode == 0;
        }


        public async Task<decimal> GetProfit(string loginId)
        {
            TradeRecordSE[] records = await Instance.GetOpenTradeByLoginAsync(int.Parse(loginId));
            if (records != null && records.Length > 0)
            {
                return (decimal)records.Sum(e => e.Profit);
            }
            return 0m;
        }
    }
}
