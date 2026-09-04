namespace Application.Features.LoanPayments.Dtos;

public sealed class RecievePaymentResult
{
    public string? RecieveFrom { get; init; } = string.Empty;

    public DateTime? RecieveOn { get; init; } = DateTime.UtcNow;

    public decimal? Amount { get; init; } = 0;

    public static RecievePaymentResult Create(string? recieveFrom, DateTime? recieveOn, decimal? amount)
    {
        return new RecievePaymentResult
        {
            RecieveFrom = recieveFrom,
            RecieveOn = recieveOn,
            Amount = amount
        };
    }

    public static RecievePaymentResult Success(string? recieveFrom, decimal? amount) 
    {
        return new RecievePaymentResult
        {
            RecieveFrom = recieveFrom,
            Amount = amount
        };
    }


}
