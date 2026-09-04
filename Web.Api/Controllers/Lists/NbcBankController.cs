using Application.Features.NbcBank.Dto;
using Application.Features.NbcBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class NbcBankController(INbcExchangeRateService nbcExchangeRateService) : ApiControllerBase
{
    [HttpGet("exchange-rates")]
    public async Task<IActionResult> GetExchangeRates(
        [FromQuery] string? language = "en",
        [FromQuery] bool forceRefresh = false,
        CancellationToken cancellationToken = default)
    {
        var result = await nbcExchangeRateService.GetExchangeRatesAsync(language, forceRefresh, cancellationToken);
        return Ok(result);
    }

    [HttpGet("exchange-rates/{currencyCode}")]
    public async Task<IActionResult> GetRateByCurrencyCode(
        string currencyCode,
        [FromQuery] string? language = "en",
        CancellationToken cancellationToken = default)
    {
        var result = await nbcExchangeRateService.GetRateByCurrencyCodeAsync(currencyCode, language, cancellationToken);
        if (result is null)
        {
            return NotFound(new { error = $"Currency rate for '{currencyCode}' not found." });
        }
        return Ok(result);
    }

    [HttpGet("convert")]
    public async Task<IActionResult> ConvertCurrency(
        [FromQuery] string fromCurrency,
        [FromQuery] string toCurrency,
        [FromQuery] decimal amount,
        [FromQuery] string? language = "en",
        CancellationToken cancellationToken = default)
    {
        var result = await nbcExchangeRateService.ConvertCurrencyAsync(fromCurrency, toCurrency, amount, language, cancellationToken);
        return Ok(result);
    }
}
