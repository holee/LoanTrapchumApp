using System.Security.Claims;
using Applications.Extensions.Identity.Enumerations;

namespace Applications.Features.Identity.Users
{
    public class ClaimStore
    {
        public static List<Claim> ClaimStores = new List<Claim>
        {
            new Claim(Permission.CreateUser.ToString(),"Create User"),
            new Claim(Permission.ViewUser.ToString(),"View User"),
            new Claim(Permission.EditUser.ToString(),"Edit User"),
            new Claim(Permission.DeleteUser.ToString(),"Delete User"),
            new Claim(Permission.CreateRole.ToString(),"Create Role"),
            new Claim(Permission.ViewRole.ToString(),"View Role"),
            new Claim(Permission.EditRole.ToString(),"Edit Role"),
            new Claim(Permission.DeleteRole.ToString(),"Delete Role"),
            new Claim(Permission.AssignPersmission.ToString(),"Edit Journal"),
            new Claim(Permission.CreateLedger.ToString(),"Create Ledger"),
            new Claim(Permission.ViewLedger.ToString(),"View Ledger"),
            new Claim(Permission.DeleteLedger.ToString(),"Edit Ledger"),
            new Claim(Permission.EditLedger.ToString(),"Edit Ledger"),
            new Claim(Permission.CreateJournal.ToString(),"Create Journal"),
            new Claim(Permission.ViewJournal.ToString(),"View Journal"),
            new Claim(Permission.EditJournal.ToString(),"Edit Journal"),
            new Claim(Permission.DeleteJournal.ToString(),"Delete Journal"),
            new Claim(Permission.OpenBranch.ToString(),"Opened Branch"),
            new Claim(Permission.CloseBranch.ToString(),"Closeed Branch"),
            new Claim(Permission.ValaultIn.ToString(),"Valault In"),
            new Claim(Permission.ValaultOut.ToString(),"Valault Out"),
            new Claim(Permission.AssignRole.ToString(),"Assign Role"),
        };



        public static List<Claim> SystemClaims = new List<Claim>
        {
            new Claim(SystemPermissions.Create.ToString(),"Create"),
            new Claim(SystemPermissions.View.ToString(),"View"),
            new Claim(SystemPermissions.Edit.ToString(),"Edit"),
            new Claim(SystemPermissions.Delete.ToString(),"Delete"),
        };




        public static List<Claim> GetCLaims()
        {
            var permissions = Enum.GetValues<Permission>()
                                .Select(x => x.ToString())
                                .ToList();
            List<Claim> list = new List<Claim>();
            foreach (var item in permissions)
            {
                var claim = new Claim(item, item);
                list.Add(claim);
            }
            return list;
        }
    }
}


//public static Dictionary<string,List<Claim>> GenerateClaims() 
//{

//    Dictionary<string,List<Claim>> roleClaims = [];

//    Enum.GetValues<Module>().ToList().ForEach(y =>
//    {
//        List<Claim> claims = new List<Claim>();
//        Enum.GetValues<Suffix>().ToList().ForEach(x =>
//        {
//            var claim = new Claim($"{x.ToString()}{y.ToString()}", $"{x.ToString()} {y.ToString()}");
//            claims.Add(claim);
//        });
//        roleClaims.Add(y.ToString(), claims);   
//    });
//    return roleClaims;
//}