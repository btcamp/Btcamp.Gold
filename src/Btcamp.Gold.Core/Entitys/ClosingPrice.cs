using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Entitys
{
    [Table("ClosingPrice")]
    public class ClosingPrice : BaseEntity
    {
        public decimal Price { get; set; }
    }
}
