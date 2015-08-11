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
    public class WithdrawalController : BaseController
    {
        private readonly IWithdrawalsService withdrawalService = null;

        private readonly IMT4Service mt4Service = null;

        private readonly IAccountService accountService = null;

        private readonly IUnitOfWork unitOfWork = null;
        public WithdrawalController(
            IUnitOfWork _unitOfWork,
            IWithdrawalsService _withdrawalService,
            IAccountService _accountService,
            IMT4Service _mt4Service)
        {
            this.withdrawalService = _withdrawalService;
            this.mt4Service = _mt4Service;
            this.accountService = _accountService;
            this.unitOfWork = _unitOfWork;

        }
        public override string RedirectUrl
        {
            get { return Request.UrlReferrer == null ? Url.Action("Index") : Request.UrlReferrer.AbsolutePath; }
        }
        // GET: Admin/Withdrawal
        public ActionResult Index(int pageIndex = 1, int pageSize = 20)
        {
            Page page = new Page(pageIndex, pageSize);
            IPagedList<Withdrawals> list = withdrawalService.GetPageAsNoTracking(page, e => true, e => e.UpdateTime, true);
            return View(list);
        }


        public async Task<ActionResult> Check(Guid? id)
        {
            ResponseModel response = new ResponseModel();
            Withdrawals model = withdrawalService.GetById(id.Value);
            Account account = model.Account;
            if (model.Amount > account.Amount)
            {
                response.Msg = "提现金额已超限!";
                response.Success = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //1.入金mt4金额
            //2.修改account的金额
            //3.修改审核状态
            bool flg = await mt4Service.ModifyBalance(model.Account.MT4Account, model.Amount * -1);
            if (flg)
            {
                account.Amount -= model.Amount;
                model.IsAudit = true;
                accountService.Update(account);
                withdrawalService.Update(model);
                unitOfWork.Commit();
                response.Msg = "成功审核提现申请!";
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