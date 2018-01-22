using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project2_Cooperation.Data;
using Project2_Cooperation.Models;

namespace Project2_Cooperation.Services
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext _db;

        public EFProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Product> Products => _db.Products;

        public void CreateProduct(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void DeleteProduct(int Id)
        {
            Product productToDelete = _db.Products.FirstOrDefault(p=> p.ProductId == Id);

            if (productToDelete != null)
            {
                _db.Products.Remove(productToDelete);
                _db.SaveChanges();
            }            
        }

        public void UpdateProduct(Product product)
        {
            if (product != null)
            {
                _db.Products.Update(product);
                _db.SaveChanges();
            }
        }
    }
}
