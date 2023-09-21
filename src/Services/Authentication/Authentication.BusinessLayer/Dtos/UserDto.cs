using Authentication.BusinessLayer.Validations;

namespace Authentication.BusinessLayer.Dtos
{
    [UserValidation]
    public class UserDto
    {
        public string? Nickname { get; set; }
        public string? Email { get; set; }  
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }
}
