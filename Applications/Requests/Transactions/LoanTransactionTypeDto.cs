namespace Applications.Requests.Transactions
{
    public class LoanTransactionTypeDto
    {
        public int TransactionTypeId { get; set; }
        public string TransactionCode { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsCommission { get; set; } = false;
    }
}
