using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Result = Applications.Responses.Result;

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

        public async Task<ErrorOr<int>> AddOrUpdateActivity(CustomerActivity model, CancellationToken cancellation = default)
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

                
                return model.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Error.Unexpected(e.Message);
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

        public async Task<Result<VCustomerActivities>> GetCustomerActivity(int? id, string customerId, CancellationToken cancellationToken = default)
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            
            var activity=await context.CustomerActivites
                                                .AsNoTracking() 
                                                .Select(x=> new VCustomerActivities
                                                {
                                                    ActivityId = x.Id,
                                                    Description = x.Description,
                                                    ActivityDate = x.ActivityDate,
                                                    Status = x.Status.Value,
                                                    CustomerRefId = x.CustomerRefId,
                                                })
                                                .FirstOrDefaultAsync(x=>x.ActivityId==id && x.CustomerRefId==customerId);

            if (activity is not null)
            {
                return Result.Success<VCustomerActivities>(activity);
            }

            return Result.Failure<VCustomerActivities>(ErrorResponse.ServerError("Unable to find customer activity."));
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