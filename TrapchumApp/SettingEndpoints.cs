using Applications.Features.AdditionalInformations.Command.CreateOccupation;
using Applications.Features.AdditionalInformations.Command.UpdateOccupation;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.AdditionalInformations.Queries.GetOccupationDetailsById;
using Applications.Features.AdditionalInformations.Queries.GetOccupations;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Incomes.Command.CreateIncomeExpenseSource;
using Applications.Features.Incomes.Command.DeleteIncomeExpense;
using Applications.Features.Incomes.Command.EditIncomeExpenseSource;
using Applications.Features.Incomes.Command.GetEditIncomeExpenseSource;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Features.LoanPurposes;
using Applications.Features.LoanPurposes.Commands.CreateLoanPurpose;
using Applications.Features.LoanPurposes.Commands.DeleteLoanPurpose;
using Applications.Features.LoanPurposes.Commands.UpdateLoanPurpose;
using Applications.Features.LoanPurposes.Queries.GetLoanPurposeById;
using Applications.Features.LoanPurposes.Queries.GetLoanPurposes;
using Applications.Features.Periods.Commands.CreatePeriod;
using Applications.Features.Periods.Commands.DeletePeriod;
using Applications.Features.Periods.Commands.UpdatePeriod;
using Applications.Features.Periods.Dto;
using Applications.Features.Periods.Queries.GetPeriods;
using Applications.Features.Settings.Commands.AppSettings.CreateAppSetting;
using Applications.Features.Settings.Commands.AppSettings.DeleteAppSetting;
using Applications.Features.Settings.Commands.AppSettings.UpdateAppSetting;
using Applications.Features.Settings.Dto;
using Applications.Features.Settings.Queries.AppSettings.GetAppSettings;
using Applications.Features.Settings.Queries.CollateralSubTypes.GetCollateralSubTypes;
using Applications.Interfaces.Audits;
using Applications.Interfaces.Lists;
using Applications.Responses.Audits;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Applications.Utils;
using Domains.Enumerations;
using Domains.Models.Collaterals;
using Domains.Shared.Enums;

namespace TrapchumApp;

/// <summary>
/// Minimal API surface for the Settings area consumed by the Web.UI (Blazor WebAssembly) client.
/// Every route delegates to the existing application handlers, so the server stays the single
/// source of truth for validation and persistence.
/// </summary>
internal static class SettingEndpoints
{
    public static RouteGroupBuilder MapSettingEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/settings")
                             .WithTags("Settings")
                             .RequireAuthorization();

        // ------------------------------------------------------------- app settings

        group.MapGet("/", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            string? filter = null,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new GetAppSettingsQuery(filter), cancellationToken);

                return result.IsError
                    ? Results.Ok(Array.Empty<AppSettingDto>())
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the application settings.");
            }
        })
        .Produces<IEnumerable<AppSettingDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetAppSettings");

        group.MapPost("/", async Task<IResult> (
            AppSettingDtoCreate setting,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new CreateAppSettingCommand(setting), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to create the application setting.");
            }
        })
        .DisableAntiforgery()
        .Produces<AppSettingDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("CreateAppSetting");

        group.MapPut("/{id:int}", async Task<IResult> (
            int id,
            AppSettingDtoUpdate setting,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                setting.Id = id;

                var result = await mediator.Send(new UpdateAppSettingCommand(setting), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to update the application setting {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<AppSettingDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpdateAppSetting");

        group.MapDelete("/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new DeleteAppSettingCommand(id), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(new SettingMessageResponse($"Setting {id} removed."));
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to delete the application setting {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("DeleteAppSetting");

        // ------------------------------------------------------- collateral settings

        group.MapGet("/collateral-types", async Task<IResult> (
            ICollateralsService collateralService,
            ILogger<Program> logger,
            int? id = null,
            string? q = null) =>
        {
            try
            {
                var result = await collateralService.GetCollateralTypes(id, q);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the collateral types.");
            }
        })
        .Produces<IEnumerable<CollateralTypeDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetCollateralTypes");

        group.MapGet("/collateral-subtypes", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int page = 0,
            int take = 15,
            string? sortColumn = "Id",
            bool sortDescending = false,
            bool includeInactive = true,
            string? filter = null,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var param = new QueryParam
                {
                    Page = page,
                    Take = take,
                    SortColumn = sortColumn,
                    SortDescending = sortDescending,
                    IncludeInActive = includeInactive,
                    Filter = filter ?? string.Empty
                };

                var result = await mediator.Send(new GetCollateralSubTypesQuery(param), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the collateral sub-types.");
            }
        })
        .Produces<Pager<CollateralSubTypeDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetCollateralSubTypes");

        group.MapPost("/collateral-subtypes", async Task<IResult> (
            CollateralSubTypeDtoCreate subType,
            ICollateralsServiceImpl collateralService,
            ILogger<Program> logger) =>
        {
            try
            {
                var result = await collateralService.UpsertAsync(subType);

                return result.IsSuccess
                    ? Results.Ok(new SettingMessageResponse(result.Data ?? "Saved."))
                    : Results.Problem(detail: result.Errors?.ErrMessage ?? "Unable to save the collateral sub-type.",
                                      statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to save the collateral sub-type.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpsertCollateralSubTypeDto");

        group.MapPut("/collateral-subtypes/{id:int}", async Task<IResult> (
            int id,
            CollateralSubTypeDto subType,
            ICollateralsService collateralService,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var entity = new CollateralSubType
                {
                    Id = id,
                    CollateralSubTypeName = subType.CollateralSubTypeName,
                    CollateralSubTypeNameInKhmer = subType.CollateralSubTypeNameInKhmer,
                    CollateralTypeRefId = subType.CollateralTypeRefId,
                    ValueRate = subType.ValueRate,
                    LoanSizeAmuntMin = subType.LoanSizeAmuntMin,
                    LoanSizeAmuntMax = subType.LoanSizeAmuntMax,
                    IsActive = subType.IsActive
                };

                var result = await collateralService.Upsert(entity, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(new SettingMessageResponse(result.Data ?? "Updated."))
                    : Results.Problem(detail: result.Errors?.ErrMessage ?? "Unable to update the collateral sub-type.",
                                      statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to update the collateral sub-type {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpdateCollateralSubType");

        // ----------------------------------------------------------------- periods

        group.MapGet("/periods", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int page = 0,
            int take = 15,
            string? sortColumn = "Id",
            bool sortDescending = false,
            string? filter = null,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var param = new QueryParam
                {
                    Page = page,
                    Take = take,
                    SortColumn = sortColumn,
                    SortDescending = sortDescending,
                    IncludeInActive = false,
                    Filter = filter ?? string.Empty
                };

                var result = await mediator.Send(new GetPeriodsQuery(param), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the periods.");
            }
        })
        .Produces<Pager<PeriodDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetPeriods");

        group.MapPost("/periods", async Task<IResult> (
            PeriodDtoCreate period,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new CreatePeriodCommand(period), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to create the period.");
            }
        })
        .DisableAntiforgery()
        .Produces<PeriodDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("CreatePeriod");

        group.MapPut("/periods/{id:int}", async Task<IResult> (
            int id,
            PeriodDtoUpdate period,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                period.Id = id;

                var result = await mediator.Send(new UpdatePeriodCommand(period), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to update the period {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<PeriodDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpdatePeriod");

        group.MapDelete("/periods/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new DeletePeriodCommand(id), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(new SettingMessageResponse($"Period {id} deleted."));
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to delete the period {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("DeletePeriod");

        // ------------------------------------------------------------ loan purposes

        group.MapGet("/loan-purposes", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int page = 0,
            int take = 15,
            string? sortColumn = "Id",
            bool sortDescending = false,
            string? filter = null,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var param = new QueryParam
                {
                    Page = page,
                    Take = take,
                    SortColumn = sortColumn,
                    SortDescending = sortDescending,
                    IncludeInActive = false,
                    Filter = filter ?? string.Empty
                };

                var result = await mediator.Send(new GetLoanPurposesQuery(param), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the loan purposes.");
            }
        })
        .Produces<Pager<LoanPurposeDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetLoanPurposes");

        group.MapGet("/loan-purposes/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new GetLoanPurposeByIdQuery(id), cancellationToken);

                return result is null
                    ? Results.Problem(detail: $"Loan purpose {id} was not found.",
                                      statusCode: StatusCodes.Status404NotFound)
                    : Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to load the loan purpose {id}.");
            }
        })
        .Produces<LoanPurposeDto>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetLoanPurposeById");

        group.MapPost("/loan-purposes", async Task<IResult> (
            LoanPurposeDtoCreate loanPurpose,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new CreateLoanPurposeCommand(loanPurpose), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to create the loan purpose.");
            }
        })
        .DisableAntiforgery()
        .Produces<LoanPurposeDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("CreateLoanPurpose");

        group.MapPut("/loan-purposes/{id:int}", async Task<IResult> (
            int id,
            LoanPurposeDtoUpdate loanPurpose,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                loanPurpose.Id = id;

                var result = await mediator.Send(new UpdateLoanPurposeCommand(loanPurpose), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to update the loan purpose {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<LoanPurposeDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpdateLoanPurpose");

        group.MapDelete("/loan-purposes/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new DeleteLoanPurposeCommand(id), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(new SettingMessageResponse($"Loan purpose {id} deleted."));
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to delete the loan purpose {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("DeleteLoanPurpose");

        // ------------------------------------------------------------- occupations

        group.MapGet("/occupations", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int? id = 0,
            int page = 0,
            int take = 15,
            string? sortColumn = "OccupationId",
            bool sortDescending = false,
            bool includeInactive = false,
            string? filter = null,
            TypeOfEnum type = TypeOfEnum.All,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var param = new QueryParam
                {
                    Page = page,
                    Take = take,
                    SortColumn = sortColumn,
                    SortDescending = sortDescending,
                    IncludeInActive = includeInactive,
                    Filter = filter ?? string.Empty
                };

                var result = await mediator.Send(new GetOccupationsQuery(id, param, type), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the occupations.");
            }
        })
        .Produces<Pager<OccupationDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetOccupations");

        group.MapGet("/occupations/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new GetOccupationDetailsByIdQuery(id), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status404NotFound)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to load the occupation {id}.");
            }
        })
        .Produces<OccupationDto>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetOccupationById");

        group.MapPost("/occupations", async Task<IResult> (
            OccupationDtoCreate occupation,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new CreateOccupationCommand(occupation), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to create the occupation.");
            }
        })
        .DisableAntiforgery()
        .Produces<OccupationDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("CreateOccupation");

        group.MapPut("/occupations/{id:int}", async Task<IResult> (
            int id,
            OccupationDtoUpate occupation,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                occupation.OccupationId = id;

                var result = await mediator.Send(new UpdateOccupationCommand(occupation), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to update the occupation {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<OccupationDto>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpdateOccupation");

        // -------------------------------------------------------- income / expenses

        group.MapGet("/income-expenses", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int? id = null,
            int page = 0,
            int take = 15,
            string? sortColumn = "Types",
            bool sortDescending = false,
            string? filter = null,
            IncomeExpenseTypes type = IncomeExpenseTypes.All,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var param = new QueryParam
                {
                    Page = page,
                    Take = take,
                    SortColumn = sortColumn,
                    SortDescending = sortDescending,
                    IncludeInActive = false,
                    Filter = filter ?? string.Empty
                };

                var result = await mediator.Send(new GetIncomeExpenseQuery(id, type, param), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the income/expense sources.");
            }
        })
        .Produces<Pager<IncomeExpenseDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetIncomeExpenses");

        group.MapGet("/income-expenses/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new GetEditIncomeExpenseQuery(id), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status404NotFound)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to load the income/expense source {id}.");
            }
        })
        .Produces<IncomeExpenseDto>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetIncomeExpenseById");

        group.MapPost("/income-expenses", async Task<IResult> (
            CreateIncomeExpenseDto incomeExpense,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new CreateIncomeExpenseCommand(incomeExpense), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(new SettingMessageResponse(result.Value.ToString()));
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to create the income/expense source.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("CreateIncomeExpense");

        group.MapPut("/income-expenses/{id:int}", async Task<IResult> (
            int id,
            CreateIncomeExpenseDto incomeExpense,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                incomeExpense.Id = id;

                var result = await mediator.Send(new EditIncomeExpenseCommand(incomeExpense), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(new SettingMessageResponse(result.Value.ToString()));
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to update the income/expense source {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpdateIncomeExpense");

        group.MapDelete("/income-expenses/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new DeleteIncomeExpenseCommand(id), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(new SettingMessageResponse(result.Value));
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to delete the income/expense source {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<SettingMessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("DeleteIncomeExpense");

        // ------------------------------------------------------------ audit trails

        group.MapGet("/audits", async Task<IResult> (
            IAuditReadService auditService,
            ILogger<Program> logger,
            string? userId = null,
            DateOnly? from = null,
            DateOnly? to = null,
            int page = 0,
            int take = 15,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await auditService.GetAllAsync(userId, from, to, new QueryParam(page, take), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the audit trails.");
            }
        })
        .Produces<Pager<AuditVm>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetAuditTrails");

        return group;
    }

    private static IResult Failure(ILogger logger, Exception ex, string message)
    {
        logger.LogError(ex, "{Message}", message);

        return Results.Json(new { error = message, message = ex.Message },
                            statusCode: StatusCodes.Status500InternalServerError);
    }
}
