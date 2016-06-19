using Domain.Abstract;
using Domain.Entity;
using Domain.EF;
using System.Collections.Generic;
using System.Linq;


namespace Domain.Concrete
{
    public class CommentRepository : IRepository<Comment>
    {
        public void Add(Comment comment)
        {
            if (comment != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    comment.User = db.Users.Find(comment.User.Login);
                    comment.Blog = db.Blogs.Find(comment.Blog.BlogId);
                    db.Comments.Add(comment);
                    db.SaveChanges();
                }
            }
        }


        public void Delete(int commentId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Comment comment = db.Comments.Find(commentId);
                if (comment != null)
                {
                    db.Comments.Remove(comment);
                    db.SaveChanges();
                }
            }
        }


        public List<Comment> GetAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Comments.Include("Users").Include("Blogs").ToList();
            }
        }
    }
}
