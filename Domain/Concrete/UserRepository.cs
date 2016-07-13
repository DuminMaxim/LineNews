using Domain.Abstract;
using Domain.Entity;
using Domain.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class UserRepository : IRepository<User>
    {
        public void Add(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User dbEntity = db.Users.FirstOrDefault(u => u.Login == user.Login);

                // Если пользователь существует в БД, изменяем существующего
                // иначе добавляем нового 
                if (dbEntity != null)
                {
                    dbEntity.Login = user.Login;
                    dbEntity.Email = user.Email;
                    dbEntity.Password = user.Password;
                    dbEntity.RoleId = user.RoleId;
                    dbEntity.IsBlocked = user.IsBlocked;
                }
                else
                {
                    db.Users.Add(
                        new User
                        {
                            Login = user.Login,
                            Email = user.Email,
                            Password = user.Password,
                            RoleId = user.RoleId,
                            IsBlocked = user.IsBlocked
                        }
                    );
                }
                db.SaveChanges();
            }
        }


        public IQueryable<User> GetAll()
        {
            ApplicationContext db = new ApplicationContext();

            return db.Users;
        }


        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
