using Applications;
using Applications.Commons.Authorizations;
using Domains.Models.Identity;
using Infrastructures;
using Infrastructures.DependencyInjects;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuestPDF.Infrastructure;
using TrapchumApp;
using TrapchumApp.Authorizations.Handlers;
using TrapchumApp.Authorizations.Rquirements;
using TrapchumApp.Components;
using TrapchumApp.Components.Modals.SweetAlerts;
using TrapchumApp.Components.Toasts;
using TrapchumApp.Hubs;
using TrapchumApp.Services;
var builder = WebApplication.CreateBuilder(args);


QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPolicyServices();
builder.Services.AddApplicationServices();
builder.Services.AddOtherServices();
builder.Services.AddValidationServices();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddMapster();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthorizationHandler, SameUserRequestAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SameUserApprovedAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsCanApprovedNormalAmountHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsCanApprovedMaximumAmountHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanApprovedRequestHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanDisburseRequestHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanRequestHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanReceivePaymentHandler>();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AddScoped<NotificationServices>();
builder.Services.AddScoped<NotificationHubService>();
builder.Services.AddToastService(options =>
{
    options.Position = ToastPosition.TopRight;
    options.ShowCloseButton = true;
    options.ShowProgressBar = true;
    options.NewestOnTop = true;
    options.CloseOnHover = true;
    options.PreventDuplicates = false;
});
builder.Services.AddSignalR();
builder.Services.AddScoped<MessageBoxService>();
builder.Services.AddScoped<SwalService>();
builder.Services.AddScoped<DashboardServices>();
builder.Services.AddScoped<TrapchumApp.Components.Modals.Dialoques.ModalService>();
builder.Services.AddSingleton<IAppNotificationService, AppNotificationService>();
builder.Services.AddSingleton<IProfilePhotoService, ProfilePhotoService>();
builder.Services.AddSingleton<UserServices>();
builder.Services.AddSingleton<BranchService>();
builder.Services.AddSingleton<CurrencyServices>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

builder.Services.Configure<CircuitOptions>(options =>
{
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

await app.SeedSystemData();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{

    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapControllers();

await app.MapAdditionalIdentityEndpoints();

app.MapUploadFileEndpoints();

app.MapHub<AppNotificationHub>("/hubs/notifications");


app.Run();



//// Inject IAuthorizationService
//var result = await AuthorizationService.AuthorizeAsync(
//    user, loanRequest, new IsCanApprovedNormalAmountRequirement());
//// or for large amounts:
//var result = await AuthorizationService.AuthorizeAsync(
//    user, loanRequest, new IsCanApprovedMaximumAmountRequirement());

//var resource = (loanRequest, userLoanSize);
//await _authService.AuthorizeAsync(user, resource, new CanApprovedRequestRequirement());