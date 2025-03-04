using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;

namespace BlulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository(ApplicationDbContext db) : Repository<ShoppingCart>(db), IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(ShoppingCart cart)
        {
            if (cart != null)
            {
                _db.ShoppingCart.Update(cart);
            }
        }
    }
}
