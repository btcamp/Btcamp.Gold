using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class AccountInfoViewModel : BaseViewModel
    {
        [Display(Name = "姓名")]
        [StringLength(32,ErrorMessage="输入有效的姓名")]
        public string Name { get; set; }
        //ww@qq.com
        [Display(Name = "手机号")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的手机号")]
        public string PhoneNumber { get; set; }

        [Display(Name = "邮箱")]
        [RegularExpression(@".+@\..+",ErrorMessage="输入正确的邮箱地址")]
        public string Email { get; set; }

        [Display(Name = "总资产")]
        public double Amount { get; set; }


    }
}