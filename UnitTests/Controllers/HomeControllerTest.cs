using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entity;
using System.Collections.Generic;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;
using System.Linq;

namespace UnitTests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        List<Blog> blogs = new List<Blog>
        {
            new Blog {BlogId = 1, Title = "Статья 1", Date = DateTime.Parse("17.06.2016 18:12:11")},
            new Blog {BlogId = 2, Title = "Статья 2", Date = DateTime.Parse("17.06.2016 19:03:11")},
            new Blog {BlogId = 3, Title = "Статья 3", Date = DateTime.Parse("17.06.2016 20:12:11")},
            new Blog {BlogId = 4, Title = "Статья 4", Date = DateTime.Parse("18.06.2016 14:15:11")},
            new Blog {BlogId = 5, Title = "Статья 5", Date = DateTime.Parse("18.06.2016 17:12:11")},
            new Blog {BlogId = 6, Title = "Статья 6", Date = DateTime.Parse("18.06.2016 18:12:11"),
                ImageData = new byte[] { }, ImageExtention="image/jpeg" }
        };

        [TestMethod]
        public void Paginate()
        {
            // Подготовка
            var mock = new Mock<IRepository<Blog>>();
            mock.Setup(m => m.GetAll()).Returns(blogs.AsQueryable<Blog>);

            // Действия 
            HomeController controller = new HomeController(mock.Object, null, null);
            controller.pageSize = 5;
            var result = controller.Index(2).Model as BlogListViewModel;

            // Утверждения
            Assert.AreEqual(result.Blogs.Count, 1);
            Assert.AreEqual(result.Blogs[0].Title, "Статья 1");

            Assert.AreEqual(result.pagingInfo.BlogsPerPage, 5);
            Assert.AreEqual(result.pagingInfo.TotalBlogs, 6);
            Assert.AreEqual(result.pagingInfo.CurrentPage, 2);
            Assert.AreEqual(result.pagingInfo.TotalPage, 2);
        }


        [TestMethod]
        public void TakeLastNews()
        {
            // Подготовка
            var mock = new Mock<IRepository<Blog>>();
            mock.Setup(m => m.GetAll()).Returns(blogs.AsQueryable<Blog>);

            // Действия
            HomeController controller = new HomeController(mock.Object, null, null);
            var result = controller.LastNews().Model as List<Blog>;

            // Утверждения
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual(result[0].Title, "Статья 6");
            Assert.AreEqual(result[4].Title, "Статья 2");
        }


        [TestMethod]
        public void GoToNewsDetails()
        {
            // Подготовка
            var mock = new Mock<IRepository<Blog>>();
            mock.Setup(m => m.GetAll()).Returns(blogs.AsQueryable<Blog>);

            // Действия
            HomeController controller = new HomeController(mock.Object, null, null);
            Blog result = controller.NewsDetails(4).Model as Blog;

            // Утверждение
            Assert.AreEqual(result.BlogId, 4);
        }


        [TestMethod]
        public void SearchNews()
        {
            // Подготовка
            var mock = new Mock<IRepository<Blog>>();
            mock.Setup(m => m.GetAll()).Returns(blogs.AsQueryable<Blog>);

            // Действия
            HomeController controller = new HomeController(mock.Object, null, null);
            ViewResult viewResult = controller.Search("Статья 4");
            var resultModel = viewResult.Model as BlogListViewModel;

            // Утверждение
            Assert.AreEqual(resultModel.Blogs.Count, 1);
            Assert.AreEqual(resultModel.Blogs[0].BlogId, 4);
            Assert.AreEqual(resultModel.pagingInfo.TotalPage, 1);

            Assert.AreEqual(viewResult.ViewBag.searchText, "Статья 4");
        }

        [TestMethod]
        public void GetImage()
        {
            // Подготовка 
            var mock = new Mock<IRepository<Blog>>();
            mock.Setup(m => m.GetAll()).Returns(blogs.AsQueryable<Blog>);

            // Действие
            HomeController controller = new HomeController(mock.Object, null, null);
            FileResult result = controller.GetImage(6);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.AreEqual("image/jpeg", result.ContentType);
        }



        [TestMethod]
        public void AddEmptyComment()
        {
            // Подготовка
            var mock = new Mock<IRepository<Blog>>();
            mock.Setup(m => m.GetAll()).Returns(blogs.AsQueryable<Blog>);

            var mockComment = new Mock<IRepository<Comment>>();
            mockComment.Setup(m => m.Add(It.IsAny<Comment>())).Throws<Exception>();

            // Действие
            HomeController controller = new HomeController(mock.Object, mockComment.Object, new Mock<IRepository<User>>().Object);
            controller.AddNewCommment("", 1);

            // Утверждение
            mockComment.Verify();
        }


        [TestMethod]
        public void DeleteComment()
        {
            // Подготовка
            var mock = new Mock<IRepository<Comment>>();
            var commentId = 4;
            var blogId = 1;

            // Действие
            HomeController controller = new HomeController(null, mock.Object, null);
            var routeResult = controller.DeleteComment(commentId, blogId) as RedirectToRouteResult;

            // Утверждение
            mock.Verify(m => m.Delete(It.Is<int>(v => v == 4)));
            Assert.AreEqual("CommentsList", routeResult.RouteValues["action"]);
            Assert.AreEqual(blogId, routeResult.RouteValues["blogId"]);
        }
    }
}
