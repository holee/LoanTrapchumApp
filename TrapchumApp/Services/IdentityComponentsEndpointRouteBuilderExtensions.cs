using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Services;
using Applications.Features.Reports.Queries.PrintLoanScheduleDetails;
using Applications.Features.Reports.Queries.PrintLoanScheduleDetailsByGroup;
using Applications.Features.Schedules;
using Applications.Features.Schedules.GetLoanScheduleBySchedule;
using Applications.Services.Loans;
using Applications.Utils;
using DocumentFormat.OpenXml.Bibliography;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using QuestPDF.Fluent;
using Razor.Templating.Core;
using System.Text.Json;

namespace TrapchumApp.Services;

internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
    // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
    public static async Task<IEndpointConventionBuilder> MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {

        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");


        accountGroup.MapPost("/Logout", async (
            HttpContext httpContext,
            [FromServices] SignInManager<ApplicationUser> signInManager,
            [FromServices] IAntiforgery antiforgery) =>
        {
            // Validate the antiforgery token on a best-effort basis. Blazor Server's
            // prerendering can occasionally embed a token generated for a slightly
            // different HttpContext.User snapshot than the one active when this POST
            // is handled, which throws AntiforgeryValidationException even for a
            // legitimate logout click. Logging out is a low-risk action (a forged
            // request can, at worst, log the user out), so we tolerate a mismatch
            // instead of surfacing an unhandled 400/500 error to the user.
            try
            {
                await antiforgery.ValidateRequestAsync(httpContext);
            }
            catch (AntiforgeryValidationException)
            {
                // Ignore: proceed with sign-out regardless of token mismatch.
            }

            await signInManager.SignOutAsync();

            return TypedResults.LocalRedirect($"~/");

        }).DisableAntiforgery();

        var manageGroup = accountGroup.MapGroup("/Manage").RequireAuthorization();

        var loggerFactory = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>();

        var downloadLogger = loggerFactory.CreateLogger("DownloadPersonalData");

        manageGroup.MapPost("/DownloadPersonalData", async (
            HttpContext context,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] AuthenticationStateProvider authenticationStateProvider) =>
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                return Results.NotFound($"Unable to load user with ID '{userManager.GetUserId(context.User)}'.");
            }

            var userId = await userManager.GetUserIdAsync(user);
            downloadLogger.LogInformation("User with ID '{UserId}' asked for their personal data.", userId);

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();

            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await userManager.GetLoginsAsync(user);

            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            personalData.Add("Authenticator Key", (await userManager.GetAuthenticatorKeyAsync(user))!);

            var fileBytes = JsonSerializer.SerializeToUtf8Bytes(personalData);

            context.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");

            return TypedResults.File(fileBytes, contentType: "application/json", fileDownloadName: "PersonalData.json");


        });



        var reportGroup = endpoints.MapGroup("/Reports");

        reportGroup.MapGet("/Schedules/{loanId}/{branchId:int}/{fileName}", async (IMediator mediator, string loanId, int branchId) =>
        {

            var command = new PrintLoanScheduleDetailsQuery(loanId, branchId);

            var rsp = await mediator.Send(command);

            if (rsp.IsError)
            {
                return Results.Problem();
            }

            var data = rsp.Value;

            var doc = new ScheduleDocument(rsp.Value);

            var stream = doc.GeneratePdf();

            return Results.File(stream, contentType: "application/pdf", null, enableRangeProcessing: false);
        });

        reportGroup.MapGet("/Schedules/Pdf/{loanId}/{branchId:int}/{fileName}", async (IMediator mediator, string loanId, int branchId) =>
        {

            var command = new PrintLoanScheduleDetailsQuery(loanId, branchId);

            var rsp = await mediator.Send(command);

            if (rsp.IsError)
            {
                return Results.Problem();
            }

            var data = rsp.Value;

            var doc = new ScheduleDocument(rsp.Value);

            var stream = doc.GeneratePdf();

            return Results.File(stream, contentType: "application/pdf", null, enableRangeProcessing: false);


        });

        reportGroup.MapGet("/schedules/pdf/{loanId}/{branchId:int}/{scheduleId:int}", async (IMediator mediator, string loanId, int branchId, int scheduleId) =>
        {

            var command = new GetLoanScheduleByScheduleQuery(branchId, loanId, scheduleId);

            var rsp = await mediator.Send(command);

            if (rsp.IsError)
            {
                return Results.NotFound();
            }

            int installmentNumber = 1;

            rsp.Value.Schedules.ForEach(x =>
            {
                x.InstallmentNo = installmentNumber++;
            });


            var doc = new ScheduleByScheduleDocument(rsp.Value);

            var stream = doc.GeneratePdf();

            return TypedResults.File(stream, contentType: "application/pdf", null, enableRangeProcessing: false);

        });




        //this endpoint is for print loan schedule details with group by schedule, it will print each schedule in one pdf file, and return the pdf file of the schedule that is requested by the client

        reportGroup.MapGet("/pdf/loans/schedules/{loanId}/{branchId:int}/{scheduleId:int}", async (IMediator mediator, IWebHostEnvironment env, string loanId, int branchId, int scheduleId) =>
        {
            var command = new PrintLoanScheduleDetailsByGroupQuery(loanId, branchId, scheduleId);

            var rsp = await mediator.Send(command);

            if (rsp.IsError)
            {
                return Results.Problem();
            }

            var data = rsp.Value;


            var fontPath = Path.Combine(env.WebRootPath, "lib", "dist", "fonts", "SiemreapSans.woff");

            var fontMuolPath = Path.Combine(env.WebRootPath, "lib", "dist", "fonts", "KhmerOSmuollight.ttf");

            var logoPath = Path.Combine(env.WebRootPath, "assets", "logo", "trapchum.jpg");

            data.FontModel.FontBaseSiemReap64 = File.Exists(fontPath) ? Convert.ToBase64String(File.ReadAllBytes(fontPath)) : string.Empty;

            data.FontModel.FontBaseKhmerMuol64 = File.Exists(fontMuolPath) ? Convert.ToBase64String(File.ReadAllBytes(fontMuolPath)) : string.Empty;

            data.FontModel.LogoBase64 = File.Exists(logoPath) ? Convert.ToBase64String(File.ReadAllBytes(logoPath)) : string.Empty;

            var html = await RazorTemplateEngine.RenderAsync("/Views/Reports/RptSchedules.cshtml", data);

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
            });


            using var page = await browser.NewPageAsync();

            await page.SetContentAsync(html);

            await page.EvaluateExpressionHandleAsync("document.fonts.ready");

            await page.BringToFrontAsync();

            var pdf = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "12px",
                    Bottom = "12px",
                    Left = "12px",
                    Right = "12px"
                },
                WaitForFonts = true,
                DisplayHeaderFooter = false,

            });

            return Results.File(pdf, "application/pdf", null, enableRangeProcessing: false);

        });


        //this endpoint is for print loan schedule details without group by schedule, it will print all schedule in one pdf file

        reportGroup.MapGet("/pdf/loans/schedules/{loanId}/{branchId:int}", async (IMediator mediator, IWebHostEnvironment env, string loanId, int branchId) =>
        {
            var command = new PrintLoanScheduleDetailsQuery(loanId, branchId);

            var rsp = await mediator.Send(command);

            if (rsp.IsError)
            {
                return Results.Problem();
            }

            var data = rsp.Value;


            var fontPath = Path.Combine(env.WebRootPath, "lib", "dist", "fonts", "SiemreapSans.woff");

            var fontMuolPath = Path.Combine(env.WebRootPath, "lib", "dist", "fonts", "KhmerOSmuollight.ttf");

            var logoPath = Path.Combine(env.WebRootPath, "assets", "logo", "trapchum.jpg");

            data.FontModel.FontBaseSiemReap64 = File.Exists(fontPath) ? Convert.ToBase64String(File.ReadAllBytes(fontPath)) : string.Empty;

            data.FontModel.FontBaseKhmerMuol64 = File.Exists(fontMuolPath) ? Convert.ToBase64String(File.ReadAllBytes(fontMuolPath)) : string.Empty;

            data.FontModel.LogoBase64 = File.Exists(logoPath) ? Convert.ToBase64String(File.ReadAllBytes(logoPath)) : string.Empty;

            var html = await RazorTemplateEngine.RenderAsync("/Views/Reports/RptSchedules.cshtml", data);

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
            });


            using var page = await browser.NewPageAsync();

            await page.SetContentAsync(html);

            await page.EvaluateExpressionHandleAsync("document.fonts.ready");

            await page.BringToFrontAsync();

            var pdf = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "12px",
                    Bottom = "12px",
                    Left = "12px",
                    Right = "12px"
                },
                WaitForFonts = true,
                DisplayHeaderFooter = false,

            });

            return Results.File(pdf, "application/pdf", null, enableRangeProcessing: false);

        });










        reportGroup.MapGet("/Schedules/{loanId}/{branchId:int}/Exel", async ([FromServices] IPrintPdfServiceImpl printer,
            [FromServices] IWebHostEnvironment env,
            [FromServices] IPrintExelServiceImpl exel,
            string loanId, int branchId) =>
        {

            var rsp = await exel.DownLoadScheduleAsExcel(loanId, branchId);

            if (!rsp.IsSuccess)
            {
                return Results.NoContent();
            }

            var ms = rsp.Data ?? Stream.Null;

            return Results.File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{loanId}.xlsx");

        });



        reportGroup.MapGet("/Schedules/{loanId}/{branchId:int}/Excel", async ([FromServices] IWebHostEnvironment env,
            [FromServices] IPrintExelServiceImpl exel,
            string loanId, int branchId) =>
        {

            var rsp = await exel.DownLoadScheduleAsExcel(loanId, branchId);

            if (!rsp.IsSuccess)
            {
                return Results.NoContent();
            }

            var ms = rsp.Data ?? Stream.Null;

            return Results.File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{loanId}.xlsx");

        });






        return accountGroup;
    }
}



