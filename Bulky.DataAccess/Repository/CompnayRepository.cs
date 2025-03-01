using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;

namespace BlulkyBook.DataAccess.Repository
{
    public class CompnayRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompnayRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Company entity)
        {
            _db.Company.Update(entity);
        }
    }
}
