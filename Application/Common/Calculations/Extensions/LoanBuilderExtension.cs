using Application.Features.Loans.Dtos.Requests;
using Application.Features.Loans.Mappers;
using Domains.Enumerations;
using Domains.Models.Loans;

namespace Application.Calculations
{
    public static class LoanBuilderExtension
    {
        public static IEnumerable<ScheduleLine> CreateSchedules(this LoanDtoCreate req)
        {
            var app = Calculators.CalculatorBuilder(req).Build();

            return app.CreateSchedules(req);

        }

        public static IEnumerable<LoanScheduleDtoCreate> GenerateSchedules(this LoanDtoCreate req)
        {
            var app = Calculators.CalculatorBuilder(req).Build();

            var schedules = app.CreateSchedules(req);

            return schedules.MapToLoanSchedules();

        }


        public static IEnumerable<LoanScheduleDtoCreate> GenerateSchedules(this LoanDtoCreate req, LoanRepaymentModes mode)
        {
            var app = Calculators.CalculatorBuilder(req).Build();

            var schedules = app.CreateSchedules(req, mode);

            return schedules.MapToLoanSchedules();

        }




    }
}
