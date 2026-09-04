using Application.Features.Incomes.Dto;
using Application.Responses.ViewModels.Customers;

namespace Application.Commons.Mappers.Incomes
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
