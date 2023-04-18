using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCapp.Models
{
    public class LoginViewModel
    {
        [DisplayName("E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; } = null!;

        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }

    public class RegisterViewModel
    {
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; } = null!;

        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DisplayName("Jelszó megerősítése")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordRepeat { get; set; } = null!;
    }
}
