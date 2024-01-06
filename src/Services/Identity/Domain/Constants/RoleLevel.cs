namespace Identity.Domain.Constants;
/// <summary>
/// Class <c>Access Roles</c> it is a class contains available app roles
/// </summary>
public static class RoleLevel
{
    public const string User = "user";
    public const string Admin = "admin";
    public const string Moderator = "moderator";
    public const string Staff = "moderator,admin";
    public const string Everyone = "user,admin,moderator";
}
