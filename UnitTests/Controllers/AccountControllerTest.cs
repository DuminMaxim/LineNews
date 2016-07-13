using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entity;
using System.Collections.Generic;
using WebUI.Models;
using WebUI.Controllers;
using System.Web.Mvc;
using System.Linq;

namespace UnitTests.Controllers
{

    // Authorization Tests

    [TestClass]
    public class AccountControllerTest
    {
        List<User> users = new List<User> {
                    new User { Login = "User1", Email = "User1@mail.ru", Password = "123456", IsBlocked = true},
                    new User { Login = "User2", Email = "User2@mail.ru", Password = "654321", IsBlocked = false},
                    new User { Login = "User3", Email = "User3@mail.ru", Password = "qwerty", IsBlocked = true}
                };


        [TestMethod]
        public void AuthorizeBlockedUser()
        {
            // Подготовка
            var mock = new Mock<IRepository<User>>();
            mock.Setup(m => m.GetAll()).Returns(users.AsQueryable<User>);

            // Действие
            AccountController controller = new AccountController(mock.Object);
            var result = controller.Login(new LoginModel { Login = "User1", Password = "123456" }) as ViewResult;

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
        }


        [TestMethod]
        public void AuthorizeNonExistUser()
        {
            // Подготовка
            var mock = new Mock<IRepository<User>>();
            mock.Setup(m => m.GetAll()).Returns(users.AsQueryable<User>);

            // Действие
            AccountController controller = new AccountController(mock.Object);
            var result = controller.Login(new LoginModel { Login = "User5", Password = "123456" }) as ViewResult;

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
        }


        // Registration Tests

        [TestMethod]
        public void RegUserWithExistLogin()
        {
            // Подготовка
            var mock = new Mock<IRepository<User>>();
            mock.Setup(m => m.GetAll()).Returns(users.AsQueryable<User>);
            mock.Setup(m => m.Add(It.IsAny<User>())).Throws<Exception>();

            // Действие
            AccountController controller = new AccountController(mock.Object);
            controller.Register(new RegisterModel { Login = "User1", Email = "UniqueAdress@mail.ru" });

            // Утверждение
            mock.Verify();
        }


        [TestMethod]
        public void RegUserWithExistEmail()
        {
            // Подготовка
            var mock = new Mock<IRepository<User>>();
            mock.Setup(m => m.GetAll()).Returns(users.AsQueryable<User>);
            mock.Setup(m => m.Add(It.IsAny<User>())).Throws<Exception>();

            // Действие
            AccountController controller = new AccountController(mock.Object);
            controller.Register(new RegisterModel { Login = "UniqueUser", Email = "user1@mail.ru" });

            // Утверждение
            mock.Verify();
        }


        [TestMethod]
        public void RegUser()
        {
            // Подготовка
            var regModel = new RegisterModel
            {
                Login = "User4",
                Email = "user4@mail.ru",
                Password = "qwerty",
            };

            var mock = new Mock<IRepository<User>>();
            mock.Setup(m => m.GetAll()).Returns(users.AsQueryable<User>);

            // Действие
            AccountController controller = new AccountController(mock.Object);
            controller.Register(regModel);

            // Утверждение
            mock.Verify(m => m.Add(It.Is<User>(u => 
                u.Login == regModel.Login &&
                u.Password == regModel.Password &&
                u.Email == regModel.Email &&
                u.RoleId == 2
                ))
             );
        }
    }
}

