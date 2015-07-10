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

namespace Btcamp.Gold.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly IAccountService _accountService = null;
        public AccountController(IUnitOfWork unitOfWork, IAccountService accountService)
        {
            this._accountService = accountService;
            this._unitOfWork = unitOfWork;
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
            AccountViewModel accountviewModel = Mapper.Map<AccountViewModel>(account);
            if (account != null)
            {
                string result = JsonConvert.SerializeObject(accountviewModel);
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
    }
}