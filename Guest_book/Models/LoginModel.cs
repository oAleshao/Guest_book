using System.ComponentModel.DataAnnotations;

namespace Guest_book.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string? login {  get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        public string? password { get; set; }

    }
}
