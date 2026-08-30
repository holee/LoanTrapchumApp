using Applications;
using Applications.Calculations;
using Applications.Features.NbcBank.Services;
using Domains.Models.Identity;
using Infrastructures;
using Infrastructures.DependencyInjects;
using Infrastructures.Middlewares;
using Mapster;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TrapchumApp;
using TrapchumApp.Components;
using TrapchumApp.Components.Modals.SweetAlerts;
using TrapchumApp.Components.Toasts;
using TrapchumApp.Hubs;
using TrapchumApp.Services;

var builder = WebApplication.CreateBuilder(args);
QuestPdfSetup.QuestPdfConfig();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorTemplating();
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPoliciesServices();
builder.Services.AddApplicationServices();
builder.Services.AddOtherServices();
builder.Services.AddValidationServices();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddMapster();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddControllers();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AddScoped<NotificationServices>();
builder.Services.AddScoped<NotificationHubService>();
builder.Services.AddScoped<CalculatorSceduleService>();
builder.Services.AddHostedService<BrowserSetupService>();
builder.Services.AddScoped<PdfServices>();
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
builder.Services.AddScoped<AppSettingService>();
builder.Services.AddScoped<ManagerRedirectMiddleware>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<INbcExchangeRateService, NbcExchangeRateService>();
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

app.UseMiddleware<ManagerRedirectMiddleware>();

app.UseStaticFiles();

app.MapStaticAssets();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapControllers();

app.MapHealthChecks("/health/live").AllowAnonymous();

await app.MapAdditionalIdentityEndpoints();

app.MapUploadFileEndpoints();

app.MapNbcExchangeRateEndpoints();

app.MapHub<AppNotificationHub>("/hubs/notifications");

app.Run();


//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SameSite = SameSiteMode.Lax;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//});

// Only forwarded by a reverse proxy that has been explicitly configured as trusted.
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//});

//app.UseForwardedHeaders();

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
//    context.Response.Headers.Append("X-Frame-Options", "DENY");
//    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
//    context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
//    await next();
//});