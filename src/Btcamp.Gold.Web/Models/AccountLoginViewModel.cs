using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class AccountLoginViewModel
    {
        [Display(Name = "手机号")]
        [Required(ErrorMessage = "请输入手机号")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的手机号")]
        public string PhoneNumber { get; set; }

        [Display(Name = "登陆密码")]
        [Required(ErrorMessage = "请输入登陆密码")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(36, MinimumLength = 6, ErrorMessage = "密码长度只能是6-20位的字符长度")]
        public string LoginPwd { get; set; }
    }
}