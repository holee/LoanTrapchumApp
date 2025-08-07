using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Message;
using Dapper;
using Domains.Models.Lists.Customers;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public sealed class CustomerNoteRepository : ICustomerNoteService
    {
        private readonly ILogger<CustomerNoteRepository> _logger;
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly IDapperConnectionFactory _dapperFactory;
        public CustomerNoteRepository(
            ILogger<CustomerNoteRepository> logger,
            IDbContextFactory<DataContext> contextFactory,
            IDapperConnectionFactory dapperFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
            _dapperFactory = dapperFactory;
        }

        public async Task<Result<string>> AddCustomerNote(CustomerNote model,CancellationToken cancellation = default)
        {
            try
            {
                _logger.LogInformation("try adding new customer note");

                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

                context.CustomerNote.Add(model);

                await context.SaveChangesAsync(cancellation);

                return Result.Success(Messages.DataCreated);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return Result.Failure<string>(ErrorResponse.ServerError(e.Message));
            }
        }
        public async Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(string? customerId, CancellationToken cancellation = default)
           where T : class
        {
            _logger.LogInformation("try adding new customer note");
            try
            {
                using var connection = await _dapperFactory.CreateConnectionAsync(cancellation);

                var sql = @"
                     SELECT 
	                    c.NoteId,
	                    c.CustomerRefId,
	                    CASE
		                    WHEN c.CreditAgentName IS NULL THEN 'Administrator'
		                    ELSE c.CreditAgentName
	                    END AS CreditAgentName,
	                    c.Description,
	                    c.Created
                    FROM VCustomerNotes c WHERE c.CustomerRefId=@customerId";

                var notes = await connection.QueryAsync<T>(sql, new { customerId });

                if (notes.Any())
                {
                    return Result.Success(notes);
                }

                return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("There is no any notes."));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Result.Failure<IEnumerable<T>>(ErrorResponse.ServerError(e.Message));
            }
        }
    }
}