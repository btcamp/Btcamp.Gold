using Autofac.Integration.Mvc;
using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Models;
using Btcamp.Gold.Core.Repository;
using Btcamp.Gold.Core.Services;
using Btcamp.Gold.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Btcamp.Gold.Web
{
    public class AutoGetClosingPriceConfig
    {

        public static void Start()
        {
            IClosingPriceService service = (IClosingPriceService)AutofacDependencyResolver.Current.GetService(typeof(IClosingPriceService));
            IUnitOfWork unitOfWork = (IUnitOfWork)AutofacDependencyResolver.Current.GetService(typeof(IUnitOfWork));
            IMT4Service mt4Service = (IMT4Service)AutofacDependencyResolver.Current.GetService(typeof(IMT4Service));
            IAccountService accountService = (IAccountService)AutofacDependencyResolver.Current.GetService(typeof(IAccountService));
            log4net.ILog log = (log4net.ILog)AutofacDependencyResolver.Current.GetService(typeof(log4net.ILog));
            ThreadPool.QueueUserWorkItem(async (obj) =>
            {
                DateTime today = DateTime.Now, yesterday = DateTime.Now;
                while (true)
                {
                    try
                    {
                        yesterday = DateTime.Now;
                        if (yesterday.AddDays((double)1).Day == today.Day)
                        {
                            decimal closeprice = await mt4Service.GetGoldPrice();
                            while (closeprice <= 0)
                            {
                                closeprice = await mt4Service.GetGoldPrice();
                                Thread.Sleep(2000);
                            }
                            foreach (Account item in accountService.GetAll().ToList())
                            {
                                List<TradingModel> tradings = await mt4Service.GetTradingByLoginId(item.MT4Account);
                                if (tradings == null)
                                {
                                    item.Profit = 0;
                                    accountService.Update(item);
                                    unitOfWork.Commit();
                                    continue;
                                }
                                if (tradings.Count == 0)
                                {
                                    if (item.Profit != 0 && item.Interest != 0)
                                    {
                                        item.Profit = 0;
                                        item.Interest = 0;
                                        accountService.Update(item);
                                        unitOfWork.Commit();
                                    }
                                }
                                else
                                {
                                    //昨日收益
                                    var profit = (decimal)tradings.Sum(e => e.Profit);
                                    var sumWeight = (decimal)tradings.Sum(e => e.Weight);
                                    item.Profit = profit;
                                    item.Interest = GetInterest(closeprice, sumWeight);
                                    item.SumInterest += item.Interest;
                                    accountService.Update(item);
                                    unitOfWork.Commit();
                                }

                            }
                            service.Add(new Core.Entitys.ClosingPrice() { Price = closeprice });
                            unitOfWork.Commit();
                        }
                        Thread.Sleep(1000 * 60);
                        today = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            });
        }

        private static decimal GetInterest(decimal profit, decimal weight)
        {
            return (profit * weight * 0.058m) / 365;
        }
    }
}