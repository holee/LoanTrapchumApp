using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;
using Infrastructures.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NoteService> _logger;
        public NoteService(ApplicationDbContext context, ILogger<NoteService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<string>> AddNotesAsync(CustomerNote customerNote)
        {
            _logger.LogInformation("Add new note..");
            try
            {
                _context.CustomerNote.Add(customerNote);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("new note awas added"));
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
        public async Task<Result<string>> UpdateNotesAsync(CustomerNote customerNote)
        {
            try
            {
                _logger.LogInformation("updating note..");
                _context.CustomerNote.Update(customerNote);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("note was updated"));
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
        public async Task<Result<IEnumerable<VCustomerNotes>>> GetCustomerNotesAsync(long customerId, int noteId = 0)
        {
            try
            {
                _logger.LogInformation("try to get note..");

                IQueryable<VCustomerNotes> noteQueries = _context.Database
                                                .SqlQuery<VCustomerNotes>($"SELECT * FROM VCustomerNotes")
                                                .Where(x => x.CustomerRefId == customerId.ToString());
                if (noteId > 0)
                {
                    noteQueries = noteQueries.Where(x => x.NoteId == noteId);
                }
                if (!noteQueries.Any())
                {
                    return Result.Failure<IEnumerable<VCustomerNotes>>(ErrorResponse.NotFound("no any note found."));

                }
                var items = await noteQueries.AsNoTracking().ToListAsync();

                return Result.Success<IEnumerable<VCustomerNotes>>(items);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<IEnumerable<VCustomerNotes>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
        public async Task<Result<VCustomerNotes>> GetCustomerNoteAsync(int noteId)
        {
            try
            {
                _logger.LogInformation("try to get note..");

                var sql = "SELECT * FROM VCustomerNotes WHERE noteId=@noteId";

                var note = await _context.Database.SqlQueryRaw<VCustomerNotes>(sql, new { noteId = noteId })
                                                  .FirstOrDefaultAsync();

                if (note is null) return Result.Failure<VCustomerNotes>(ErrorResponse.NotFound("No note found."));

                return Result.Success<VCustomerNotes>(note);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result.Failure<VCustomerNotes>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }

}
