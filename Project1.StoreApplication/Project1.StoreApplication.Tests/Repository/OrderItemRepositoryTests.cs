using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Domain.Models;
using Project1.StoreApplication.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project1.StoreApplication.Tests.Repository
{
    public class OrderItemRepositoryTests
    {
        Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext();

        [Fact]
        public void GetAllOrderItemsTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.Database.ExecuteSqlRaw("delete from Products");
            _context.Database.ExecuteSqlRaw("delete from OrderItems");
            _context.SaveChanges();

            _context.Database.ExecuteSqlRaw(@"insert into Products values ('Calzone','Cheese and tomato',6),('Cheese pizza','Extra cheese for no cost!',11)");
            _context.SaveChanges();

            Product product1 = _context.Products.FromSqlRaw<Product>("select * from Products where Name1 = 'Calzone'").First();
            Product product2 = _context.Products.FromSqlRaw<Product>("select * from Products where Name1 = 'Cheese pizza'").First();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId, OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5 });
            _context.SaveChanges();

            _context.OrderItems.Add(new OrderItem { OrderId=orderId,ProductId=product1.Id});
            _context.OrderItems.Add(new OrderItem { OrderId=orderId,ProductId=product1.Id});
            _context.OrderItems.Add(new OrderItem { OrderId=orderId,ProductId=product2.Id});
            _context.SaveChanges();

            OrderItemRepository orderItemRepository = new OrderItemRepository(_context);
            List<OrderItem> orderItems = orderItemRepository.GetAllOrderItems();
            Assert.Equal(3, orderItems.Count);
            Assert.Equal(orderId, orderItems[0].OrderId);
            Assert.Equal(product2.Id, orderItems[2].ProductId);

        }

        [Fact]
        public void InsertOrderItemTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.Database.ExecuteSqlRaw("delete from Products");
            _context.Database.ExecuteSqlRaw("delete from OrderItems");
            _context.SaveChanges();

            _context.Database.ExecuteSqlRaw(@"insert into Products values ('Calzone','Cheese and tomato',6),('Cheese pizza','Extra cheese for no cost!',11)");
            _context.SaveChanges();

            Product product1 = _context.Products.FromSqlRaw<Product>("select * from Products where Name1 = 'Calzone'").First();
            Product product2 = _context.Products.FromSqlRaw<Product>("select * from Products where Name1 = 'Cheese pizza'").First();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId, OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5 });
            _context.SaveChanges();

            OrderItemRepository orderItemRepository = new OrderItemRepository(_context);
            orderItemRepository.InsertOrderItem(orderId, product1.Id);
            orderItemRepository.InsertOrderItem(orderId, product2.Id);
            orderItemRepository.InsertOrderItem(orderId, product2.Id);

            List<OrderItem> orderItems = _context.OrderItems.FromSqlRaw<OrderItem>("select * from OrderItems").ToList();
            Assert.True(orderItems.Count == 3);
            
        }




        public void InsertOrderItem(Guid orderId, int productId)
        {
            _context.Database.ExecuteSqlRaw($"insert into OrderItems (OrderId,ProductId) values ('{orderId}',{productId})");
            _context.SaveChanges();
        }
    }
}
