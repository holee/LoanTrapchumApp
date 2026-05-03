using Applications.Features.Identity.Roles.Requests;
using Applications.Features.Identity.Roles.Validation;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.LoanPayments.Validations;
using Applications.Features.LoanRequests.Requests;

namespace Applications
{
    public static class AddValidatorDependencyInjection
    {

        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {

            services.AddScoped<IValidator<RoleDtoUpdate>, RoleDtoUpdateValidator>();
            services.AddScoped<IValidator<AproveLoanDtoCreate>, AprovedLoanDtoCreateValidator>();
            services.AddScoped<IValidator<CollateralDtoCreate>, CollateralCreateValidator>();
            services.AddScoped<IValidator<CollateralLineDtoCreate>, CollateralLineCreateValidator>();
            services.AddScoped<IValidator<VCollateralLines>, VCollateralLineCreateValidator>();
            services.AddScoped<IValidator<LoanPaymentDtoCreate>, LoanPaymentDtoValidator>();
            return services;

        }
    }
}