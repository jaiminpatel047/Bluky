using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product entity)
        {
            var productDetail = _db.Product.FirstOrDefault(u => u.Id == entity.Id);
            if (productDetail != null)
            {
                productDetail.Title = entity.Title;
                productDetail.Description = entity.Description;
                productDetail.ISBN = entity.ISBN;
                productDetail.Author = entity.Author;
                productDetail.ListPrice = entity.ListPrice;
                productDetail.Price = entity.Price;
                productDetail.Price50 = entity.Price50;
                productDetail.Price100 = entity.Price100;
                productDetail.CategoryID = entity.CategoryID;
                if (entity.ImageURl != null)
                {
                    productDetail.ImageURl = entity.ImageURl;
                }
            }
        }
    }
}
