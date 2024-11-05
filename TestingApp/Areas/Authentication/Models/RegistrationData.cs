using System.ComponentModel.DataAnnotations;

namespace TestingApp.Areas.Authentication.Models
{
    public class RegistrationData : AuthenticationData
    {
        [Required(ErrorMessage = "Требуется ввести имя")]
        [MinLength(2, ErrorMessage = "Имя должно быть не менее 2 символов")]
        [MaxLength(100, ErrorMessage = "Имя должно быть не более 100 символов")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Требуется ввести подтверждение пароля")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public required string PasswordConfirm { get; set; }
    }
}
