using Domains.Models.Lists.Customers;

namespace Applications.Features.Incomes.Dto;

public sealed class CreateIncomeDto
{
    public CreateIncomeDto()
    {
    }

    public CreateIncomeDto(string customerId)
    {
        CustomerId= customerId ?? throw new ArgumentNullException(nameof(customerId));
    }

    public CreateIncomeDto(long id, string? customerId, string? incomeSource, decimal? incomeAmount,
        string? inDescription)
    {
        Id = id;
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        IncomeSource = incomeSource;
        Amount = incomeAmount < 0 ? 0 : incomeAmount;
        Description = inDescription;
    }

    public CreateIncomeDto(string customerId, string? incomeSource, decimal? incomeAmount, string? inDescription)
    {
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        IncomeSource = incomeSource;
        Amount = incomeAmount < 0 ? 0 : incomeAmount;
        Description = inDescription;
    }

    public long Id { get; set; }
    public string? CustomerId { get; set; }
    public string? IncomeSource { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }

    public static CustomerIncome Create()
    {
        return new CustomerIncome
        {
             //Id=this.Id,
             //CustomerId=this.CustomerId,
             //IncomeSource=this.IncomeSource,
             //Amount=this.Amount,
             //Description=this.Description   
        };
    }
}