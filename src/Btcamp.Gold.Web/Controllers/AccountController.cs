using AutoMapper;
using Newtonsoft.Json;
using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using Btcamp.Gold.Core.Models;
using Btcamp.Gold.Core.EventHandler;
using Btcamp.Gold.Core.Common;

namespace Btcamp.Gold.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly IAccountService _accountService = null;
        private readonly IMT4Service _mt4Service = null;
        private readonly ISystemSettingsService _systemSettingService = null;
        private readonly IDepositService _depositService = null;
        private readonly log4net.ILog _log = null;
        public AccountController(IUnitOfWork unitOfWork,
            IAccountService accountService,
            IMT4Service mt4service,
            ISystemSettingsService systemSettingService,
            IDepositService depositService,
            log4net.ILog log)
        {
            this._accountService = accountService;
            this._mt4Service = mt4service;
            this._unitOfWork = unitOfWork;
            this._systemSettingService = systemSettingService;
            this._depositService = depositService;
            this._log = log;
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountLoginViewModel model)
        {
            if (Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            ResponseModel response = new ResponseModel();
            Account account = _accountService.AccountLoginByPhoneNumber(model.PhoneNumber, model.LoginPwd);
            if (account != null)
            {
                string result = JsonConvert.SerializeObject(new { Id = account.Id, Name = account.Name, MT4Account = account.MT4Account });
                FormsAuthentication.SetAuthCookie(result, true);
                response.Success = true;
                response.Msg = "登陆成功！";
            }
            else
            {
                response.Msg = "登陆用户名密码错误！";
            }
            return Json(response);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            ResponseModel response = new ResponseModel();
            response.Success = true;
            response.Msg = "登出成功，欢迎下次登陆！";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountViewModel model)
        {
            ResponseModel response = new ResponseModel();
            Account account = Mapper.Map<Account>(model);
            account.LoginPwd = model.LoginPwd.ToMd5String();
            _accountService.RegisterAccount(account);
            _unitOfWork.Commit();
            response.Success = true;
            response.Msg = "欢迎您，已经成功加入我们";
            CustomRaiseEvent.RaiseAccountRegister(account);//触发用户注册事件
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidePhoneNumber(string PhoneNumber)
        {
            Account account = _accountService.Get(e => e.PhoneNumber == PhoneNumber);
            return Json(account == null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Setting()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Withdrawal()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Withdrawal(WithdrawalsViewModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            AccountChangePasswordViewModel model = new AccountChangePasswordViewModel();
            model.AccountId = LoginAccount.Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(AccountChangePasswordViewModel model)
        {
            ResponseModel response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                response.Msg = ModelState.Keys.FirstOrDefault();
            }
            else
            {
                Account account = _accountService.GetById(model.AccountId);
                if (!account.LoginPwd.Equals(model.OldPwd.ToMd5String()))
                {
                    response.Msg = "旧密码与现在使用密码不一致！如需修改密码请联系客户";

                }
                else
                {
                    account.LoginPwd = model.NewPwd.ToMd5String();
                    _accountService.Update(account);
                    _unitOfWork.Commit();
                    response.Success = true;
                    response.Msg = "成功修改密码，请重新登录";
                    FormsAuthentication.SignOut();
                }
            }
            return Json(response);
        }

        public async Task<ActionResult> Tradelogs()
        {
            if (!string.IsNullOrEmpty(LoginAccount.MT4Account))
            {
                List<TradingModel> logs = await _mt4Service.GetTradeLogs(LoginAccount.MT4Account);
                List<TradingViewModel> trades = Mapper.Map<List<TradingViewModel>>(logs);
                return View(trades);
            }
            return View(new List<TradingViewModel>());
        }

        [HttpGet]
        public ActionResult ModifyInfo()
        {

            Account Information = _accountService.GetById(LoginAccount.Id);
            AccountInfoViewModel viewModel = new AccountInfoViewModel();
            viewModel.Amount = Information.Amount;
            viewModel.Name = Information.Name;
            viewModel.PhoneNumber = Information.PhoneNumber;
            viewModel.Email = Information.Email;
            viewModel.Id = Information.Id;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ModifyInfo(AccountInfoViewModel model)
        {
            ResponseModel response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    if (item.Value.Errors.Count > 0)
                    {
                        response.Success = false;
                        response.Msg = item.Value.Errors.FirstOrDefault().ErrorMessage;
                    }
                }
            }
            else
            {
                Account account = _accountService.GetById(model.Id);
                account.UpdateTime = DateTime.Now;
                account.Name = model.Name;
                account.PhoneNumber = model.PhoneNumber;
                account.Email = model.Email;
                _accountService.Update(account);
                _unitOfWork.Commit();
                response.Success = true;
                response.Msg = "已成功修改个人信息";
                response.RedirectUrl = Url.Action("Setting", "Account");
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            return Json(response);

        }

        #region Deposit

        public ActionResult Deposit()
        {
            //http://shypet.cn/Payment/Index?Username=测试&Phone=15208377468&Email=22335114101112@qq.com&Linkphone=88163584&Successurl=http://www.fino.trade/umbraco/Api/Payment/PaySuccess&Failurl=http://www.fino.trade/umbraco/Api/Payment/PayFailure&Paytype=2&Formurl=https://pay.ecpss.com/sslpayment&USD=1.00&CNY=6.2210
            Account account = _accountService.GetManyAsNoTracking(e => e.Id == LoginAccount.Id).FirstOrDefault();
            string payurl = _systemSettingService.Get(e => e.Key == "pay").Info;
            string paytype = _systemSettingService.Get(e => e.Key == "Paytype").Info;
            string failure = _systemSettingService.Get(e => e.Key == "Failure").Info;
            string successurl = _systemSettingService.Get(e => e.Key == "Successurl").Info;
            string formurl = _systemSettingService.Get(e => e.Key == "Formurl").Info;
            //email 赋值成的Phone联系方式，支付接口那边以email为唯一建操作的
            ViewBag.Url = string.Format("{0}?Username={1}&Email={2}&LinkPhone={3}&successurl={4}&Failurl={5}&Paytype={6}&Formurl={7}", payurl, account.Name, account.PhoneNumber, account.MT4Account, successurl, failure, paytype, formurl);
            return View();
        }

        public ActionResult Success(string Content, bool isServerCall = false)
        {
            Content = DESHelper.DecryptDES(Content);
            _log.DebugFormat("第一步：{0}", Content);
            AccountDepostiViewModel depostiViewModel = JsonConvert.DeserializeObject<AccountDepostiViewModel>(Content);
            _log.DebugFormat("第二步：{0}", JsonConvert.SerializeObject(depostiViewModel));
            Btcamp.Gold.Core.Entitys.Deposit model = new Btcamp.Gold.Core.Entitys.Deposit();
            Account account = _accountService.Get(a => a.PhoneNumber == depostiViewModel.Email);
            model.AccountId = account.Id;
            model.Amount = depostiViewModel.Amount;
            model.IsAudit = false;
            _depositService.Add(model);
            _unitOfWork.Commit();
            _log.DebugFormat("第三步：{0}", "ok");
            return Json(Content, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}