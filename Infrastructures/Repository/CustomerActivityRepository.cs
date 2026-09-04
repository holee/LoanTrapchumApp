using Applications.Commons.Messages.Activities;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Result = Applications.Responses.Result;

namespace Infrastructures.Repository
{
    public sealed class CustomerActivityRepository : ICustomerActivityService
    {
        private readonly DataContext _contextFactory;

        private readonly ILogger<CustomerActivityRepository> _logger;

        public CustomerActivityRepository(DataContext contextFactory,
            ILogger<CustomerActivityRepository> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<ErrorOr<int>> AddOrUpdateActivity(CustomerActivity model, CancellationToken cancellation = default)
        {
            _logger.LogInformation("try adding new customer note");

            try
            {
                var context = _contextFactory;

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


        public async Task<ErrorOr<bool>> DeleteActivityAsync(int? id, string customerId, CancellationToken cancellation = default)
        {
            _logger.LogInformation("try adding new customer note");

            try
            {
                var context = _contextFactory;

                var model = await context.CustomerActivites.FirstOrDefaultAsync(x => x.Id == id && x.CustomerRefId == customerId, cancellation);

                if (model is null)
                {
                    return Error.NotFound(description: ActivityMessage.NotFound);
                }

                context.CustomerActivites.Remove(model);

                await context.SaveChangesAsync(cancellation);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Error.Unexpected(description: e.Message);
            }
        }


        public async Task<IEnumerable<VCustomerActivities>> GetCustomerActivies(string? customerId, CancellationToken cancellation = default)
        {
            _logger.LogInformation("try adding new customer note");

            try
            {
                var context = _contextFactory;
                var notes = await context.Set<VCustomerActivities>()
                    .AsNoTracking()
                    .Where(x => x.CustomerRefId == customerId)
                    .Select(ProjectTo())
                    .ToListAsync();

                if (notes.Any())
                {
                    return notes;
                }

                return [];
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                throw new Exception(e.Message, e);
            }
        }

        public async Task<VCustomerActivities> GetCustomerActivity(int? id, string? customerId, CancellationToken cancellationToken = default)
        {
            var context = _contextFactory;

            var activity = await context.CustomerActivites
                                                .AsNoTracking()
                                                .Select(x => new VCustomerActivities
                                                {
                                                    ActivityId = x.Id,
                                                    Description = x.Description ?? string.Empty,
                                                    ActivityDate = x.ActivityDate,
                                                    Status = x.Status ?? false,
                                                    CustomerRefId = x.CustomerRefId,
                                                })
                                                .FirstOrDefaultAsync(x => x.ActivityId == id && x.CustomerRefId == customerId);

            if (activity is not null)
            {
                return activity;
            }

            throw new Exception("Unable to find customer activity.");
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