namespace Identity.Api.Common
{
    public static class AccessRoles
    {
        public const string User = "user";
        public const string Admin = "admin";
        public const string Moderator = "moderator";
        public const string Staff = "moderator,admin";
        public const string All = "user,admin,moderator";
    }
}
