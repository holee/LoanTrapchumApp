using Applications.Enumerations;
using Applications.Requests.Loans.Disbursements;
using Domains.Models.Loans;

namespace Applications.Calculations
{
    public static class LoanBuilderExtension
    {
        public static IEnumerable<ScheduleLine> CreateSchedules(this LoanDtoCreate req)
        {
            var app = Calculators.CreateBuilder(req).Build();

            return app.CreateSchedules(req);

        }


    }
}
