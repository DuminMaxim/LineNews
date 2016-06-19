using System.Collections.Generic;

namespace Domain.Abstract
{
    public interface IRepository<T>
    {
        List<T> GetAll();

        void Add(T entity);

        void Delete(int id);
    }
}
