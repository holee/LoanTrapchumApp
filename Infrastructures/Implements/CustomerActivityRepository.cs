using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Message;
using Domains.Models.Lists.Customers;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public sealed class CustomerActivityRepository : ICustomerActivityService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;

        private readonly ILogger<CustomerActivityRepository> _logger;

        public CustomerActivityRepository(IDbContextFactory<DataContext> contextFactory, ILogger<CustomerActivityRepository> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Result<string>> AddCustomerActivity(CustomerActivity model, CancellationToken cancellation = default)
        {
            _logger.LogInformation("try adding new customer note");
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();

                context.CustomerActivites.Add(model);

                await context.SaveChangesAsync(cancellation);

                return Result.Success(Messages.DataCreated);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return Result.Failure<string>(ErrorResponse.ServerError(e.Message));
            }
        }
        public async Task<Result<IEnumerable<T>>> GetCustomerActivies<T>(string? customerId, CancellationToken cancellation = default) where T : class
        {
            _logger.LogInformation("try adding new customer note");
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);
                var notes = await context.Database
                    .SqlQuery<T>($"SELECT * FROM VCustomerActivities WHERE CustomerRefId={customerId}")
                    .ProjectToType<T>()
                    .AsNoTracking()
                    .ToListAsync();

                if (notes.Any())
                {
                    return Result.Success<IEnumerable<T>>(notes);
                }

                return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("Something went wrong with server."));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Result.Failure<IEnumerable<T>>(ErrorResponse.ServerError(e.Message));
            }
        }
    }
}