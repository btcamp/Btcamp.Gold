using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Controllers
{
    public class BankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly IAccountService _accountService = null;
        public BankController(IUnitOfWork unitOfWork, IAccountService accountService)
        {
            this._accountService = accountService;
            this._unitOfWork = unitOfWork;
        }
        // GET: Bank
        public ActionResult Index()
        {
            var account = _accountService.GetById(LoginAccount.Id);
            if (string.IsNullOrEmpty(account.Bank) || string.IsNullOrEmpty(account.BankNumber))
            {
                //没有绑定银行 提示用户绑定银行卡
                return RedirectToAction("Add");
            }
            else
            {
                Models.AccountViewModel viewModel = AutoMapper.Mapper.Map<Models.AccountViewModel>(account);
                return View(viewModel);
            }
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Models.BankViewModel model)
        {
            ResponseModel response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Msg = ModelState.Keys.FirstOrDefault();
            }
            else
            {
                var account = _accountService.GetById(LoginAccount.Id);
                account.BankBranch = model.BankBranch;
                account.Name = model.Name;
                account.IDNumber = model.IDNumber;
                account.Bank = model.Bank;
                account.BankNumber = model.BankNumber;
                _accountService.Update(account);
                _unitOfWork.Commit();
                response.Success = true;
                response.RedirectUrl = Url.Action("Index");
                response.Msg = "成功添加银行卡！";
            }
            return Json(response);
        }
        [HttpGet]
        public ActionResult Modify()
        {
            BankViewModel model = new BankViewModel();
            Account account = _accountService.GetById(LoginAccount.Id);
            model.Bank = account.Bank;
            model.BankBranch = account.BankBranch;
            model.BankNumber = account.BankNumber;
            model.IDNumber = account.IDNumber;
            model.Name = account.Name;
            return View(model);
        }

        public ActionResult Modify(Models.BankViewModel model)
        {
            ResponseModel response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Msg = ModelState.Keys.FirstOrDefault();
            }
            else
            {
                var account = _accountService.GetById(LoginAccount.Id);
                account.BankBranch = model.BankBranch;
                account.Bank = model.Bank;
                account.BankNumber = model.BankNumber;
                _accountService.Update(account);
                _unitOfWork.Commit();
                response.Success = true;
                response.RedirectUrl = Url.Action("Index");
                response.Msg = "成功修改银行卡！";
            }
            return Json(response);
        }
    }
}