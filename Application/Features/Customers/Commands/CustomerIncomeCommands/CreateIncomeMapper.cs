using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Models.Lists.Customers;

namespace Application.Features.Customers.Commands.CustomerIncomeCommands
{
    internal static class CreateIncomeMapper
    {
        internal static CustomerIncome ToCreateCommand(this CreateIncomeCommand  customerIncome)
        {
            return new CustomerIncome
            {
                CustomerRefId = customerIncome.CustomerRefId,
                IncomeSource = customerIncome.IncomeSource,
                Amount = customerIncome.Amount,
                Description = customerIncome.Description
            };
        }
    }
}
