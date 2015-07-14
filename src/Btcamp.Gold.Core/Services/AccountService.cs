using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btcamp.Gold.Core.MT4;
using System.Configuration;

namespace Btcamp.Gold.Core.Services
{
    public class AccountService : ServiceBase<Account>, Interface.IAccountService
    {
        public AccountService(IRepository<Account> repository)
            : base(repository)
        {

        }
        public Account AccountLoginByPhoneNumber(string phoneNumber, string pwd)
        {
            string md5pwd = pwd.ToMd5String();
            return base.Get(e => e.PhoneNumber == phoneNumber && e.LoginPwd == md5pwd);
        }

        public void RegisterAccount(Account account)
        {
            string mt4Pwd = Guid.NewGuid().ToString().Substring(0, 6);
            string investorPwd = Guid.NewGuid().ToString().Substring(0, 6);
            var mt4Account = new UserRecordSE();
            mt4Account.Name = account.PhoneNumber;
            mt4Account.PasswordInvestor = investorPwd;
            mt4Account.Enable = 1;
            mt4Account.EnableChangePassword = 1;
            mt4Account.Password = mt4Pwd;
            mt4Account.Group = ConfigurationManager.AppSettings.Get("mt4:group") ?? "1";
            MT4OperResult result = MT4Service.Instance.NewUser(mt4Account);
            if (result.ErrorCode == -1)
            {
                throw new Exception(result.ErrorDescription);
            }
            else
            {
                account.MT4Pwd = mt4Pwd;
                account.MT4InvestorPwd = investorPwd;
                account.MT4Account = result.ReturnValue.ToString();
            }
            base.Add(account);
        }
    }
}
