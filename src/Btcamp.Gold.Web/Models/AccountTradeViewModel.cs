using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btcamp.Gold.Web.Models
{
    public class TradeBuyViewModel
    {
        [Display(Name = "手数")]
        [Required(ErrorMessage = "请输入购买的手数")]
        [RegularExpression(@"^\d+$", ErrorMessage = "请输入正确的手数")]
        public int Volume { get; set; }
    }

    public class TradingViewModel
    {
        public int OrderID { get; set; }
        public string TradeTime { get; set; }
        public decimal Price { get; set; }
        public double Weight { get; set; }

        public double Profit { get; set; }
    }
}