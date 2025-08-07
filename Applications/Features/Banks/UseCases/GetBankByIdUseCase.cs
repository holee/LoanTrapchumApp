using Applications.Interfaces.Banks;
using Applications.Responses;
using Applications.Responses.Lists.Currency;

namespace Applications.Features.Banks.UseCases
{
    public interface IGetBankByIdUseCase
    {
    }
    public sealed class GetBankByIdUseCase : IGetBankByIdUseCase
    {
        private readonly IBankService _bankService;

        public GetBankByIdUseCase(IBankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<Result<BankResponse>> ExecuteAsync(int? bankId)
        {
            if (!bankId.HasValue || bankId == 0)
            {
                return Result.Failure<BankResponse>(ErrorResponse.Validation("Bank ID Is required."));
            }

            await _bankService.GetByIdAsync(0);

            return Result.Success(new BankResponse());
        }
    }
}
