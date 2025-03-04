using System.Linq.Expressions;

namespace BlulkyBook.DataAccess.Repository.Interface
{
    public interface IRepositroy<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includePropeties = null);
        T Get(Expression<Func<T, bool>> filter, string? includePropeties = null, bool Tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
