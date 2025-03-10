using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlulkyBook.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetail>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetail entity)
        {
            _db.OrderDetails.Update(entity);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
