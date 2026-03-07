using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Dashbaord.Accountants.Dtos
{
    public sealed class AccountantLoanSummary
    {
        public int TotalCustomer { get; set; }
        public int CurrencyId { get; set; }
        public decimal? TotalPrinciple { get; set; } = 0;
        public decimal? TotalInterest { get; set; } = 0;
    }
}
