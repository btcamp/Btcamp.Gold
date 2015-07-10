using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class BankViewModel
    {
        [Display(Name = "开户人")]
        [Required(ErrorMessage="请输入开户姓名")]
        [StringLength(32)]
        public string Name { get; set; }
        [StringLength(18)]
        [Display(Name = "身份证号")]
        [Required(ErrorMessage = "请输入身份证号")]
        [RegularExpression(@"(\d{18})|(\d{17}[Xx])", ErrorMessage = "请输入有效的身份证号码")]
        public string IDNumber { get; set; }
        [StringLength(64)]
        [Display(Name = "开户行")]
        [Required(ErrorMessage = "请输入开户行")]
        public string Bank { get; set; }
        [StringLength(64)]
        [Display(Name = "银行卡号")]
        [Required(ErrorMessage = "请输入银行卡号")]
        public string BankNumber { get; set; }

        /// <summary>
        /// 开户行 分行
        /// </summary>
        [StringLength(64)]
        [Display(Name = "开户行分行")]
        [Required(ErrorMessage = "请输入开户行分行")]
        public string BankBranch { get; set; }
    }
}