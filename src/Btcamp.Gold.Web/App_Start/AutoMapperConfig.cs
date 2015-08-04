using Btcamp.Gold.Core.Entitys;
using Admin = Btcamp.Gold.Web.Areas.Admin.Models;
using Btcamp.Gold.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web
{
    public class AutoMapperConfig
    {
        public static void RegisterMapper()
        {
            #region Admin Model Mapper
            AutoMapper.Mapper.CreateMap<UserInfo, Admin.UserViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.UserViewModel, UserInfo>();

            AutoMapper.Mapper.CreateMap<Account, Admin.AccountViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.AccountViewModel, Account>();

            AutoMapper.Mapper.CreateMap<Address, Admin.AddressViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.AddressViewModel, Address>();

            AutoMapper.Mapper.CreateMap<Deposit, Admin.DepositViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.DepositViewModel, Deposit>();

            AutoMapper.Mapper.CreateMap<Withdrawals, Admin.WithdrawalsViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.WithdrawalsViewModel, Withdrawals>();

            AutoMapper.Mapper.CreateMap<About, Admin.AboutViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.AboutViewModel, About>();

            AutoMapper.Mapper.CreateMap<SystemSettings, Admin.SystemSettingsViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.SystemSettingsViewModel, SystemSettings>();

            AutoMapper.Mapper.CreateMap<SystemSettings, Admin.SystemSettingsKeyViewModel>();
            AutoMapper.Mapper.CreateMap<Admin.SystemSettingsKeyViewModel, SystemSettings>();
            #endregion

            #region Web Model Mapper
            AutoMapper.Mapper.CreateMap<Account, AccountViewModel>();
            AutoMapper.Mapper.CreateMap<AccountViewModel, Account>();

            AutoMapper.Mapper.CreateMap<Address, AddressViewModel>();
            AutoMapper.Mapper.CreateMap<AddressViewModel, Address>();

            AutoMapper.Mapper.CreateMap<Deposit, DepositViewModel>();
            AutoMapper.Mapper.CreateMap<DepositViewModel, Deposit>();

            AutoMapper.Mapper.CreateMap<Withdrawals, WithdrawalsViewModel>();
            AutoMapper.Mapper.CreateMap<WithdrawalsViewModel, Withdrawals>();

            AutoMapper.Mapper.CreateMap<Btcamp.Gold.Core.Models.TradingModel, TradingViewModel>();
            #endregion
        }
    }
}