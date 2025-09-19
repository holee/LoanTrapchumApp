using Applications.Features.Collaterals.Mapper;
using Applications.Features.Collaterals.Requests;
using Applications.Features.Customers.Mappers;
using Applications.Interfaces.Customers;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Applications.Services.Lists; 

public sealed class CollateralsService(ICollateralsService collateralsService,
        ICustomerCollateralService cuscollateralsService,
        IValidator<CollateralDtoCreate> collValidator,
        ILogger<CollateralsService> logger) : ICollateralsServiceImpl
{
    public async Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param)
    {
        return await collateralsService.GetCollateralsAysnc(param);
    }

    public async Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id = 0, string? searchText = null)
    {
        return await collateralsService.GetCollateralSubTypes(id, searchText);
    }

    public async Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralId = 0, string? q = null)
    {
        return await collateralsService.GetCollateralTypes(collateralId, q);
    }

    public async Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null)
    {
        return await collateralsService.GetsCollateralAysnc(q);
    }

    public async Task<Result<string>> CreateAsync(CollateralDtoCreate entity, bool detarching = false)
    {

        logger.LogInformation("Creating collateral: {Collateral}", entity);

        var validationResult = collValidator.Validate(entity);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToDictionary();

            return Result.Failure<string>(ErrorResponse.Validation(errors));
        }

        var insertModel = entity.MapCollateral();

        return await cuscollateralsService.InsertOrUpdate(insertModel, detarching);

    }


    public async Task<Result<string>> UpsertAsync(CollateralSubTypeDtoCreate entity)
    {

        var insertModel = entity.MapCollateralSubTypeCreate();

        return await collateralsService.Upsert(insertModel);

    }
    public Task<Result<string>> UpdateAsync(CollateralSubType entity)
    {
        logger.LogInformation("Updating collateral subtype: {CollateralSubType}", entity);

        return collateralsService.InsertOrUpdateAsync(entity);
    }

    public async Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId)
    {
        logger.LogInformation("Fetching collateral by ID: {CollateralId}", collateralId);

        if (string.IsNullOrWhiteSpace(collateralId))
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.Validation("Collateral ID is required"));
        }
        return await cuscollateralsService.GetCollateralByIdAsync(collateralId);
    }
}
