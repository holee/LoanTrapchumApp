using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.NbcBank.Dto;

public sealed class OfficialExchangeRate
{
    public string BaseCurrency { get; set; } = "USD";

    public string TargetCurrency { get; set; } = "KHR";

    public decimal Rate { get; set; }

    public string Description { get; set; } = string.Empty;
}
