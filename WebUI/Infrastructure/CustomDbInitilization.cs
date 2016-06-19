using System;
using System.Data.Entity;
using Domain.Entity;
using Domain.EF;
using System.Threading;

namespace WebUI.Infrastructure
{
    public class CustomDbInitilization : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            db.Configuration.LazyLoadingEnabled = false;

            // Добавление ролей
            db.Roles.Add(new Role { RoleId = 1, Name = "admin" });
            db.Roles.Add(new Role { RoleId = 2, Name = "user" });

            // Добавление пользователя с ролью администратор
            db.Users.Add(new User
            {
                Login = "admin",
                Email = "admin@gmail.com",
                Password = "112233",
                RoleId = 1,
                IsBlocked = false
            });

            // Добавление 10 тестовых пользователей
            for (int i = 0; i < 10; i++)
            {
                db.Users.Add(new User
                {
                    Login = "User" + i,
                    Email = string.Format("adress{0}@gmail.com", i),
                    Password = "123456",
                    RoleId = 2,
                    IsBlocked = false
                });
            }

            // Добавление 15 тестовых новостей
            for (int i = 1; i <= 15; i++)
            {
                db.Blogs.Add(new Blog
                {
                    BlogId = i,
                    Title = "Статья " + i,
                    Description = "Et tellus suspendisse suscipit orci sit amet sem venenatis nec lobortis sem suscipit nullam nec imperdiet velit mauris eu nisi a felis imperdiet porta at ac nulla vivamus faucibus felis nec dolor pretium eget pellentesque dolor suscipit maecenas vitae enim arcu, at tincidunt nunc pellentesque eleifend vulputate lacus, vel semper sem ornare sit amet proin sem sapien, auctor vel faucibus id, aliquet vitae ipsum etiam auctor ultricies ante, at dapibus urna viverra sed nullam mi arcu, tempor vitae interdum a, sodales non urna vestibulum dignissim auctor mauris, ac elementum purus fermentum vitae duis placerat laoreet risus, sit amet eleifend tellus lobortis non vivamus iaculis dapibus leo a ornare cras vel sem at felis convallis mollis posuere ultrices dolor sed tellus arcu, accumsan a consectetur sit amet, volutpat eget lorem phasellus quis ipsum orci integer sodales tincidunt nibh a elementum ut ac libero nec orci euismod euismod nec at nulla duis malesuada faucibus porta aliquam nec consequat eros sed porttitor placerat dolor, accumsan imperdiet neque ornare in aenean non elit non leo porta mattis mauris non dolor nunc, id congue odio donec tellus nisl, semper id consectetur vitae, dapibus dictum nisl morbi sed augue purus sed dictum diam convallis tortor interdum volutpat phasellus dapibus arcu sit amet neque vulputate sed elementum orci fringilla in hac habitasse platea dictumst maecenas ut dui diam curabitur adipiscing vestibulum libero, nec varius dui pulvinar eget vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; aliquam dui neque, varius eu laoreet vel.",
                    Date = DateTime.Now
                });

                //  для Date
                Thread.Sleep(10);
            }
        }
    }
}