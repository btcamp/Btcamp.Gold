using AutoMapper;
using Btcamp.Gold.Core.Common;
using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Services;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMT4Service _mt4Service = null;
        private readonly IAccountService _accountService = null;
        private readonly ISystemSettingsService _SystemSettingsService = null;
        public HomeController(IMT4Service mt4Service, IAccountService accountService, ISystemSettingsService SystemSettingsService)
        {
            _mt4Service = mt4Service;
            _accountService = accountService;
            _SystemSettingsService = SystemSettingsService;
        }
        // GET: Home
        public Task<ActionResult> Index()
        {
            return Task.Factory.StartNew<ActionResult>(() =>
            {
                if (Request.IsAuthenticated)
                {
                    Account account = _accountService.GetById(LoginAccount.Id);
                    ViewBag.Interest = account.Interest.ToString("f2");
                    SystemSettings model = _SystemSettingsService.GetManyAsNoTracking(e => e.Key == "InterestRate").FirstOrDefault();
                    ViewBag.Info = model == null ? "" : model.Info;
                }
                else
                {
                    ViewBag.Interest = 0.00;
                    ViewBag.Profit = 0.00;
                }

                return View();
            });

        }

        public async Task<ActionResult> PriceHub(decimal currentPrice)
        {
            decimal price = await _mt4Service.GetGoldPrice();
            if (price <= 0)
            {
                price = currentPrice;
            }
            return Json(price.ToString("f2"), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetGold()
        {
            Core.Models.TradeGoldModel model = await _mt4Service.GetAllGold(LoginAccount.MT4Account);
            var result = new { Profit = model.SumProfit.ToString("F2"), Gold = model.SumGold.ToString("f2") };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Buy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Buy(Models.TradeBuyViewModel model)
        {
            ResponseModel response = new ResponseModel();
            bool flg = await _mt4Service.Buy(LoginAccount.MT4Account, model.Volume);
            if (flg)
            {
                response.Success = flg;
                response.Msg = "成功买入黄金";
            }
            else
            {
                response.Success = true;
                response.Msg = "账户余额不足,买入黄金失败";
                response.RedirectUrl = Url.Action("Deposit", "Account");
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Sell()
        {
            List<Core.Models.TradingModel> result = await _mt4Service.GetTradingByLoginId(LoginAccount.MT4Account);
            var viewResut = AutoMapper.Mapper.Map<List<Models.TradingViewModel>>(result);
            return View(viewResut);
        }

        [HttpPost]
        public async Task<ActionResult> Sell(List<int> list)
        {
            ResponseModel response = new ResponseModel();
            bool flg = await _mt4Service.Sell(list);
            if (flg)
            {
                response.Success = true;
                response.Msg = "成功卖出黄金";
            }
            else
            {
                response.Success = false;
                response.Msg = "黄金卖出有失败，请重试！";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetExists()
        {
            List<Core.Models.TradingModel> result = await _mt4Service.GetTradingByLoginId(LoginAccount.MT4Account);
            var viewResut = AutoMapper.Mapper.Map<List<Models.TradingViewModel>>(result);
            return Json(viewResut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SumInterest()
        {
            var account = _accountService.GetById(LoginAccount.Id);
            ViewBag.Data = account.SumInterest.ToString("f2");
            return View();
        }
    }
}