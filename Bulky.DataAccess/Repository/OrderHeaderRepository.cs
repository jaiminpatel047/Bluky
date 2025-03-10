using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader entity)
        {
            _db.OrderHeaders.Update(entity);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
