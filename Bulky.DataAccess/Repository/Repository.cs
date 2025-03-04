using BlulkyBook.DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepositroy<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            _db.Product.Include(u => u.Category).Include(i => i.CategoryID);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includePropeties = null, bool Tracked = false)
        {
            IQueryable<T> entity;
            if (Tracked)
            {
                entity = dbSet;
            }
            else
            {
                entity = dbSet.AsNoTracking();
            }
            
            entity = entity.Where(filter);
            if (!string.IsNullOrEmpty(includePropeties))
            {
                foreach (var includepro in includePropeties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    entity = entity.Include(includePropeties);
                }
            }
            return entity.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includePropeties = null)
        {
            IQueryable<T> entity = dbSet;
            if (filter != null)
            {
                entity = entity.Where(filter);
            }
            if (!string.IsNullOrEmpty(includePropeties))
            {
                foreach (var includepro in includePropeties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    entity = entity.Include(includePropeties);
                }
            }
            return entity.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
