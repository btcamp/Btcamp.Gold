using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class AccountChangePasswordViewModel
    {
        [Display(Name = "旧密码")]
        [Required(ErrorMessage = "请输入旧密码")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(36, MinimumLength = 6, ErrorMessage = "密码长度只能是6-20位的字符长度")]
        public string OldPwd { get; set; }

        [Display(Name = "新密码")]
        [Required(ErrorMessage = "请输入新密码")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(36, MinimumLength = 6, ErrorMessage = "密码长度只能是6-20位的字符长度")]
        public string NewPwd { get; set; }

        [Display(Name = "重复密码")]
        [Required(ErrorMessage = "重复登陆密码")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(36, MinimumLength = 6, ErrorMessage = "密码长度只能是6-20位的字符长度")]
        [Compare("NewPwd", ErrorMessage = "两次密码不一致")]
        public string ConfirmPwd { get; set; }

        public Guid AccountId { get; set; }

    }
}