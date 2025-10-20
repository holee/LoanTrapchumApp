using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.ViewModels.Customers;
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

        public async Task<Result<string>> AddOrUpdateActivity(CustomerActivity model, CancellationToken cancellation = default)
        {
            _logger.LogInformation("try adding new customer note");

            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

                if (model.Id > 0)
                {
                    context.CustomerActivites.Update(model);
                }
                else
                {
                    context.CustomerActivites.Add(model);
                }

                await context.SaveChangesAsync(cancellation);

                return Result.Success(Messages.DataCreated);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Result.Failure<string>(ErrorResponse.ServerError(e.Message));
            }
        }

        public async Task<Result<IEnumerable<VCustomerActivities>>> GetCustomerActivies(string? customerId, CancellationToken cancellation = default)
        {
            _logger.LogInformation("try adding new customer note");

            try
            {

                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

                var notes = await context.Database
                    .SqlQuery<VCustomerActivities>($"SELECT * FROM VCustomerActivities WHERE CustomerRefId={customerId}")
                    .AsNoTracking()
                    .Select(ProjectTo())
                    .ToListAsync();

                if (notes.Any())
                {
                    return Result.Success<IEnumerable<VCustomerActivities>>(notes);
                }

                return Result.Failure<IEnumerable<VCustomerActivities>>(ErrorResponse.NotFound("Something went wrong with server."));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Result.Failure<IEnumerable<VCustomerActivities>>(ErrorResponse.ServerError(e.Message));
            }
        }

        private static System.Linq.Expressions.Expression<Func<VCustomerActivities, VCustomerActivities>> ProjectTo()
        {
            return x => new VCustomerActivities
            {
                ActivityId = x.ActivityId,
                Description = x.Description,
                Status = x.Status,
                ActivityDate = x.ActivityDate,
            };
        }
    }
}