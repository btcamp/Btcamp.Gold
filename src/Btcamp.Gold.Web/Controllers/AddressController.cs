using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Btcamp.Gold.Web.Controllers
{
    public class AddressController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly IAccountService _accountService = null;
        private readonly IAddressService _addressService = null;
        public AddressController(IUnitOfWork unitOfWork, IAccountService accountService, IAddressService addressService)
        {
            this._accountService = accountService;
            this._unitOfWork = unitOfWork;
            this._addressService = addressService;
        }
        // GET: Address
        public ActionResult Index()
        {
            List<Address> address = _addressService.GetMany(a => a.AccountId == LoginAccount.Id).ToList();
            return View();
        }
    }
}