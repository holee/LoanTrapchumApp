using Applications.Features.NbcBank.Dto;
using Applications.Features.NbcBank.Services;

namespace TrapchumApp;

internal static class NbcEndpoints
{
    public static RouteGroupBuilder MapNbcExchangeRateEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/exchangerate")
            .WithTags("ExchangeRate");

        group.MapGet("/", async Task<IResult> (
            INbcExchangeRateService exchangeRateService,
            ILogger<Program> logger,
            string? lang = "en",
            bool refresh = false,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await exchangeRateService.GetExchangeRatesAsync(lang, refresh, cancellationToken);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching exchange rates from NBC.");
                return Results.Json(new
                {
                    error = "Failed to retrieve exchange rates from National Bank of Cambodia.",
                    message = ex.Message
                }, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .Produces<NbcExchangeRateResponse>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetExchangeRates");

        group.MapGet("/latest", async Task<IResult> (
            INbcExchangeRateService exchangeRateService,
            ILogger<Program> logger,
            string? lang = "en",
            bool refresh = false,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var result = await exchangeRateService.GetExchangeRatesAsync(lang, refresh, cancellationToken);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching exchange rates from NBC.");
                return Results.Json(new
                {
                    error = "Failed to retrieve exchange rates from National Bank of Cambodia.",
                    message = ex.Message
                }, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .Produces<NbcExchangeRateResponse>()
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetLatestExchangeRates");

        /// <summary>
        /// Gets a list of all available currency codes and names.
        /// </summary>
        /// <param name="lang">Language: 'en' or 'km'.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        group.MapGet("/currencies", async Task<IResult> (
            INbcExchangeRateService exchangeRateService,
            ILogger<Program> logger,
            string? lang = "en",
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var data = await exchangeRateService.GetExchangeRatesAsync(lang, false, cancellationToken);
                var currencies = new List<object>
                {
                    new { code = "USD", name = lang == "km" ? "ដុល្លារអាមេរិក" : "US Dollar", symbol = "USD/KHR" },
                    new { code = "KHR", name = lang == "km" ? "រៀលកម្ពុជា" : "Cambodian Riel", symbol = "KHR/KHR" }
                };

                currencies.AddRange(data.Rates.Select(r => new
                {
                    code = r.CurrencyCode,
                    name = r.CurrencyName,
                    symbol = r.Symbol
                }));

                return Results.Ok(currencies);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting currency list.");
                return Results.Json(new { error = ex.Message },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetAvailableCurrencies");

        /// <summary>
        /// Gets the exchange rate for a specific currency code (e.g. USD, EUR, THB, JPY).
        /// </summary>
        /// <param name="code">3-letter currency code (e.g. USD, THB, EUR, JPY, SGD).</param>
        /// <param name="lang">Language: 'en' or 'km'.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        group.MapGet("/{code}", async Task<IResult> (
            string code,
            INbcExchangeRateService exchangeRateService,
            ILogger<Program> logger,
            string? lang = "en",
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var rate = await exchangeRateService.GetRateByCurrencyCodeAsync(code, lang, cancellationToken);
                if (rate == null)
                {
                    return Results.NotFound(new { error = $"Currency code '{code.ToUpperInvariant()}' was not found in NBC exchange rates." });
                }
                return Results.Ok(rate);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching rate for currency {Code}", code);
                return Results.Json(new { error = ex.Message },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .Produces<CurrencyRate>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetByCurrencyCode");

        group.MapGet("/convert", async Task<IResult> (
            INbcExchangeRateService exchangeRateService,
            ILogger<Program> logger,
            string from = "USD",
            string to = "KHR",
            decimal amount = 1,
            string? lang = "en",
            CancellationToken cancellationToken = default) =>
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            {
                return Results.BadRequest(new { error = "Parameters 'from' and 'to' currency codes are required." });
            }

            if (amount < 0)
            {
                return Results.BadRequest(new { error = "Amount must be greater than or equal to zero." });
            }

            try
            {
                var result = await exchangeRateService.ConvertCurrencyAsync(from, to, amount, lang, cancellationToken);
                return Results.Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error converting {Amount} {From} to {To}", amount, from, to);
                return Results.Json(new { error = ex.Message },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .Produces<ConversionResult>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("ConvertCurrency");

        return group;
    }
}
