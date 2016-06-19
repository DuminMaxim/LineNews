using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entity;

namespace WebUI.Controllers
{
    [Authorize (Roles = "admin")]
    public class AdminController : Controller
    {
        IRepository<Blog> blogRepository;
        IRepository<User> userRepository;

        public AdminController(IRepository<Blog> blogRepository, IRepository<User> userRepository)
        {
            this.blogRepository = blogRepository;
            this.userRepository = userRepository;
        }


        public ViewResult Editor(string returnUrl, int blogId = 0)
        {
            ViewBag.returnUrl = returnUrl;

            if (blogId == 0)
            {
                return View(new Blog());
            }
            else
            {
                Blog blog = blogRepository.GetAll().Find(b => b.BlogId == blogId);
                return View(blog);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editor(Blog blog, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                // Добавление изоражения в blog
                if (image != null)
                {
                    blog.ImageExtention = image.ContentType;
                    blog.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(blog.ImageData, 0, image.ContentLength);
                }

                // Добавление даты и времени создания новости
                blog.Date = DateTime.Now;

                blogRepository.Add(blog);

                // Получение ID соданной новости
                int blogId = blogRepository.GetAll().First(b => b.Title == blog.Title).BlogId;

                //Пенаправление на страницу с созданной новостью
                return RedirectToAction("NewsDetails", "Home", new { blogId });
            }
            else
            {
                return View();
            }
        }


        public ViewResult UserAdministration()
        {
            var users = userRepository
                .GetAll()
                .Where(u => u.RoleId != 1)
                .OrderByDescending(u => u.IsBlocked)
                .ToList();

            return View(users);
        }


        [HttpPost]
        public ActionResult UserAdministration(string login)
        {
            User user = userRepository.GetAll().Find(u => u.Login == login);
            if(user != null)
            {
               if(user.IsBlocked)
                {
                    user.IsBlocked = false;
                }
                else
                {
                    user.IsBlocked = true;
                }
                userRepository.Add(user);
            }
            return RedirectToAction("UserAdministration");
        }


        // Удаление новости
        public ActionResult DeleteNews(int blogId)
        {
            blogRepository.Delete(blogId);
            return RedirectToAction("Index", "Home");
        }
    }
}