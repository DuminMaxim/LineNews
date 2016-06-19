using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entity;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        public int pageSize = 10;
        public IRepository<Blog> blogRepository;
        public IRepository<Comment> commentRepository;
        public IRepository<User> userRepository;


        public HomeController(IRepository<Blog> blogRepository, IRepository<Comment> commentRepository, IRepository<User> userRepository)
        {
            this.blogRepository = blogRepository;
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
        }

       
        public ViewResult Index(int currentPage = 1)
        {
            BlogListViewModel model = new BlogListViewModel
            {
                // Выбор 10 новостей
                Blogs = blogRepository.GetAll()
                  .OrderByDescending(b => b.Date)
                  .Skip(pageSize * (currentPage - 1))
                  .Take(pageSize)
                  .ToList(),

                pagingInfo = new PagingInfo()
                {
                    CurrentPage = currentPage,
                    BlogsPerPage = pageSize,
                    TotalBlogs = blogRepository.GetAll().Count()
                }
            };

            return View(model);
        }

        // Детальное описание новости
        public ViewResult NewsDetails(int blogId)
        {
            Blog blog = blogRepository.GetAll().ToList().Find(b => b.BlogId == blogId);
            if (blog != null)
            {
                return View(blog);
            }
            else
            {
                throw new ArgumentException();
            }
        }


        // Последние новости
        public PartialViewResult LastNews()
        {
            // Выбор 5 последних новостей
            List<Blog> model = blogRepository.GetAll()
                .OrderByDescending(b => b.Date)
                .Take(5)
                .ToList();

            return PartialView(model);
        }


        public FileContentResult GetImage(int blogId)
        {
            Blog blog = blogRepository.GetAll().FirstOrDefault(p => p.BlogId == blogId);
            if (blog != null)
            {
                return File(blog.ImageData, blog.ImageExtention);
            }
            else
            {
                return null;
            }
        }


        // Поиск новостей
        public ViewResult Search(string searchText)
        {
            // Поиск новостей по совпадению текста в заголовках 
            //(с приведением к общему регистру для больших совпадений)
            var result = blogRepository
                 .GetAll()
                 .Where(b => b.Title.ToLower().Contains(searchText.ToLower()))
                 .OrderByDescending(b => b.Date)
                 .ToList();

            BlogListViewModel model = new BlogListViewModel
            {
                Blogs = result,
                pagingInfo = new PagingInfo()
                {
                    CurrentPage = 1,
                    BlogsPerPage = pageSize,
                    TotalBlogs = result.Count()
                }
            };

            ViewBag.searchText = searchText;
            return View("Index", model);
        }


        // Добавлие нового комментария
        [ValidateAntiForgeryToken]
        public ActionResult AddNewCommment(string text, int blogId)
        {
            if (!string.IsNullOrEmpty(text))
            {
                // Поиск пользователя в БД
                User user = userRepository.GetAll().Find(u => u.Login == User.Identity.Name);

                // Поиск статьи в БД
                Blog blog = blogRepository.GetAll().Find(b => b.BlogId == blogId);
                if (blog != null)
                {
                    Comment newComment = new Comment
                    {
                        Text = text,
                        Date = DateTime.Now,
                        Blog = blog,
                        User = user
                    };
                    commentRepository.Add(newComment);
                }
            }
            return RedirectToAction("NewsDetails", new { blogId });
        }


        // Удаление комментария
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int commentId, int blogId)
        {
            commentRepository.Delete(commentId);

            return RedirectToAction("NewsDetails", new { blogId });
        }


        public ViewResult About()
        {
            return View();
        }
    }
}

