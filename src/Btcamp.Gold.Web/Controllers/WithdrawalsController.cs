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
        public WithdrawalsController(IUnitOfWork _unitOfWork, IWithdrawalsService _withdrawalsService)
        {
            this.withdrawalsService = _withdrawalsService;
            this.unitOfWork = _unitOfWork;
        }
        // GET: Withdrawals
        public ActionResult Index()
        {
            Page page = new Page(1, 10);
            List<Withdrawals> list = withdrawalsService.GetPageAsNoTracking(page, e => true, e => e.UpdateTime, true).ToList();
            List<WithdrawalsViewModel> result = Mapper.Map<List<WithdrawalsViewModel>>(list);
            return View(result);
        }
    }
}