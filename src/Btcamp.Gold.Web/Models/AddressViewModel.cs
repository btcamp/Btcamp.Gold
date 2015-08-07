using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class AddressViewModel : BaseViewModel
    {
        [StringLength(32, ErrorMessage = "收货人姓名最长为32位有效长度")]
        [Display(Name = "收货人姓名")]
        [Required(ErrorMessage = "请输入收货人姓名")]
        public string Name { get; set; }

        [StringLength(11, ErrorMessage = "联系电话最长为11位有效长度")]
        [Display(Name = "联系电话")]
        [Required(ErrorMessage = "请输入联系电话")]
        public string PhoneNumber { get; set; }

        [Display(Name = "邮编")]
        [Required(ErrorMessage = "请输入邮编")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "邮编为6位有效数字")]
        public string ZipCode { get; set; }

        [StringLength(128, ErrorMessage = "详细收货地址最长为128位有效长度")]
        [Display(Name = "详细收货地址")]
        [Required(ErrorMessage = "请输入详细收货地址")]
        public string DetailAddress { get; set; }

        public Guid AccountId { get; set; }
    }
}