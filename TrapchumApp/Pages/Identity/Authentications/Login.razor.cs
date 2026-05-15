using Applications.Services.Identity;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TrapchumApp.Services;

namespace TrapchumApp.Pages.Identity.Authentications
{

    public sealed class Input 
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = false;
    }


    public partial class Login
    {
        private string? errorMessage;
        private readonly ILogger<Login> _logger;
        private readonly IdentityRedirectManager _redirectManager;
        private readonly ILoginServiceImpl _loginService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public Login(ILoginServiceImpl loginService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<Login> logger, IdentityRedirectManager redirectManager)
        {
            _loginService = loginService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _redirectManager = redirectManager;
        }
        [CascadingParameter] HttpContext HttpContext { get; set; } = default!;


        [SupplyParameterFromForm]
        private Input Input { get; set; } = new();
        [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

        private bool _isBusy = false;
        protected override async Task OnInitializedAsync()
        {
            if (HttpContext is not null && HttpMethods.IsGet(HttpContext!.Request.Method))
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }
        }

        public async Task LoginUser()
        {
            _isBusy = true;

            var loginUser = await _userManager.FindByEmailAsync(Input.Email);

            loginUser ??= await _userManager.FindByNameAsync(Input.Email);

            if (loginUser is null)
            {
                errorMessage = $"Error:{Input.Email} is not register yet.";

                _isBusy = false;

                return;
            }

            if (loginUser.IsActive == false)
            {

                errorMessage = $"Error:User is already deactive....";

                _isBusy = false;

                return;
            }

            var checkedPassword = await _userManager.CheckPasswordAsync(loginUser, Input.Password);

            if (!checkedPassword)
            {
                errorMessage = $"Error:Invalid Password...!";

                _isBusy = false;

                return;
            }


            var loginResult = await _signInManager.PasswordSignInAsync(loginUser, Input.Password, Input.RememberMe, false);

            if (loginResult.Succeeded)
            {
                _logger.LogInformation("{UserName} has logged in.", Input.Email);

                _isBusy = false;

                HttpContext.Response.Cookies.Append("showAppLoading", "1", new CookieOptions
                {
                    HttpOnly = false,
                    SameSite = SameSiteMode.Strict,
                    MaxAge = TimeSpan.FromSeconds(30)
                });

                NavigationManager.NavigateTo(ReturnUrl ?? "/", true);
            }

        }


    }
}