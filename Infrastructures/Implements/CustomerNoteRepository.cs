using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Dapper;
using Domains.Models.Lists.Customers;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Result = Applications.Responses.Result;

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

        public async Task<ErrorOr<int>> AddOrUpdateNote(CustomerNote model, CancellationToken cancellation = default)
        {
            try
            {
                _logger.LogInformation("try adding new customer note");

                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

                if (model.NoteId > 0)
                {
                    context.CustomerNote.Update(model);
                }
                else
                {
                    context.CustomerNote.Add(model);
                }

                await context.SaveChangesAsync(cancellation);

                return model.NoteId;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Error.Unexpected(e.Message);
            }
        }

        public async Task<ErrorOr<bool>> DeleteNote(int? noteId, string? customerId, CancellationToken cancellation = default)
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var note = await context.CustomerNote
                            .FirstOrDefaultAsync(x => x.NoteId == noteId &&
                                                      x.CustomerRefId == customerId);

            if (note is null)
            {
                return Error.NotFound("Note not found.");
            }

            context.CustomerNote.Remove(note);

            await context.SaveChangesAsync(cancellation);

            return true;

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



        public async Task<ErrorOr<VCustomerNotes>> GetNoteById(int? id, string? customerId, CancellationToken cancellation = default)
        {
            _logger.LogInformation("try adding new customer note");

            try
            {
                
                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);
               
                var note = await context 
                                       .Database
                                       .SqlQuery<VCustomerNotes>($@"SELECT * FROM VCustomerNotes WHERE CustomerRefId={customerId}")
                                       .AsNoTracking()
                                       .Select(ProjectTo())
                                       .FirstOrDefaultAsync(cancellation);
                if (note is not null)
                {
                    return note;
                }

                return Error.NotFound("Note not found.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return Error.Unexpected(e.Message);
            }
        }

        public async Task<ErrorOr<IEnumerable<VCustomerNotes>>> GetNotes(string? customerId, CancellationToken cancellation = default)
        {
            _logger.LogInformation("Trying to retrieve customer note with  for Customer {CustomerId}", customerId);

            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

                var notes = await context
                                        .Database
                                        .SqlQuery<VCustomerNotes>($@"SELECT * FROM VCustomerNotes WHERE CustomerRefId={customerId}")
                                        .AsNoTracking()
                                        .Select(ProjectTo())
                                        .ToListAsync(cancellation);

                if (!notes.Any())

                    return Error.NotFound("Note.NotFound", "Note not found.");

                return notes;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while retrieving note with ID {customerId}", customerId);
                
                return Error.Unexpected("UnexpectedError", e.Message);
            }
        }

        private static Expression<Func<VCustomerNotes, VCustomerNotes>> ProjectTo()
        {
            return c => new VCustomerNotes
            {
                NoteId = c.NoteId,
                CustomerRefId = c.CustomerRefId,
                CreditAgentName = c.CreditAgentName ?? "Administrator",
                Description = c.Description,
                Created = c.Created
            };
        }
    }
}




//using var connection = await _dapperFactory.CreateConnectionAsync(cancellation);
//var sql = @"
//     SELECT 
//     c.NoteId,
//     c.CustomerRefId,
//     CASE
//      WHEN c.CreditAgentName IS NULL THEN 'Administrator'
//      ELSE c.CreditAgentName
//     END AS CreditAgentName,
//     c.Description,
//     c.Created
//    FROM VCustomerNotes c WHERE 
//         NoteId=@noteId AND c.CustomerRefId=@customerId";
//var note = await connection.QueryFirstOrDefaultAsync<VCustomerNotes>(sql,
//    new
//    {
//        customerId,
//        NoteId = id
//    });