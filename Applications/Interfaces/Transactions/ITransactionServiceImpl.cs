namespace Applications.Interfaces.Transactions
{
    public interface ITransactionServiceImpl  
    {
        Task<IEnumerable<dynamic>> GetAllTransactionsTypesAsync();   
    }
}
