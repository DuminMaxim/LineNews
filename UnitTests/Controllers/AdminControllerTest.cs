using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using System.Collections.Generic;
using Domain.Entity;
using WebUI.Controllers;

namespace UnitTests.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public void UserAdministrationTest()
        {
            // Подготовка
            var mock = new Mock<IRepository<User>>();
            mock.Setup(m => m.GetAll()).Returns(
                new List<User>
                {
                    new User { Login = "admin", Email = "admin@mail.ru", RoleId = 1, IsBlocked = false },
                    new User { Login = "User1", Email = "user1@mail.ru", RoleId = 2, IsBlocked = false },
                    new User { Login = "User2", Email = "user2@mail.ru", RoleId = 2, IsBlocked = true },
                }
            );

            // Действие
            AdminController controller = new AdminController(null, mock.Object);
            List<User> result = controller.UserAdministration().Model as List<User>;

            // Утверждение
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].Login, "User2");
            Assert.AreEqual(result[1].Login, "User1");
        }


        [TestMethod]
        public void UserBlockUnBlock()
        {
            // Подготовка
            var mock = new Mock<IRepository<User>>();
            mock.Setup(m => m.GetAll()).Returns(
                new List<User>
                {
                    new User { Login = "admin", RoleId = 1, IsBlocked = false },
                    new User { Login = "User1", RoleId = 2, IsBlocked = false },
                    new User { Login = "User2", RoleId = 2, IsBlocked = true },
                }
            );

            // Действие
            AdminController controller = new AdminController(null, mock.Object);
            controller.UserAdministration("User2");
            controller.UserAdministration("User1");

            // Утверждение
            Assert.AreEqual(mock.Object.GetAll()[1].IsBlocked, true);
            Assert.AreEqual(mock.Object.GetAll()[2].IsBlocked, false);
        }


        [TestMethod]
        public void EditNews()
        {
            // Подготовка
            var mock = new Mock<IRepository<Blog>>();
            mock.Setup(m => m.GetAll()).Returns(
                new List<Blog>
                {
                   new Blog { BlogId = 1},
                   new Blog { BlogId = 2},
                   new Blog { BlogId = 3}
                });

            // Действие
            AdminController controller = new AdminController(mock.Object, null);
            Blog result = controller.Editor("ReturnUrl", 2).Model as Blog;

            // Утверждение
            Assert.AreEqual(result.BlogId, 2);
        }


        [TestMethod]
        public void CreateNews()
        {
            // Действие
            AdminController controller = new AdminController(null, null);
            Blog result = controller.Editor("ReturnUrl").Model as Blog;

            // Утверждение
            Assert.AreEqual(result.BlogId, 0);
        }


        [TestMethod]
        public void DeleteNews()
        {
            // Подготовка
            var mock = new Mock<IRepository<Blog>>();

            // Действие
            AdminController controller = new AdminController(mock.Object, null);
            controller.DeleteNews(2);

            // Утверждение
            mock.Verify(m => m.Delete(It.Is<int>(v => v == 2)));
        }

    }
}
