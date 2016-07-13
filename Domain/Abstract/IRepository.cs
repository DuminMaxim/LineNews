using System.Collections.Generic;
using System.Linq;

namespace Domain.Abstract
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        void Add(T entity);

        void Delete(int id);
    }
}
