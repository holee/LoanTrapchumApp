using Applications.Responses.ViewModels.Auths;

namespace TrapchumApp.Pages.Identity
{
    public partial class Permissions
    {
        string? filter=string.Empty; 
        int pageNumber = 1;
        int pageSize = 10;
        int counts = 0;
        IEnumerable<PermissionsVm> permissions = [];
       protected override async Task OnInitializedAsync()
        {
            await GetPermissions();

            await base.OnInitializedAsync();
        }


        async Task GetPermissions()
        {
            var param = new QueryParam(pageNumber, pageSize,string.Empty, filter);

            var rsp = await _userService.GetPermissionsAsync(param);

            counts = rsp.Count;

            permissions = rsp.Items;
        }

        private async Task PageSizeChanged(int args)
        {
            pageSize = args;    
            pageNumber = 0;
            await GetPermissions();
        }
        private async Task LoadData(LoadDataArgs args)
        {
            pageNumber = args.Skip!.Value;

            if (args.Filter != null)
            {
                filter = args.Filter.ToString();
            }
            else
            {
                filter = string.Empty;
            }


            await GetPermissions();
        }
        private async Task OnTextChanged(string args)
        {
            filter = args;
            await GetPermissions();
        }
    }
}