using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.Commands.IncomeCommands.EditIncomeCommands;

public static class UpdateIncomeCommandMapper
{
    public static CustomerIncome ToDto(this IncomeDtoUpdate source)
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

    public static CustomerIncome ToDto(this UpdateIncomeCommand source)
    {
        return source.Request.ToDto();
    }



    public static List<CustomerIncome> ToDtos(this UpdateIncomesCommand source)
    {
        return source.Requests.Select(x => x.ToDto()).ToList();
    }


}
