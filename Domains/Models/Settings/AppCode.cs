namespace Domains.Models.Settings
{
    public enum AppCode 
    {
        UserEmptyCode = 000,
        UnAuthorization = 401, 
        UserUnActive = 403,
        Succeed = 200,
        InternalServerError = 500,
        DataNoFount = 404,
        BadRequest = 400
    }
}
