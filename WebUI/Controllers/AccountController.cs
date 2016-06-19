using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebUI.Models;
using Domain.Abstract;
using Domain.Entity;

namespace WebUI.Controllers
{

    public class AccountController : Controller
    {
        IRepository<User> userRepository;

        public AccountController(IRepository<User> repository)
        {
            userRepository = repository;
        }


        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Аутентификация пользоватея
                User user = userRepository.GetAll().FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    // Проверка блокировки пользователя
                    if (user.IsBlocked)
                    {
                        ModelState.AddModelError("", user.Login + " заблокирован");
                    }
                    else
                    {
                        // Авторизация пользователя
                        FormsAuthentication.SetAuthCookie(model.Login, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин/пароль");
                }
            }

            return View(model);
        }


        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
                #region Дополнительная валидация

                User user = null;

                // Проверка наличия в БД пользователя с таким же логином
                user = userRepository.GetAll().FirstOrDefault(u => u.Login.ToLower() == model.Login.ToLower());
                if (user != null)
                {
                    ModelState.AddModelError("", "пользователь с таким Логином существует");
                }

                // Проверка наличия в БД пользователя с таким же Email
                user = userRepository.GetAll().FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
                if (user != null)
                {
                    ModelState.AddModelError("", "пользователь с таким E-mail существует");
                }

                if (user != null)
                {
                    return View(model);
                }
            #endregion


            if (ModelState.IsValid)
            {
                // Добавление пользователя в БД
                userRepository.Add(
                    new User
                    {
                        Login = model.Login,
                        Password = model.Password,
                        Email = model.Email,
                        RoleId = 2,
                        IsBlocked = false
                    });

                // Авторизация зарегистрированного пользователя
                user = userRepository.GetAll().Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }


        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}