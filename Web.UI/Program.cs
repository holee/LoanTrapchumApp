using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Refit;
using Web.UI;
using Web.UI.Services;
using Web.UI.Services.Authentications;
using Web.UI.Components.Messageboxes;
using Web.UI.Components.Modals.SweetAlerts;
using Web.UI.Components.Toasts;
using Applications;
using Applications.Utils;
using Applications.Features.Addresses.Interfaces;
using Applications.Services.Identity;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// The API base address
var apiBaseAddress = builder.Configuration["ApiBaseUrl"] is { Length: > 0 } configured
    ? configured
    : builder.HostEnvironment.BaseAddress;

// Authentication: JWT bearer token kept in browser storage.
builder.Services.AddScoped<TokenStore>();
builder.Services.AddTransient<BearerTokenHandler>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddScoped<AuthenticationClient>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped(sp =>
{
    var bearer = sp.GetRequiredService<BearerTokenHandler>();

    bearer.InnerHandler = new HttpClientHandler();

    return new HttpClient(bearer) { BaseAddress = new Uri(apiBaseAddress) };
});
builder.Services.AddRadzenComponents();

// Registers every named policy (AdminOnly, AccountOnly, CanExchanged, ReadLoan, ...) used by the layout menu.
builder.Services.AddPoliciesServices();

builder.Services.AddToastService();
builder.Services.AddScoped<MessageBoxService>();
builder.Services.AddScoped<SwalService>();
builder.Services.AddScoped<LoadingTrackerService>();
builder.Services.AddScoped<NotificationServices>();
builder.Services.AddScoped<BranchService>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<CurrencyServices>();
builder.Services.AddScoped<AppSettingService>();
builder.Services.AddScoped<ExcelImportService>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<IMediator, ClientMediator>();
builder.Services.AddScoped<IAddressServiceImpl, AddressServiceClient>();
builder.Services.AddScoped<IUserServiceImpl, UserServiceClient>();
builder.Services.AddScoped<Applications.Interfaces.Identity.IRoleService, RoleServiceClient>();
builder.Services.AddScoped<IProfilePhotoService, ProfilePhotoService>();
builder.Services.AddScoped<Applications.Interfaces.setting.IAppSettingService, AppSettingServiceClient>();
builder.Services.AddScoped<Applications.Interfaces.Transactions.IForiegnCurrencyExchange, ForiegnCurrencyExchangeClient>();

builder.Services
    .AddRefitClient<ICurrencyApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(apiBaseAddress))
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services
    .AddRefitClient<ISettingApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(apiBaseAddress))
    .AddHttpMessageHandler<BearerTokenHandler>();

await builder.Build().RunAsync();
