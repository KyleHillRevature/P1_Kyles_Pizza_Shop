using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Domain.Models;
using Project1.StoreApplication.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project1.StoreApplication.Tests
{
    public class ProductRepositoryTests
    {
        Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext();

        [Fact]
        public void GetProducts()
        {
            _context.Database.ExecuteSqlRaw("delete from Products");
            _context.SaveChanges();
            _context.Database.ExecuteSqlRaw(@"insert into Products values ('Calzone','Cheese and tomato',6),('Cheese pizza','Extra cheese for no cost!',11),
            ('Wings', 'Nice and greasy', 8)");
            _context.SaveChanges();

            ProductRepository productRepository = new ProductRepository(_context);
            List<Product> products = productRepository.GetProducts();
            Assert.True(products[0].Name1.Equals("Calzone"));
            Assert.True(products[1].Description1.Equals("Extra cheese for no cost!"));
            Assert.True(products[2].ProductPrice == 8);
            Assert.Equal(3, products.Count);
        }

        [Fact]
        public void GetProductTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Products");
            _context.SaveChanges();

            _context.Products.Add(new Product() { Name1 = "Calzone", Description1 = "A little spicy", ProductPrice = 5 });
            _context.SaveChanges();

            Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
            Product product = newContext.Products.FromSqlRaw<Product>("select * from Products").First();

            ProductRepository productRepository = new ProductRepository(_context);
            Product product1 = productRepository.GetProduct(product.Id);
            Assert.True("Calzone".Equals(product1.Name1));
            Assert.True("A little spicy".Equals(product1.Description1));
        }
    }
}
