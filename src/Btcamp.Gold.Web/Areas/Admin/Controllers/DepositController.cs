using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Areas.Admin.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Areas.Admin.Controllers
{
    public class DepositController : BaseController
    {
        private readonly IDepositService depositService = null;

        private readonly IMT4Service mt4Service = null;

        private readonly IAccountService accountService = null;

        private readonly IUnitOfWork unitOfWork = null;


        public DepositController(IUnitOfWork _unitOfWork, IDepositService _depositService, IAccountService _accountService, IMT4Service _mt4Service)
        {
            this.depositService = _depositService;
            this.mt4Service = _mt4Service;
            this.accountService = _accountService;
            this.unitOfWork = _unitOfWork;
        }
        public override string RedirectUrl
        {
            get { return Request.UrlReferrer == null ? Url.Action("Index") : Request.UrlReferrer.AbsolutePath; }
        }
        // GET: Admin/Deposit
        public ActionResult Index(int pageIndex = 1, int pageSize = 20)
        {
            Page page = new Page(pageIndex, pageSize);
            IPagedList<Deposit> list = depositService.GetPageAsNoTracking(page, e => true, e => e.IsAudit, false);
            return View(list);
        }


        public async Task<ActionResult> Check(Guid? id)
        {
            ResponseModel response = new ResponseModel();
            Deposit model = depositService.GetById(id.Value);
            //1.入金mt4金额
            //2.修改account的金额
            //3.修改审核状态
            double amount = Math.Round(model.USDAmount, 2);
            bool flg = await mt4Service.ModifyBalance(model.Account.MT4Account, amount);
            if (flg)
            {
                Account account = accountService.GetById(model.AccountId);
                account.Amount += amount;
                model.IsAudit = true;
                accountService.Update(account);
                depositService.Update(model);
                unitOfWork.Commit();
                response.Msg = "成功审核入金申请!";
                response.Success = true;
                response.RedirectUrl = RedirectUrl;
            }
            else
            {
                response.Msg = "链接交易服务器失败！请重试...";
                response.Success = false;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


    }
}