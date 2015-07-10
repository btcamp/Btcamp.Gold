using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btcamp.Gold.Core.Entitys
{
    [Table("Withdrawals")]
    public class Withdrawals : BaseEntity
    {

        public double Amount { get; set; }

        public DateTime Time { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        public bool IsAudit { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        public Guid AccountId { get; set; }
    }
}
