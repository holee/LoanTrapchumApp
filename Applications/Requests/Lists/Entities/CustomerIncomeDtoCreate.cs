namespace Applications.Requests.Lists.Entities;

public class CustomerIncomeDtoCreate
{
    public CustomerIncomeDtoCreate() { }

    public CustomerIncomeDtoCreate(string? incomeSource, decimal? amount, string? description)
    {
        IncomeSource = incomeSource;
        Amount = amount;
        Description = description;
    }
    public CustomerIncomeDtoCreate(long? id,Guid? customerId, string? incomeSource, decimal? amount, string? description)
    {
        Id = id;
        IncomeSource = incomeSource;
        Amount = amount;
        Description = description;
        CustomerRefId = customerId;
    }

    public long? Id { get; set; } = 0;
    public Guid? CustomerRefId { get; set; }
    public string? IncomeSource { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
}
