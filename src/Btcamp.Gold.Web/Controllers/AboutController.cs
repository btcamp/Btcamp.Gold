using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Controllers
{
    public class AboutController : BaseController
    {
        private IAboutService _aboutService = null;
        public AboutController(IAboutService aboutService)
        {
            this._aboutService = aboutService;
        }

        // GET: About
        public ActionResult Index()
        {
            About Information = _aboutService.GetAll().OrderByDescending(e=>e.UpdateTime).FirstOrDefault();
            AboutViewModel viewModel = new AboutViewModel();
            viewModel.PhoneNumber = Information.PhoneNumber;
            viewModel.QQGroupNumber = Information.QQGroupNumber;
            viewModel.WeChatNumber = Information.WeChatNumber;
            viewModel.VersionNumber = Information.VersionNumber;
            viewModel.Contents = Information.Contents;
            viewModel.PhotoUrl = Information.PhotoUrl;
            return View(viewModel);
        }
    }
}