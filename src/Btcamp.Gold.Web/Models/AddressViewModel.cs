using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class AddressViewModel : BaseViewModel
    {
        [StringLength(32)]
        [Display(Name = "收货人姓名")]
        [Required(ErrorMessage = "请输入收货人姓名")]
        public string Name { get; set; }

        [StringLength(11)]
        [Display(Name = "联系电话")]
        [Required(ErrorMessage = "请输入联系电话")]
        public string PhoneNumber { get; set; }

        [StringLength(6)]
        [Display(Name = "邮编")]
        [Required(ErrorMessage = "请输入邮编")]
        public string ZipCode { get; set; }

        [StringLength(128)]
        [Display(Name = "详细收货地址")]
        [Required(ErrorMessage = "请输入详细收货地址")]
        public string DetailAddress { get; set; }

        public string AccountId { get; set; }
    }
}