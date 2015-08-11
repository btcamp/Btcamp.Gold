using AutoMapper;
using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Controllers
{
    public class WithdrawalsController : BaseController
    {
        private readonly IWithdrawalsService withdrawalsService = null;
        private readonly IUnitOfWork unitOfWork = null;
        private readonly IAccountService accountService = null;
        public WithdrawalsController(IUnitOfWork _unitOfWork, IWithdrawalsService _withdrawalsService, IAccountService _accountService)
        {
            this.withdrawalsService = _withdrawalsService;
            this.unitOfWork = _unitOfWork;
            this.accountService = _accountService;
        }
        // GET: Withdrawals
        public ActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            Page page = new Page(pageIndex, pageSize);
            List<Withdrawals> list = withdrawalsService.GetPageAsNoTracking(page, e => true, e => e.UpdateTime, true).ToList();
            List<WithdrawalsViewModel> result = Mapper.Map<List<WithdrawalsViewModel>>(list);
            return View(result);
        }

        [HttpGet]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(WithdrawalsViewModel model)
        {
            ResponseModel response = new ResponseModel();
            //Guid guid = Guid.NewGuid();
            Account account = accountService.GetById(LoginAccount.Id);
            if (string.IsNullOrEmpty(account.Name) || string.IsNullOrEmpty(account.Bank) || string.IsNullOrEmpty(account.BankBranch) || string.IsNullOrEmpty(account.BankNumber))
            {
                response.Success = false;
                response.Msg = "您还未进行实名认证！请完善个人信息和银行卡信息";
            }
            else
            {
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
                    Withdrawals entity = AutoMapper.Mapper.Map<Withdrawals>(model);
                    entity.AccountId = LoginAccount.Id;
                    entity.IsAudit = false;
                    withdrawalsService.Add(entity);
                    unitOfWork.Commit();
                    response.Success = true;
                    response.RedirectUrl = Url.Action("Index");
                    response.Msg = "已成功申请提现！";
                }
            }
            return Json(response);

        }
    }
}