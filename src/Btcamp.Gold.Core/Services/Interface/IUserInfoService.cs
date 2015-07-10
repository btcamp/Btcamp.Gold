using Btcamp.Gold.Core.Entitys;
using Btcamp.Gold.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Services.Interface
{
    public interface IUserInfoService : IService<UserInfo>
    {
        UserInfo UserLoginByEmail(string email, string pwd);
        UserInfo GetUserByEmail(string email);
    }
}
