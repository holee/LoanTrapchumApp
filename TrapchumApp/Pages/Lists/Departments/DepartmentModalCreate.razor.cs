using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Departments.Commands.CreateDepartment;
using Applications.Features.Departments.Dto;
using Domains.Models.Lists;
using Mapster;

namespace TrapchumApp.Pages.Lists.Departments
{
    public partial class DepartmentModalCreate
    {
        private DepartmentDtoCreate Req = new DepartmentDtoCreate();
        private IEnumerable<VBranch> vBranches = [];
        protected async Task Create()
        {

            var model = Req.Adapt<Department>();

            var command = new CreateDepartmentCommand(Req);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "Department record was created successful!");

                DialogService.Close(rsp.Value);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }

        }

        protected override async Task OnInitializedAsync()
        {
            var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0, nameof(VBranch.BranchId)));

            var rsp = await mediator.Send(command);

            vBranches = rsp.Items;

            await base.OnInitializedAsync();
        }


        private void Closed()
        {
            DialogService.Close(null);
        }


    }
}