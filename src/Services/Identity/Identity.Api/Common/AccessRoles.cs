namespace Identity.Api.Common
{
    /// <summary>
    /// Class <c>Access Roles</c> it is a class contains available app roles
    /// </summary>
    public static class AccessRoles
    {
        public const string User = "user";
        public const string Admin = "admin";
        public const string Moderator = "moderator";
        public const string Staff = "moderator,admin";
        public const string All = "user,admin,moderator";
    }
}
