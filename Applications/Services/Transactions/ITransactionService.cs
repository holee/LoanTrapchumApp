namespace Applications.Services.Transactions
{
    public interface ITransactionService
    {
        Task<IEnumerable<dynamic>> GetAllTransactionsTypesAsync();   
    }
}
