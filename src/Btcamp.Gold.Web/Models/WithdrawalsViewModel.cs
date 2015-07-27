using Btcamp.Gold.Core.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class WithdrawalsViewModel : BaseViewModel
    {
        public WithdrawalsViewModel()
        {
            Time = DateTime.Now;
        }
        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        [Required(ErrorMessage = "请输入金额")]
        public double Amount { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        public bool IsAudit { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入姓名")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "姓名长度为2-32个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [Display(Name = "银行卡号")]
        [Required(ErrorMessage = "请输入银行卡号")]
        [RegularExpression(@"^\d+$", ErrorMessage = "请输入正确的银行卡号")]
        [StringLength(64, ErrorMessage = "银行卡卡号过长")]
        public string BankNumber { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [Display(Name = "银行")]
        [Required(ErrorMessage = "请输入银行")]
        [StringLength(64, MinimumLength = 2, ErrorMessage = "银行名称过长")]
        public string Bank { get; set; }

        public virtual Account Account { get; set; }
        public Guid AccountId { get; set; }
    }
}