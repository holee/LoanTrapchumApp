using Application.Features.NbcBank.Dto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using HtmlAgilityPack;
namespace Application.Features.NbcBank.Services;

public class NbcExchangeRateService : INbcExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<NbcExchangeRateService> _logger;

    private const string KhmerUrl = "https://www.nbc.gov.kh/economic_research/exchange_rate.php";
    private const string EnglishUrl = "https://www.nbc.gov.kh/english/economic_research/exchange_rate.php";
    private const string CacheKeyPrefix = "NBC_EXCHANGE_RATE_";
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);

    public NbcExchangeRateService(
        HttpClient httpClient,
        IMemoryCache memoryCache,
        ILogger<NbcExchangeRateService> logger)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<NbcExchangeRateResponse> GetExchangeRatesAsync(
        string? language = "en",
        bool forceRefresh = false,
        CancellationToken cancellationToken = default)
    {
        var isKhmer = string.Equals(language, "km", StringComparison.OrdinalIgnoreCase) ||
                      string.Equals(language, "kh", StringComparison.OrdinalIgnoreCase);

        var langKey = isKhmer ? "km" : "en";
        var cacheKey = $"{CacheKeyPrefix}{langKey}";

        if (!forceRefresh && _memoryCache.TryGetValue(cacheKey, out NbcExchangeRateResponse? cachedResponse) && cachedResponse != null)
        {
            _logger.LogInformation("Returning exchange rates for {Lang} from memory cache.", langKey);
            return cachedResponse;
        }

        var targetUrl = isKhmer ? KhmerUrl : EnglishUrl;
        _logger.LogInformation("Fetching exchange rates from NBC URL: {Url}", targetUrl);

        var htmlContent = await FetchHtmlAsync(targetUrl, cancellationToken);
        var parsedResponse = ParseHtml(htmlContent, targetUrl, langKey);

        _memoryCache.Set(cacheKey, parsedResponse, DefaultCacheDuration);
        return parsedResponse;
    }

    public async Task<CurrencyRate?> GetRateByCurrencyCodeAsync(
        string currencyCode,
        string? language = "en",
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            return null;

        var cleanCode = currencyCode.Trim().ToUpperInvariant();
        var data = await GetExchangeRatesAsync(language, false, cancellationToken);

        if (cleanCode == "USD")
        {
            return new CurrencyRate
            {
                CurrencyName = language?.ToLowerInvariant() == "km" ? "ដុល្លារអាមេរិក" : "US Dollar",
                CurrencyCode = "USD",
                Symbol = "USD/KHR",
                QuoteCurrency = "KHR",
                Unit = 1,
                Bid = data.OfficialRate.Rate,
                Ask = data.OfficialRate.Rate,
                Average = data.OfficialRate.Rate
            };
        }

        return data.Rates.FirstOrDefault(r =>
            string.Equals(r.CurrencyCode, cleanCode, StringComparison.OrdinalIgnoreCase) ||
            r.Symbol.StartsWith(cleanCode + "/", StringComparison.OrdinalIgnoreCase));
    }

    public async Task<ConversionResult> ConvertCurrencyAsync(
        string fromCurrency,
        string toCurrency,
        decimal amount,
        string? language = "en",
        CancellationToken cancellationToken = default)
    {
        if (amount < 0)
            throw new ArgumentException("Amount must be greater than or equal to 0.", nameof(amount));

        var from = fromCurrency.Trim().ToUpperInvariant();
        var to = toCurrency.Trim().ToUpperInvariant();

        if (from == to)
        {
            return new ConversionResult
            {
                FromCurrency = from,
                ToCurrency = to,
                OriginalAmount = amount,
                ConvertedAmount = amount,
                ExchangeRateUsed = 1.0m,
                AsOfDate = DateTime.UtcNow
            };
        }

        var data = await GetExchangeRatesAsync(language, false, cancellationToken);

        // Helper to get rate to KHR for 1 unit of foreign currency
        decimal GetRateInKhr(string code)
        {
            if (code == "KHR") return 1.0m;
            if (code == "USD") return data.OfficialRate.Rate > 0 ? data.OfficialRate.Rate : 4040.0m;

            var rateItem = data.Rates.FirstOrDefault(r =>
                string.Equals(r.CurrencyCode, code, StringComparison.OrdinalIgnoreCase) ||
                r.Symbol.StartsWith(code + "/", StringComparison.OrdinalIgnoreCase));

            if (rateItem == null)
                throw new KeyNotFoundException($"Exchange rate for currency code '{code}' was not found in NBC data.");

            return rateItem.RatePerOneUnit;
        }

        var fromToKhrRate = GetRateInKhr(from);
        var toToKhrRate = GetRateInKhr(to);

        // Value in KHR = amount * fromToKhrRate
        // Value in Target = Value in KHR / toToKhrRate
        var amountInKhr = amount * fromToKhrRate;
        var finalConverted = toToKhrRate != 0 ? amountInKhr / toToKhrRate : 0;
        var effectiveRate = toToKhrRate != 0 ? fromToKhrRate / toToKhrRate : 0;

        DateTime.TryParse(data.Date, out var asOfDate);
        if (asOfDate == default) asOfDate = DateTime.UtcNow;

        return new ConversionResult
        {
            FromCurrency = from,
            ToCurrency = to,
            OriginalAmount = amount,
            ConvertedAmount = Math.Round(finalConverted, 4),
            ExchangeRateUsed = Math.Round(effectiveRate, 6),
            AsOfDate = asOfDate
        };
    }

    private async Task<string> FetchHtmlAsync(string url, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
        request.Headers.Add("Accept-Language", "en-US,en;q=0.9,km;q=0.8");

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        return Encoding.UTF8.GetString(bytes);
    }

    private NbcExchangeRateResponse ParseHtml(string html, string sourceUrl, string language)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var result = new NbcExchangeRateResponse
        {
            SourceUrl = sourceUrl,
            Language = language,
            RetrievedAtUtc = DateTime.UtcNow
        };

        // 1. Extract Date
        // Try datepicker input value first: <input type="text" name="exdate" id="datepicker" ... value="2026-08-26"/>
        var dateInput = doc.DocumentNode.SelectSingleNode("//input[@name='exdate']");
        if (dateInput != null)
        {
            var dateVal = dateInput.GetAttributeValue("value", string.Empty).Trim();
            if (!string.IsNullOrEmpty(dateVal))
            {
                result.Date = ConvertKhmerDigitsToAscii(dateVal);
            }
        }

        if (string.IsNullOrEmpty(result.Date))
        {
            result.Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        }

        // 2. Extract Official Rate (USD to KHR)
        // Look for text matching Official Exchange Rate / អត្រាផ្លូវការ
        var allFontNodes = doc.DocumentNode.SelectNodes("//font[@color='#FF3300']");
        if (allFontNodes != null)
        {
            foreach (var fontNode in allFontNodes)
            {
                var text = fontNode.InnerText.Trim();
                var cleanAscii = ConvertKhmerDigitsToAscii(text);

                // If it's a numeric rate (like 4047)
                if (decimal.TryParse(cleanAscii, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedRate) && parsedRate > 1000)
                {
                    result.OfficialRate = new OfficialExchangeRate
                    {
                        BaseCurrency = "USD",
                        TargetCurrency = "KHR",
                        Rate = parsedRate,
                        Description = language == "km"
                            ? $"អត្រាផ្លូវការ : {cleanAscii} រៀល / ដុល្លារអាមេរិក"
                            : $"Official Exchange Rate : {cleanAscii} KHR / USD"
                    };
                    break;
                }
            }
        }

        // 3. Extract Exchange Rates Table
        var tableNode = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'tbl-responsive')]") ??
                        doc.DocumentNode.SelectSingleNode("//table[.//tr[contains(., 'Symbol') or contains(., 'និមិត្តសញ្ញា')]]");

        if (tableNode != null)
        {
            var rows = tableNode.SelectNodes(".//tr");
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells == null || cells.Count < 6) continue;

                    // Skip header row
                    var firstCellText = HtmlEntity.DeEntitize(cells[0].InnerText).Trim();
                    if (firstCellText.Equals("Currency", StringComparison.OrdinalIgnoreCase) ||
                        firstCellText.Equals("រូបិយប័ណ្ណ", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var currencyName = HtmlEntity.DeEntitize(cells[0].InnerText).Trim();
                    var symbol = HtmlEntity.DeEntitize(cells[1].InnerText).Trim();
                    var unitStr = ConvertKhmerDigitsToAscii(HtmlEntity.DeEntitize(cells[2].InnerText).Trim());
                    var bidStr = ConvertKhmerDigitsToAscii(HtmlEntity.DeEntitize(cells[3].InnerText).Trim());
                    var askStr = ConvertKhmerDigitsToAscii(HtmlEntity.DeEntitize(cells[4].InnerText).Trim());
                    var avgStr = ConvertKhmerDigitsToAscii(HtmlEntity.DeEntitize(cells[5].InnerText).Trim());

                    if (string.IsNullOrWhiteSpace(symbol) || !symbol.Contains('/'))
                        continue;

                    decimal.TryParse(unitStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var unit);
                    if (unit <= 0) unit = 1;

                    decimal.TryParse(bidStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var bid);
                    decimal.TryParse(askStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var ask);
                    decimal.TryParse(avgStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var avg);

                    var currencyCode = symbol.Split('/')[0].Trim();

                    var rate = new CurrencyRate
                    {
                        CurrencyName = currencyName,
                        Symbol = symbol,
                        CurrencyCode = currencyCode,
                        QuoteCurrency = "KHR",
                        Unit = unit,
                        Bid = bid,
                        Ask = ask,
                        Average = avg
                    };

                    result.Rates.Add(rate);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Converts Khmer Unicode digits (០-៩) to Standard ASCII digits (0-9).
    /// </summary>
    public static string ConvertKhmerDigitsToAscii(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var sb = new StringBuilder(input.Length);
        foreach (var c in input)
        {
            if (c >= '\u17e0' && c <= '\u17e9')
            {
                sb.Append((char)('0' + (c - '\u17e0')));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}
