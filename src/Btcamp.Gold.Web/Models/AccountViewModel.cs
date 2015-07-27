using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class AccountViewModel : BaseViewModel
    {
        [Display(Name = "手机号")]
        [Required(ErrorMessage = "请输入手机号")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的手机号")]
        [System.Web.Mvc.Remote("ValidePhoneNumber", "Account", ErrorMessage = "登陆手机号已经被注册，可直接登陆")]
        public string PhoneNumber { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "登陆密码")]
        [Required(ErrorMessage = "请输入登陆密码")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(36, MinimumLength = 6, ErrorMessage = "密码长度只能是6-20位的字符长度")]
        public string LoginPwd { get; set; }

        [Display(Name = "重复密码")]
        [Required(ErrorMessage = "重复登陆密码")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(36, MinimumLength = 6, ErrorMessage = "密码长度只能是6-20位的字符长度")]
        [Compare("LoginPwd", ErrorMessage = "两次密码不一致")]
        public string ConfirmPwd { get; set; }

        [Display(Name="开户人")]
        [StringLength(32)]
        public string Name { get; set; }
        [Display(Name = "身份证号")]
        [StringLength(18)]
        public string IDNumber { get; set; }
        [Display(Name = "开户行")]
        [StringLength(64)]
        public string Bank { get; set; }
        [Display(Name = "银行卡号")]
        [StringLength(64)]
        public string BankNumber { get; set; }

        /// <summary>
        /// 开户行 分行
        /// </summary>
        [Display(Name = "开户行分行")]
        [StringLength(64)]
        public string BankBranch { get; set; }

        [StringLength(32)]
        public string MT4Pwd { get; set; }

        [StringLength(32)]
        public string MT4InvestorPwd { get; set; }

        [Display(Name = "金额")]
        public double Amount { get; set; }

        public string MT4Account { get; set; }

        public virtual ICollection<DepositViewModel> Deposits { get; set; }

        public virtual ICollection<WithdrawalsViewModel> Withdrawals { get; set; }

        public virtual ICollection<AddressViewModel> Address { get; set; }
        public bool IsLock { get; set; }
    }

    public class AccountLoginStatusModel : BaseViewModel
    {
        public string Name { get; set; }
        public string MT4Account { get; set; }
    }
}