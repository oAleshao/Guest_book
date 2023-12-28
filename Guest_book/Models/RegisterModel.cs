using Guest_book.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Guest_book.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "обязательное поле")]
		[RegularExpression(@"[^!@#$%^&*()\\\.\-=[\]{}/<>~+|0-9]{1,100}", ErrorMessage = "Некорректные данные")]
		public string? FirstName { get; set; }


        [Required(ErrorMessage = "обязательное поле")]
        [RegularExpression(@"[^!@#$%^&*()\\\.\-=[\]{}/<>~+|0-9]{1,100}", ErrorMessage = "Некорректные данные")]
		public string? LastName { get; set; }
        
        [Required(ErrorMessage = "обязательное поле")]
        [Remote(action: "CheckLogin", controller: "Account", ErrorMessage = "Логин уже используется")]
		[RegularExpression(@"[^!@#$%^&*()\\\.\-=[\]{}/<>~+|]{1,100}", ErrorMessage = "Некорректные данные")]
		public string? login { get; set; }
        
        [Required(ErrorMessage = "обязательное поле")]
		[RegularExpression(@"[$A-Z]+[A-Za-z0-9]{5,100}", ErrorMessage = "Некорректные данные")]
		[DataType(DataType.Password)] 
        public string? password { get; set; }

        [Required(ErrorMessage = "обязательное поле")]
        [Compare("password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string? passwordConfirm { get; set; }
    }
}
