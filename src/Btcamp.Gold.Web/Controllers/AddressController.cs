using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using Btcamp.Gold.Core.Services.Interface;
using Btcamp.Gold.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var results = AutoMapper.Mapper.Map<List<AddressViewModel>>(address);
            return View(results);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddressViewModel model)
        {
            ResponseModel response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Msg = ModelState.Keys.FirstOrDefault();
            }
            else
            {
                Address address = AutoMapper.Mapper.Map<Address>(model);
                address.Id = Guid.NewGuid();
                address.AccountId = LoginAccount.Id;//关联主外键
                _addressService.Add(address);
                _unitOfWork.Commit();
                response.Success = true;
                response.Msg = "成功添加收货地址";
                response.RedirectUrl = Url.Action("Index");
            }
            return Json(response);
        }

        [HttpGet]
        public ActionResult Modify(string id)
        {
            Address address = _addressService.GetById(Guid.Parse(id));
            AddressViewModel viewModel = AutoMapper.Mapper.Map<AddressViewModel>(address);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Modify(AddressViewModel model)
        {
            ResponseModel response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                response.Msg = ModelState.Keys.FirstOrDefault();
            }
            else
            {
                Address address = AutoMapper.Mapper.Map<Address>(model);
                _addressService.Update(address);
                _unitOfWork.Commit();
                response.Msg = "成功修改收货人信息";
                response.Success = true;
                response.RedirectUrl = Url.Action("Index");
            }
            return Json(response);
        }
    }
}