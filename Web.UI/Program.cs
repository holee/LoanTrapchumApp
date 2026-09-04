using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Refit;
using Web.UI;
using Web.UI.Services;
using Web.UI.Components.Messageboxes;
using Web.UI.Components.Modals.SweetAlerts;
using Web.UI.Components.Toasts;
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

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });
builder.Services.AddRadzenComponents();
builder.Services.AddAuthorizationCore();

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

builder.Services
    .AddRefitClient<ICurrencyApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(apiBaseAddress));

builder.Services
    .AddRefitClient<ISettingApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(apiBaseAddress));

await builder.Build().RunAsync();
