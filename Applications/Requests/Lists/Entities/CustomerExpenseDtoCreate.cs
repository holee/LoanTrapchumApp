namespace Applications.Requests.Lists.Entities;

public class CustomerExpenseDtoCreate
{
    public CustomerExpenseDtoCreate()
    {
    }

    public CustomerExpenseDtoCreate(long id, string? customerRefId, string? expenseTitle, decimal? amount, string? description)
    {
        Id = id;
        CustomerRefId = customerRefId;
        ExpenseTitle = expenseTitle;
        Amount = amount;
        Description = description;
    }

    public long Id { get; set; }
    public string? CustomerRefId { get; set; } 
    public string? ExpenseTitle { get; set; }
    public decimal? Amount { get; set; } 
    public string? Description { get; set; }
}
