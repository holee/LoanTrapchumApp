using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Exceptions;

public sealed class CurrencyNotFoundException : Exception 
{
    public CurrencyNotFoundException()
        : base("The specified currency was not found.")
    {
    }

    public CurrencyNotFoundException(string currencyCode)
        : base($"Currency '{currencyCode}' was not found.")
    {
    }

    public CurrencyNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
