using Applications.Responses.Lists.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Branches.Dtos
{
    public sealed class UserBranchesResponse
    {
        public string? Id { get; set; }     
        public string? UserName { get; set; } = string.Empty;
        public int BranchId { get; set; } = 0;   
        public string? BranchName { get; set; } = string.Empty;

    }
}
