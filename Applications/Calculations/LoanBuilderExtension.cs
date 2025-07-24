using Applications.Enumerations;
using Applications.Features.Loans.Mappers;
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

        public static IEnumerable<LoanScheduleDtoCreate> GenerateSchedules(this LoanDtoCreate req)
        {
            var app = Calculators.CreateBuilder(req).Build();

            var schedules = app.CreateSchedules(req);

            return schedules.MapToLoanSchedules();

        }

    }
}
