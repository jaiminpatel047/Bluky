using System.Linq.Expressions;

namespace BlulkyBook.DataAccess.Repository.Interface
{
    public interface IRepositroy<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
