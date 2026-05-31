using Applications.Features.Identity.Users;

namespace TrapchumApp.Pages.Identity.Users
{
    public partial class UserChangePassword
    {
        [Parameter] public string UserId { get; set; } = string.Empty;

        ChangePasswordDto model = new();

        protected override void OnInitialized()
        {
            model.UserId = UserId;

            base.OnInitialized();
        }
        protected async Task SubmitOnValid()
        {
            var response = await _userService.ChangePassword(model);

            if (response.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Error, response.FirstError.Description);

                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, "Password was change successfully.");

            DialogService.Close();

        }

    }
}