using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entity;
using System.Web.Mvc;

namespace WebUI.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<BlogRepository>().As<IRepository<Blog>>();
            builder.RegisterType<CommentRepository>().As<IRepository<Comment>>();
            builder.RegisterType<UserRepository>().As<IRepository<User>>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}