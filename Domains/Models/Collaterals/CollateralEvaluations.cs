using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domains.Models.Collaterals
{
    [Table("lt_CollateralEvaluations")]
    public class CollateralEvaluations
    {

        public long Id { get; set; }
        public string? CollateralRefId { get; set; }
        public int CurrencyRefId { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? ValueBy { get; set; }
        public decimal? ValueAmount { get; set; }
        
    }
}
