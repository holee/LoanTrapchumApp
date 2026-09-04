using Applications.Commons;
using Applications.Features.Currencies;
using Applications.Features.Currencies.Command.ActivateOrDeactivateNote;
using Applications.Features.Currencies.Command.AddCurrencyNote;
using Applications.Features.Currencies.Command.AddNewCurrency;
using Applications.Features.Currencies.Command.SetSpotRate;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Currencies.Queries.GetCurrenciesAsPaging;
using Applications.Features.Currencies.Queries.GetCurrencyCounter;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Queries.GetSpotRates;
using Applications.Features.Currencies.Responses;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace TrapchumApp;

/// <summary>
/// Minimal API surface consumed by the Web.UI (Blazor WebAssembly) client.
/// Every route delegates to the existing application handlers, so the server
/// stays the single source of truth for validation and persistence.
/// </summary>
internal static class CurrencyEndpoints
{
    public static RouteGroupBuilder MapCurrencyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/currencies")
                             .WithTags("Currencies")
                             .RequireAuthorization();

        // ---------------------------------------------------------------- currencies

        group.MapGet("/", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int page = 0,
            int take = 15,
            string? sortColumn = "CurrencyCode",
            bool sortDescending = false,
            bool includeInactive = false,
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

                var result = await mediator.Send(new GetCurrencyAsPaging(param), cancellationToken);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the currency list.");
            }
        })
        .Produces<Pager<VCurrenciesList>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetCurrenciesPaged");

        group.MapGet("/list", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int? currencyId = 0,
            int page = 0,
            int take = 0,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(
                    new GetCurrenciesQuery(currencyId, new QueryParam(page, take)), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the currency lookup list.");
            }
        })
        .Produces<PageMetaData<CurrencyDto>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetCurrencyLookupList");

        group.MapGet("/{id:int}", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new GetCurrencyByIdQuery(id), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status404NotFound)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to load currency {id}.");
            }
        })
        .Produces<CurrencyDto>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetCurrencyById");

        group.MapPost("/", async Task<IResult> (
            CurrencyDtoCreate currency,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new AddNewCurrencyCommand(currency), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to create the currency.");
            }
        })
        .DisableAntiforgery()
        .Produces<VCurrenciesList>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("CreateCurrency");

        // ---------------------------------------------------------------- spot rates

        group.MapGet("/{id:int}/spot-rates", async Task<IResult> (
            int id,
            IMediator mediator,
            ILogger<Program> logger,
            int page = 0,
            int take = 15,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(
                    new GetSpotRatesQuery(id, new QueryParam(page, take)), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to load spot rates for currency {id}.");
            }
        })
        .Produces<PageMetaData<CurrencyDtoSpotRate>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetCurrencySpotRates");

        group.MapPost("/{id:int}/spot-rate", async Task<IResult> (
            int id,
            CurrencySpotRateDtoCreate spotRate,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new SetSpotRateCommand(id, spotRate), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to set the spot rate for currency {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<SpotRateDtoCreate>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("SetCurrencySpotRate");

        // ---------------------------------------------------------------- notes

        group.MapGet("/notes", async Task<IResult> (
            IMediator mediator,
            ILogger<Program> logger,
            int? currencyId = 0,
            int? branchId = 0,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(
                    new GetCurrencyCounterQuery(currencyId, branchId), cancellationToken);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to load the currency notes.");
            }
        })
        .Produces<Pager<VCurrencyCounter>>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetCurrencyNotes");

        group.MapPost("/notes", async Task<IResult> (
            CurrencyNoteDtoCreate note,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(new AddCurrencyNoteCommand(note), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, "Unable to create the currency note.");
            }
        })
        .DisableAntiforgery()
        .Produces<VCurrencyCounter>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("CreateCurrencyNote");

        group.MapPatch("/notes/{id:int}/toggle", async Task<IResult> (
            int id,
            NoteToggleRequest request,
            IMediator mediator,
            ILogger<Program> logger,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await mediator.Send(
                    new ToggleNoteCommand(id, new NoteActivateDto(request.IsActive)), cancellationToken);

                return result.IsError
                    ? Results.Problem(detail: result.FirstError.Description,
                                      statusCode: StatusCodes.Status400BadRequest)
                    : Results.Ok(new MessageResponse(result.Value));
            }
            catch (Exception ex)
            {
                return Failure(logger, ex, $"Unable to toggle the currency note {id}.");
            }
        })
        .DisableAntiforgery()
        .Produces<MessageResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("ToggleCurrencyNote");

        return group;
    }

    private static IResult Failure(ILogger logger, Exception ex, string message)
    {
        logger.LogError(ex, "{Message}", message);

        return Results.Json(new { error = message, message = ex.Message },
                            statusCode: StatusCodes.Status500InternalServerError);
    }
}
