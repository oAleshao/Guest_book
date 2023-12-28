using Guest_book.Models;
using Guest_book.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Guest_book.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository repository;

        public AccountController(IRepository repository)
        {
            this.repository = repository;
        }

		[AcceptVerbs("Get", "Post")]
		public async Task<IActionResult> CheckLogin(string userLogin)
		{
            var users = await repository.GetListUsers();
            foreach(var u in users)
			    if (u.login == userLogin)
				    return Json(false);
			return Json(true);
		}

		public async Task<ActionResult> Home()
		{
            var model = new Models.Message();
			var user = await repository.GetUserById(Convert.ToInt32(HttpContext.Session.GetString("userId")));
			if (user != null)
				model.user = user;

			return View("Home", model);
		}

		public ActionResult Login()
        {
			HttpContext.Session.SetString("userId", 0.ToString());
			return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("login","password")]LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = await repository.GetListUsers();

                    if (model.Count == 0)
                    {
                        ModelState.AddModelError("", "Такого пользователя в базе нету");
                        return View(loginUser);
                    }

                    var user = model.Where(u => u.login == loginUser.login).FirstOrDefault();

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Такого пользователя в базе нету");
                        return View(loginUser);
                    }

                    string savedPasswordHash = user.password;
                    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
                    byte[] salt = new byte[16];
                    Array.Copy(hashBytes, 0, salt, 0, 16);
                    var pbkdf2 = new Rfc2898DeriveBytes(loginUser.password, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    for (int i = 0; i < 20; i++)
                        if (hashBytes[i + 16] != hash[i])
                        {
							ModelState.AddModelError("", "Данные введены не верно");
							return View();
						}

					HttpContext.Session.SetString("userId", user.Id.ToString());
					var message = new Models.Message() { user = user };
					return View("Home", message);
				}
				catch(Exception ex)
                {
                    return View("Eror");
                }
			}
			return View();


		}

		public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName", "LastName", "login", "password", "passwordConfirm")]RegisterModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User newUser = new User() { FirstName = user.FirstName, LastName = user.LastName, login = user.login, password = user.password };
                    var tmpUser = await repository.GetUser(newUser);
                    if (tmpUser == null)
                    {
                        byte[] salt;
                        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                        var pbkdf2 = new Rfc2898DeriveBytes(user.password, salt, 100000);
                        byte[] hash = pbkdf2.GetBytes(20);
                        byte[] hashBytes = new byte[36];
                        Array.Copy(salt, 0, hashBytes, 0, 16);
                        Array.Copy(hash, 0, hashBytes, 16, 20);
                        string savedPasswordHash = Convert.ToBase64String(hashBytes);
                        newUser.password = savedPasswordHash;
                        newUser.salt = Encoding.UTF8.GetString(salt);
                        await repository.CreateUser(newUser);
                        await repository.Save();
                        return View("Login");
                    }
                }catch (Exception)
                {
                    return View("Eror");
                }
            }
            return View();
        }
    }
}
