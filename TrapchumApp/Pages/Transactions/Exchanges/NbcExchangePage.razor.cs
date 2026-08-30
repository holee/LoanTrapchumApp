using Applications.Features.NbcBank.Dto;
using System.Globalization;

namespace TrapchumApp.Pages.Transactions.Exchanges;

public partial class NbcExchangePage
{
    private static readonly string[] PopularCodes = ["USD", "THB", "EUR", "JPY", "CNY", "SGD", "VND"];

    private static readonly Dictionary<string, string> FlagMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["USD"] = "🇺🇸", ["KHR"] = "🇰🇭", ["THB"] = "🇹🇭", ["EUR"] = "🇪🇺", ["JPY"] = "🇯🇵",
        ["GBP"] = "🇬🇧", ["CNY"] = "🇨🇳", ["CNH"] = "🇨🇳", ["SGD"] = "🇸🇬", ["AUD"] = "🇦🇺",
        ["CAD"] = "🇨🇦", ["CHF"] = "🇨🇭", ["HKD"] = "🇭🇰", ["IDR"] = "🇮🇩", ["INR"] = "🇮🇳",
        ["KRW"] = "🇰🇷", ["LAK"] = "🇱🇦", ["MMK"] = "🇲🇲", ["MYR"] = "🇲🇾", ["NZD"] = "🇳🇿",
        ["PHP"] = "🇵🇭", ["SEK"] = "🇸🇪", ["TWD"] = "🇹🇼", ["VND"] = "🇻🇳", ["AED"] = "🇦🇪",
        ["ZAR"] = "🇿🇦", ["NGN"] = "🇳🇬", ["DKK"] = "🇩🇰", ["SAR"] = "🇸🇦", ["SDR"] = "🌐"
    };

    private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        ["en"] = new()
        {
            ["title"] = "NBC Exchange Rates",
            ["subtitle"] = "Official Rates from National Bank of Cambodia",
            ["officialBadge"] = "Official Rate",
            ["officialLabel"] = "USD / KHR Rate",
            ["converterTitle"] = "Currency Converter",
            ["amount"] = "Amount",
            ["from"] = "From Currency",
            ["to"] = "To Currency",
            ["swap"] = "Swap currencies",
            ["searchPlaceholder"] = "Search currency name or code...",
            ["colCurrency"] = "Currency",
            ["colSymbol"] = "Symbol",
            ["colUnit"] = "Unit",
            ["colBid"] = "Bid (KHR)",
            ["colAsk"] = "Ask (KHR)",
            ["colAverage"] = "Average (KHR)",
            ["colRate1"] = "Rate for 1 Unit",
            ["refresh"] = "Refresh",
            ["ratesTitle"] = "Market Cross Rates",
            ["asOf"] = "Date:",
            ["popular"] = "Popular Pairs:",
            ["loading"] = "Loading exchange rates from National Bank of Cambodia...",
            ["empty"] = "No currency rates found.",
            ["dataSource"] = "Data source:",
            ["sourceName"] = "National Bank of Cambodia (NBC)",
            ["error"] = "Could not fetch exchange rates from the National Bank of Cambodia. Please try again."
        },
        ["km"] = new()
        {
            ["title"] = "អត្រាប្តូរប្រាក់ ធនាគារជាតិនៃកម្ពុជា",
            ["subtitle"] = "ទិន្នន័យផ្លូវការពីធនាគារជាតិនៃកម្ពុជា (NBC)",
            ["officialBadge"] = "អត្រាផ្លូវការ",
            ["officialLabel"] = "អត្រា USD / KHR",
            ["converterTitle"] = "ឧបករណ៍គណនាប្តូរប្រាក់",
            ["amount"] = "ចំនួនទឹកប្រាក់",
            ["from"] = "រូបិយប័ណ្ណដើម",
            ["to"] = "រូបិយប័ណ្ណគោលដៅ",
            ["swap"] = "ដូររូបិយប័ណ្ណ",
            ["searchPlaceholder"] = "ស្វែងរកឈ្មោះរូបិយប័ណ្ណ ឬកូដ...",
            ["colCurrency"] = "រូបិយប័ណ្ណ",
            ["colSymbol"] = "និមិត្តសញ្ញា",
            ["colUnit"] = "ឯកតា",
            ["colBid"] = "ទិញ (រៀល)",
            ["colAsk"] = "លក់ (រៀល)",
            ["colAverage"] = "មធ្យម (រៀល)",
            ["colRate1"] = "អត្រាសម្រាប់ ១ ឯកតា",
            ["refresh"] = "ផ្ទុកឡើងវិញ",
            ["ratesTitle"] = "អត្រាប្តូរប្រាក់រូបិយប័ណ្ណផ្សេងៗ",
            ["asOf"] = "កាលបរិច្ឆេទ៖",
            ["popular"] = "រូបិយប័ណ្ណពេញនិយម៖",
            ["loading"] = "កំពុងទាញយកអត្រាប្តូរប្រាក់ពីធនាគារជាតិនៃកម្ពុជា...",
            ["empty"] = "រកមិនឃើញអត្រាប្តូរប្រាក់ទេ។",
            ["dataSource"] = "ប្រភពទិន្នន័យ៖",
            ["sourceName"] = "ធនាគារជាតិនៃកម្ពុជា (NBC)",
            ["error"] = "មិនអាចទាញយកអត្រាប្តូរប្រាក់ពីធនាគារជាតិនៃកម្ពុជាបានទេ។ សូមព្យាយាមម្តងទៀត។"
        }
    };

    private NbcExchangeRateResponse? _data;
    private string _lang = "en";
    private string _search = string.Empty;
    private string _fromCurrency = "USD";
    private string _toCurrency = "KHR";
    private decimal _amount = 100m;
    private string AmountText => _amount.ToString(CultureInfo.InvariantCulture);
    private bool _isLoading;
    private string? _errorMessage;

    protected override Task OnInitializedAsync() => LoadAsync();

    private string T(string key) =>
        Translations[_lang].TryGetValue(key, out var value) ? value : key;

    private static string Flag(string code) =>
        FlagMap.TryGetValue(code ?? string.Empty, out var flag) ? flag : "💰";

    private async Task LoadAsync(bool forceRefresh = false)
    {
        _isLoading = true;
        _errorMessage = null;

        try
        {
            _data = await _nbcService.GetExchangeRatesAsync(_lang, forceRefresh);
        }
        catch (Exception)
        {
            _errorMessage = T("error");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private Task RefreshAsync() => LoadAsync(true);

    private async Task SetLanguageAsync(string lang)
    {
        if (_lang == lang) return;

        _lang = lang;
        await LoadAsync();
    }

    private void OnSearchChanged(ChangeEventArgs args) =>
        _search = args.Value?.ToString() ?? string.Empty;

    private void OnAmountChanged(ChangeEventArgs args) =>
        _amount = decimal.TryParse(args.Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value) && value > 0
            ? value
            : 0m;

    private void Swap() => (_fromCurrency, _toCurrency) = (_toCurrency, _fromCurrency);

    private void UsePopular(string code)
    {
        _fromCurrency = code;
        _toCurrency = "KHR";
    }

    private List<CurrencyRate> FilteredRates()
    {
        var rates = _data?.Rates ?? [];
        if (string.IsNullOrWhiteSpace(_search)) return rates;

        var term = _search.Trim();
        return rates.Where(r =>
            r.CurrencyName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            r.CurrencyCode.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            r.Symbol.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private List<(string Code, string Name)> CurrencyOptions()
    {
        var options = new List<(string Code, string Name)>
        {
            ("USD", _lang == "km" ? "ដុល្លារអាមេរិក" : "US Dollar"),
            ("KHR", _lang == "km" ? "រៀលកម្ពុជា" : "Cambodian Riel")
        };

        if (_data is not null)
        {
            options.AddRange(_data.Rates
                .Where(r => !string.IsNullOrWhiteSpace(r.CurrencyCode) && r.CurrencyCode is not ("USD" or "KHR"))
                .Select(r => (r.CurrencyCode, r.CurrencyName)));
        }

        return options;
    }

    private List<(string Code, decimal Rate)> PopularChips()
    {
        var chips = new List<(string Code, decimal Rate)>();
        if (_data is null) return chips;

        foreach (var code in PopularCodes)
        {
            if (code == "USD")
            {
                if (_data.OfficialRate.Rate > 0) chips.Add((code, _data.OfficialRate.Rate));
                continue;
            }

            var rate = _data.Rates.FirstOrDefault(r => string.Equals(r.CurrencyCode, code, StringComparison.OrdinalIgnoreCase));
            if (rate is not null) chips.Add((code, rate.RatePerOneUnit));
        }

        return chips;
    }

    private decimal RateInKhr(string code)
    {
        if (string.Equals(code, "KHR", StringComparison.OrdinalIgnoreCase)) return 1m;
        if (string.Equals(code, "USD", StringComparison.OrdinalIgnoreCase)) return _data?.OfficialRate.Rate ?? 0m;

        var rate = _data?.Rates.FirstOrDefault(r => string.Equals(r.CurrencyCode, code, StringComparison.OrdinalIgnoreCase));
        return rate?.RatePerOneUnit ?? 0m;
    }

    private decimal EffectiveRate
    {
        get
        {
            var toRate = RateInKhr(_toCurrency);
            return toRate == 0m ? 0m : RateInKhr(_fromCurrency) / toRate;
        }
    }

    private decimal ConvertedAmount => _amount * EffectiveRate;

    private static string FormatNumber(decimal value, int maxDecimals = 2)
    {
        var rounded = Math.Round(value, maxDecimals, MidpointRounding.AwayFromZero);
        var format = new NumberFormatInfo { NumberGroupSeparator = ",", NumberDecimalSeparator = "." };
        return rounded.ToString("#,##0." + new string('#', maxDecimals), format);
    }
}
