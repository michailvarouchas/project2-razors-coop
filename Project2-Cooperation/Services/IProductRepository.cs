using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int Id);
    }
}
