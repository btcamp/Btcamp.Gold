﻿using AutoMapper;
using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Areas.Admin.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        IAccountService _accountServie = null;
        IUnitOfWork _unitOfWork = null;
        public AccountController(IAccountService accountService, IUnitOfWork unitOfWork)
        {
            this._accountServie = accountService;
            this._unitOfWork = unitOfWork;
        }
        // GET: Admin/Account
        public ActionResult Index(int pageIndex = 1, int pageSize = 20)
        {
            Page page = new Page(pageIndex, pageSize);
            IPagedList<Account> list = _accountServie.GetPage(page, e => true, e => e.UpdateTime, true);
            return View(list);
        }

        [HttpGet]
        public ActionResult Modify(Guid? Id)
        {
            ViewBag.Url = RedirectUrl;
            Account account = _accountServie.GetById(Id.Value);
            AccountViewModel model = Mapper.Map<AccountViewModel>(account);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modify(AccountViewModel model)
        {
            ResponseModel response = new ResponseModel();
            response.RedirectUrl = Request["ReturnUlr"];
            Account account = Mapper.Map<Account>(model);
            _accountServie.Update(account);
            _unitOfWork.Commit();
            response.Success = true;
            response.Msg = "成功修改账户信息";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Del(Guid? Id)
        {
            ResponseModel response = new ResponseModel();
            if (Id != null)
            {
                Account account = _accountServie.GetById(Id.Value);
                _accountServie.Delete(account);
                _unitOfWork.Commit();
                response.Success = true;
                response.Msg = "删除成功！";
                response.RedirectUrl = RedirectUrl;
            }
            else
            {
                response.Success = false;
                response.Msg = "无效的请求";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            ResponseModel response = new ResponseModel();
            Account account = _accountServie.GetById(model.Id);
            account.LoginPwd = model.Password.ToMd5String();
            _accountServie.Update(account);
            _unitOfWork.Commit();
            response.Msg = "重置密码成功！";
            response.Success = true;
            response.RedirectUrl = string.Empty;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(Guid? Id)
        {
            Account account = _accountServie.GetById(Id.Value);
            var model = Mapper.Map<AccountViewModel>(account);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public override string RedirectUrl
        {
            get { return Request.UrlReferrer == null ? Url.Action("Index") : Request.UrlReferrer.AbsolutePath; }
        }
    }
}