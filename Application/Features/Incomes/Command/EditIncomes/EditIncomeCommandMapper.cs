using Application.Features.Incomes.Command.EditIncome;
using Application.Features.Incomes.Dto;
using Domains.Models.Lists.Customers;

namespace Application.Features.Incomes.Command.EditIncomes;

public static class EditIncomeCommandMapper
{
    public static CustomerIncome ToDto(this EditIncomeDto source)
    {
        return new CustomerIncome
        {
            Id = source.Id,
            CustomerRefId = source.CustomerId,
            IncomeSource = source.IncomeSource,
            Amount = source.Amount,
            Description = source.Description
        };
    }

    public static CustomerIncome ToDto(this EditIncomeCommand source)
    {
        return source.Request.ToDto();
    }



    public static List<CustomerIncome> ToDtos(this EditIncomesCommand source)
    {
        return source.Requests.Select(x => x.ToDto()).ToList();
    }


}
