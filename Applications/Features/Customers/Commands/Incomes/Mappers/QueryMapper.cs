using Applications.Features.Customers.Commands.Incomes.Dto;
using Applications.Responses.ViewModels.Customers;

namespace Applications.Features.Customers.Commands.Incomes.Mappers
{
    public static class QueryMapper
    {
        public static CreateIncomeDto ToCreateIncomeDto(this VCustomerIncomes income)
        {
            return new CreateIncomeDto
            {
                Id = income.IncomeId,
                CustomerId = income.CustomerRefId,
                IncomeSource = income.IncomeSource,
                Amount = income.IncomeAmount,
                Description = income.InDescription
            };
        }
    }
}
