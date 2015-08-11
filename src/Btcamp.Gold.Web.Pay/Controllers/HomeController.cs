using Btcamp.Gold.Core.Common;
using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Pay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Pay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISystemSettingsService systemSettingService = null;
        private readonly IAccountService accountService = null;
        private readonly IDepositService depositService = null;
        private readonly IMT4Service mt4Service = null;
        private readonly IUnitOfWork unitOfWork = null;
        public HomeController(ISystemSettingsService _systemSettingService, IAccountService _accountService, IDepositService _depositService, IMT4Service _mt4Service, IUnitOfWork _unitOfWork)
        {
            this.accountService = _accountService;
            this.systemSettingService = _systemSettingService;
            this.depositService = _depositService;
            this.mt4Service = _mt4Service;
            this.unitOfWork = _unitOfWork;
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //http://shypet.cn/Payment/Index?Username=测试&Phone=15208377468&Email=22335114101112@qq.com&Linkphone=88163584&Successurl=http://www.fino.trade/umbraco/Api/Payment/PaySuccess&Failurl=http://www.fino.trade/umbraco/Api/Payment/PayFailure&Paytype=2&Formurl=https://pay.ecpss.com/sslpayment&USD=1.00&CNY=6.2210
            string payurl = systemSettingService.Get(e => e.Key == "pay").Info;
            string paytype = systemSettingService.Get(e => e.Key == "Paytype").Info;
            string failure = systemSettingService.Get(e => e.Key == "Failure").Info;
            string successurl = systemSettingService.Get(e => e.Key == "Successurl").Info;
            string formurl = systemSettingService.Get(e => e.Key == "Formurl").Info;
            //email 赋值成的Phone联系方式，支付接口那边以email为唯一建操作的
            ViewBag.Url = string.Format("{0}?Username={1}&Email={2}&LinkPhone={3}&successurl={4}&Failurl={5}&Paytype={6}&Formurl={7}", payurl, string.Empty, string.Empty, string.Empty, successurl, failure, paytype, formurl);
            return View();
        }

        public async Task<ActionResult> Success(string Content, bool isServerCall = false)
        {
            //Content = DESHelper.DecryptDES(Content);
            string path = Server.MapPath("/log");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            path = System.IO.Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".txt");
            try
            {

                System.IO.File.AppendAllText(path, string.Format("第一步：{0}\r\n", Content));
                AccountDepostiViewModel depostiViewModel = JsonConvert.DeserializeObject<AccountDepostiViewModel>(Content);
                System.IO.File.AppendAllText(path, string.Format("第二步：{0}\r\n", JsonConvert.SerializeObject(depostiViewModel)));
                Btcamp.Gold.Core.Entitys.Deposit model = new Btcamp.Gold.Core.Entitys.Deposit();
                Account account = accountService.Get(a => a.PhoneNumber == depostiViewModel.Email);
                model.AccountId = account.Id;
                model.OrderNumber = depostiViewModel.Billno;
                decimal rate = await mt4Service.GetUSDCNY();
                model.USDAmount = Convert.ToDouble((decimal)depostiViewModel.Amount / rate);
                model.Amount = depostiViewModel.Amount;
                model.IsAudit = false;
                model.Time = DateTime.Now;
                depositService.Add(model);
                unitOfWork.Commit();
                System.IO.File.AppendAllText(path, string.Format("第三步：{0}\r\n", "ok"));
                return Json(Content, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(path, ex.ToString());
                return Json(Content, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Failure()
        {
            string path = Server.MapPath("/log");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            path = System.IO.Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".txt");
            System.IO.File.AppendAllText(path, "失败");
            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}
