using BlulkyBook.Models;

namespace BlulkyBook.DataAccess.Repository.Interface
{
    public interface ICategoryRepository : IRepositroy<Category>
    {
        void Update(Category obj);
        void Save();
    }
}
