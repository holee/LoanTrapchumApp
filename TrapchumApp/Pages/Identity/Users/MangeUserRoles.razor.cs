using Applications.Responses.ViewModels.Auths;

namespace TrapchumApp.Pages.Identity.Users
{
    public partial class MangeUserRoles
    {
        [Parameter] public long Id { get; set; }

        protected List<UserRolesViewModel>? userRoles = new List<UserRolesViewModel>();

        protected EditForm editForm = default!;
        protected bool IsLoading = true;
        protected void SubmitOnValid()
        {
            IsLoading = true;
            try
            {
                editForm?.EditContext?.Validate();
                //var token = await localStorageService.GetItemAsStringAsync("bearerToken");
                //var response=await userService.AssignRolesByUserAsync(Id, userRoles!, token);
                //  if (response!.IsSuccess)
                //  {
                //      Toast.Success("Permission was changed.");
                //      IsLoading = false;
                //      navigationManager.NavigateTo($"/Admins/Account/Manage/{Id}/User");
                //  }


            }
            catch (Exception ex)
            {
                IsLoading = true;
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                IsLoading = false;
            }


        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //var token = await localStorageService.GetItemAsStringAsync("bearerToken");
                //userRoles = (await userService!.GetRolesByUser(Id, token))!.ToList();
                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = true;
                Console.WriteLine(ex.Message);
            }

            finally
            {
                IsLoading = false;
            }
            await base.OnInitializedAsync();
        }




    }
}
