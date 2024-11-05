using System.ComponentModel.DataAnnotations;

namespace TestingApp.Areas.Authentication.Models
{
    public class AuthenticationData
    {
        [Required(ErrorMessage = "Требуется ввести логин")]
        public required string Login { get; set; }

        [Required(ErrorMessage = "Требуется ввести пароль")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Длина пароля должна быть не менее 8 символов")]
        public required string Password { get; set; }
    }
}
