using Applications.Requests.Identity;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Identity.Users
{
    public partial class UserChangePassword
    {
        [Parameter] public string UserId { get; set; } = string.Empty;
        ChangePasswordDto model = new();
        protected async Task SubmitOnValid()
        {
            try
            {
                model.UserId = UserId;
                var response = await _userService.ChangePassword(model);
                if (!response.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                    return;
                }
                
                NotificationService.Notify(NotificationSeverity.Success, "Password was change successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}