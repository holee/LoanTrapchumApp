using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Validations;
using Applications.Features.Identity.Roles.Requests;
using Applications.Features.Identity.Roles.Validation;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.LoanPayments.Validations;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.LoanRequests.Validations;
using Applications.Responses.Lists.Collaterals;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

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