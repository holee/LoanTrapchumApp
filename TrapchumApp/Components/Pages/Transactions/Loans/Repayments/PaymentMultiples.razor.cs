using Applications.Responses.ViewModels.Loans.Schedules;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments;
public partial class PaymentMultiples
{
    [Parameter] public IEnumerable<VSchedule> Schedules { get; set; } = [];
}
