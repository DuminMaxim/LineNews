using Domain.Abstract;
using Domain.Entity;
using Domain.EF;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class BlogRepository : IRepository<Blog>
    {
        public void Add(Blog blog)
        {
            if (blog != null)
            {
                // Если id новости 0, новость добавляется
                // иначе изменяется существующая
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (blog.BlogId == 0)
                    {
                        db.Blogs.Add(blog);
                    }
                    else
                    {
                        Blog dbEntity = db.Blogs.Find(blog.BlogId);
                        if (dbEntity != null)
                        {
                            dbEntity.Title = blog.Title;
                            dbEntity.Description = blog.Description;

                            // Условие нужно, чтобы изображение не перезаписывалось пустым значением 
                            //(происходит при редактировании новости)
                            if (blog.ImageData != null)
                            {
                                dbEntity.ImageData = blog.ImageData;
                                dbEntity.ImageExtention = blog.ImageExtention;
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
        }


        public void Delete(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Blog blog = db.Blogs.Find(id);
                if(blog != null)
                {
                    db.Blogs.Remove(blog);
                    db.SaveChanges();
                }
            }
        }


        public List<Blog> GetAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Load();
                return db.Blogs.Include("Comments").ToList();
            }
        }
    }
}
