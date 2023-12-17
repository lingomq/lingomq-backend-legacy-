namespace Identity.Domain.Constants;
public class AccessRoles
{
    public readonly static string User = "user";
    public readonly static string Admin = "admin";
    public readonly static string Moderator = "moderator";
    public readonly static string Everyone = string.Join(",", new string[] { User, Admin, Moderator });
    public readonly static string Staff = string.Join(",", new string[] { Admin, Moderator });
}
