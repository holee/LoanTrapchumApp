namespace Applications.Requests.Lists.Entities;

public class CustomerExpenseDtoCreate
{
    public CustomerExpenseDtoCreate()
    {
    }

    public CustomerExpenseDtoCreate(long id, Guid? customerRefId, string? expenseTitle, decimal? amount, string? description)
    {
        Id = id;
        CustomerRefId = customerRefId;
        ExpenseTitle = expenseTitle;
        Amount = amount;
        Description = description;
    }

    public long Id { get; set; }
    public Guid? CustomerRefId { get; set; } 
    public string? ExpenseTitle { get; set; }
    public decimal? Amount { get; set; } 
    public string? Description { get; set; }
}
