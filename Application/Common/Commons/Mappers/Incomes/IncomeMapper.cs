using Application.Features.Incomes.Command.CreateIncome;
using Application.Features.Incomes.Command.CreateIncomes;
using Application.Features.Incomes.Dto;
using Application.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Application.Commons.Mappers.Incomes;

public static class IncomeMapper
{
    private static CustomerIncome ToDto(this CreateIncomeDto source)
    {

        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new CustomerIncome
        {
            Amount = source.Amount,
            IncomeSource = source.IncomeSource,
            CustomerRefId = source.CustomerId,
            Description = source.Description
        };
    }

    public static CustomerIncome ToDto(this CreateIncomeCommand sources)
    {

        if (sources is null)
            throw new ArgumentNullException(nameof(sources));

        return sources.Request.ToDto();
    }

    public static List<CustomerIncome> ToDtos(this CreateIncomesCommand sources)
    {

        if (sources is null)
            throw new ArgumentNullException(nameof(sources));

        return sources.Request.Select(source => source.ToDto()).ToList();
    }



    public static EditIncomeDto ToIncomeEdit(this CreateIncomeDto source)
    {

        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new EditIncomeDto
        {
            Id = source.Id,
            Amount = source.Amount,
            IncomeSource = source.IncomeSource,
            CustomerId = source.CustomerId,
            Description = source.Description
        };
    }

    public static EditIncomeDto ToIncomeEdit(this VCustomerIncomes source)
    {

        if (source is null)
            throw new ArgumentNullException(nameof(source));
        return new EditIncomeDto
        {
            Id = source.IncomeId,
            Amount = source.IncomeAmount,
            IncomeSource = source.IncomeSource,
            CustomerId = source.CustomerRefId,
            Description = source.InDescription
        };
    }

    public static EditIncomeDto ToIncomeEdit(this CustomerIncome source)
    {

        if (source is null)
            throw new ArgumentNullException(nameof(source));
        return new EditIncomeDto
        {
            Id = source.Id,
            Amount = source.Amount,
            IncomeSource = source.IncomeSource,
            CustomerId = source.CustomerRefId,
            Description = source.Description
        };
    }
}
