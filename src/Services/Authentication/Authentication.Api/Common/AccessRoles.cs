namespace Authentication.Api.Common
{
    public class AccessRoles
    {
        public readonly static string User = "user";
        public readonly static string Admin = "admin";
        public readonly static string Moderator = "moderator";
        public readonly static string All = string.Join(",", new string[] { User, Admin, Moderator });
    }
}
