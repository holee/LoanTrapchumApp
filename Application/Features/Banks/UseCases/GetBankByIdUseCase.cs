using Application.Features.Currencies.Dto;
using Application.Interfaces.Banks;
using Application.Responses;

namespace Application.Features.Banks.UseCases
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
