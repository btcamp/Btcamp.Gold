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
        public HomeController(IMT4Service mt4Service, IAccountService accountService)
        {
            _mt4Service = mt4Service;
            _accountService = accountService;
        }
        // GET: Home
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                Account account = _accountService.GetById(LoginAccount.Id);
                ViewBag.Interest = account.Interest.ToString("f2");
                ViewBag.Profit = account.Profit.ToString("f2");
            }
            else
            {
                ViewBag.Interest = 0.00;
                ViewBag.Profit = 0.00;
            }
            return View();
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
            decimal profit = await _mt4Service.GetAllGold(LoginAccount.MT4Account);
            var result = new { Profit = profit.ToString("F2") };
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
                response.Success = false;
                response.Msg = "买入黄金失败；请检查账户余额";
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