using Applications.Commons.Mappers.Collaterals;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Mapper;
using Applications.Interfaces.Customers;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Applications.Services.Lists; 

public sealed class CollateralsService : ICollateralsServiceImpl
{
    private readonly ICollateralsService collateralsService;
    private readonly ICollateralService cuscollateralsService;
    private readonly ILogger<CollateralsService> logger;

    public CollateralsService(ICollateralsService collateralsService,
            ICollateralService cuscollateralsService,
            IValidator<CollateralDtoCreate> collValidator,
            ILogger<CollateralsService> logger)
    {
        this.collateralsService = collateralsService;
        this.cuscollateralsService = cuscollateralsService;
        this.logger = logger;
    }

    public async Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param)
    {
        return await collateralsService.GetCollateralsAysnc(param);
    }

    public async Task<IEnumerable<Features.Collaterals.Dtos.CollateralSubTypeDto>> GetCollateralSubTypes(int? id = 0, string? searchText = null)
    {
        return await collateralsService.GetCollateralSubTypes(id, searchText);
    }

    public async Task<IEnumerable<CollateralTypeDto>> GetCollateralTypes(int? collateralId = 0, string? q = null)
    {
        return await collateralsService.GetCollateralTypes(collateralId, q);
    }

    public async Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null)
    {
        return await collateralsService.GetsCollateralAysnc(q);
    }

    public async Task<ErrorOr<string>> CreateAsync(CollateralDtoCreate entity, bool detarching = false)
    {

        logger.LogInformation("Creating collateral: {Collateral}", entity);

        var insertModel = entity.MapCollateral();

        return await cuscollateralsService.AddOrUpdateAsync(insertModel, detarching);

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

    public async Task<ErrorOr<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId)
    {
        logger.LogInformation("Fetching collateral by ID: {CollateralId}", collateralId);

        return await cuscollateralsService.GetCollateralDetailAsync(collateralId);
    }

    Task<Result<VCustomerCollaterals>> ICollateralsServiceImpl.GetCollateralByIdAsync(string? collateralId)
    {
        throw new NotImplementedException();
    }

    Task<Result<string>> ICollateralsServiceImpl.CreateAsync(CollateralDtoCreate entity, bool detarching)
    {
        throw new NotImplementedException();
    }
}
