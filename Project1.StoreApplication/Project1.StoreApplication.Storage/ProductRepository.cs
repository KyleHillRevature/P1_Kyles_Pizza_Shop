using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using Project1.StoreApplication.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Project1.StoreApplication.Storage
{
    public class ProductRepository : IProductRepository
    {
        private readonly Kyles_Pizza_ShopContext _context;

        public ProductRepository(Kyles_Pizza_ShopContext context) 
        { _context = context; }
    
        public List<Product> GetProducts()
        {
            return _context.Products.FromSqlRaw<Product>("select * from Products order by Id").ToList();
        }
        public Product GetProduct(int productId) 
        {return _context.Products.FromSqlRaw<Product>($"select * from Products where Id = {productId}").First(); }
    }
}
