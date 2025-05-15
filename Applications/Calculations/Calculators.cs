using Applications.Requests.Loans.Disbursements;

namespace Applications.Calculations
{

    public sealed class LoanCreateConfiguration
    {
        public LoanDtoCreate? LoanCreate { get; set; }
    }


    public sealed class Calculators
    {

        static LoanDtoCreate? _parameter;

        private Calculators(LoanDtoCreate? parameter)
        {
            _parameter = parameter;
        }

        public static Calculators CreateBuilder(LoanDtoCreate parameter)
        {
            _parameter = parameter;

            return new Calculators(parameter);
        }

        public static Calculators CreateBuilder(Action<LoanCreateConfiguration> config)
        {
            var parameter = new LoanCreateConfiguration();

            config.Invoke(parameter);

            _parameter = parameter.LoanCreate;

            return new Calculators(_parameter);
        }

        public LoanBuilder Build()
        {
            if (_parameter is null) throw new ArgumentNullException(nameof(_parameter), "Parameter cannot be null");

            return Build(_parameter);

        }

        private LoanBuilder Build(LoanDtoCreate e)
        {
            return new(e);
        }
    }
}


//public LoanBuilder Build(LoanInfo e)
//{
//    return new(_currencyId, e.PayEvery, _repaymentModeId, e.CollectionFeeModeId, e.InstallmentTypeId, e.CollectionFeeRate, e.FirstPayment, e.StartDate);
//}
//int? _repaymentModeId;

//int? _currencyId;