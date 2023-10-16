namespace Authentication.Api.Common
{
    /// <summary>
    /// Class <c>Access Roles</c> it is a class contains available app roles
    /// </summary>
    public class AccessRoles
    {
        public readonly static string User = "user";
        public readonly static string Admin = "admin";
        public readonly static string Moderator = "moderator";
        public readonly static string All = string.Join(",", new string[] { User, Admin, Moderator });
    }
}
