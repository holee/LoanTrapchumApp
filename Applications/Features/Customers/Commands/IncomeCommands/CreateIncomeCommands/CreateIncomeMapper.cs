using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands
{
    public static class CreateIncomeMapper
    {
        public static CustomerIncome ToDto(this IncomeDtoCreate source)
        {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new CustomerIncome
            {
                Amount = source.Amount,
                IncomeSource = source.IncomeSource,
                CustomerRefId = source.CustomerId,
                Description = source.Description
            };
        }

        public static List<CustomerIncome> ToDtos(this AddIncomesCommand sources)
        {

            if (sources is null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Request.Select(source => source.ToDto()).ToList();
        }


        public static CustomerIncome ToDto(this AddIncomeCommand sources)
        {

            if (sources is null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Request.ToDto();
        }



    }
}
