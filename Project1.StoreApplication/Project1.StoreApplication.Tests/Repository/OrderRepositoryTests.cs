using System;
using Xunit;
using Project1.StoreApplication.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Project1.StoreApplication.Storage;
using System.Linq;

namespace Project1.StoreApplication.Tests.Repository
{
    public class OrderRepositoryTests
    {
        private Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext();
        
        [Fact]
        public void AllOrdersForCustomerTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();
            
            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Customers.Add(new Customer() { FirstName = "porky", LastName = "pig" });
            _context.Locations.Add(new Location() { CityName = "Toledo" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Customer customer2 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'porky'").First();
            Location location = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Toledo'").First();
            _context.Orders.Add(new Order() { Id = Guid.NewGuid(), OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location.Id, TotalPrice = 5 });
            _context.Orders.Add(new Order() { Id = Guid.NewGuid(), OrderDate = DateTime.Now, CustomerId = customer2.Id, LocationId = location.Id, TotalPrice = 5 });
            _context.Orders.Add(new Order() { Id = Guid.NewGuid(), OrderDate = DateTime.Now.AddYears(-100), CustomerId = customer1.Id, LocationId = location.Id, TotalPrice = 5 });
            _context.SaveChanges();

            OrderRepository orderRepository = new OrderRepository(_context);
            List<Order> orders = orderRepository.AllOrdersForCustomer(customer1.Id, Order.cartOrderDate);
            Assert.True(orders.Count == 1);
        }
        
        [Fact]
        public void AllOrdersForLocationTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Customers.Add(new Customer() { FirstName = "porky", LastName = "pig" });
            _context.Locations.Add(new Location() { CityName = "Toledo" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Customer customer2 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'porky'").First();
            Location location1 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Toledo'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            _context.Orders.Add(new Order() { Id = Guid.NewGuid(), OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location1.Id, TotalPrice = 5 });
            _context.Orders.Add(new Order() { Id = Guid.NewGuid(), OrderDate = DateTime.Now, CustomerId = customer2.Id, LocationId = location2.Id, TotalPrice = 5 });
            _context.Orders.Add(new Order() { Id = Guid.NewGuid(), OrderDate = DateTime.Now.AddYears(-100), CustomerId = customer1.Id, LocationId = location1.Id, TotalPrice = 5 });
            _context.SaveChanges();

            OrderRepository orderRepository = new OrderRepository(_context);
            List<Order> orders = orderRepository.AllOrdersForLocation(location1.Id, Order.cartOrderDate);
            Assert.True(orders.Count == 1);
        }
        
        [Fact]
        public void SubmitOrderTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId, OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5 });
            _context.SaveChanges();

            OrderRepository orderRepository = new OrderRepository(_context);
            orderRepository.SubmitOrder(orderId);
            Order order = _context.Orders.Find(orderId);
            Assert.True(order.OrderDate.Date.CompareTo(DateTime.Today) == 0);
        }

        [Fact]
        public void GetParticularOrderTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId, OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5 });
            _context.SaveChanges();

            OrderRepository orderRepository = new OrderRepository(_context);
            Order order = orderRepository.GetParticularOrder(orderId);
            Assert.Equal(orderId, order.Id);
        }
        [Fact]
        public void UpdateTotalPriceTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId, OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5.78M });
            _context.SaveChanges();

            OrderRepository orderRepository = new OrderRepository(_context);
            decimal number = 78.4M;
            orderRepository.UpdateTotalPrice(orderId, number);
            
            Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
            Order order = newContext.Orders.Find(orderId);
            Assert.True(number == order.TotalPrice);
        }

        [Fact]
        public void AddNewOrderTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId = Guid.NewGuid();
            OrderRepository orderRepository = new OrderRepository(_context);
            orderRepository.AddNewOrder(orderId, Order.cartOrderDate, customer1.Id, location2.Id, 25);

            Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
            Order order = newContext.Orders.Find(orderId);
            Assert.Equal(orderId, order.Id);
            Assert.Equal(customer1.Id, order.CustomerId);
            Assert.Equal(25, order.TotalPrice);
        }

        [Fact]
        public void DeleteSpecificCartTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId, OrderDate = DateTime.Now, CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5.78M });
            _context.SaveChanges();

            OrderRepository orderRepository = new OrderRepository(_context);
            orderRepository.Delete(orderId);

            Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
            Order order = newContext.Orders.Find(orderId);
            Assert.Null(order);
        }

        [Fact]
        public void DeleteAllCartsTest()
        {
            _context.Database.ExecuteSqlRaw("delete from Customers");
            _context.Database.ExecuteSqlRaw("delete from Locations");
            _context.Database.ExecuteSqlRaw("delete from Orders");
            _context.SaveChanges();

            _context.Customers.Add(new Customer() { FirstName = "Jason", LastName = "Ellis" });
            _context.Locations.Add(new Location() { CityName = "Columbus" });
            _context.SaveChanges();

            Customer customer1 = _context.Customers.FromSqlRaw<Customer>("select * from Customers where FirstName = 'Jason'").First();
            Location location2 = _context.Locations.FromSqlRaw<Location>("select * from Locations where CityName = 'Columbus'").First();
            Guid orderId1 = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId1, OrderDate = DateTime.Parse(Order.cartOrderDate), CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5.78M });
            Guid orderId2 = Guid.NewGuid();
            _context.Orders.Add(new Order() { Id = orderId2, OrderDate = DateTime.Parse(Order.cartOrderDate), CustomerId = customer1.Id, LocationId = location2.Id, TotalPrice = 5.78M });
            _context.SaveChanges();

            OrderRepository orderRepository = new OrderRepository(_context);
            orderRepository.Delete(Order.cartOrderDate);

            Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
            Order order1 = newContext.Orders.Find(orderId1);
            Order order2 = newContext.Orders.Find(orderId2);
            Assert.Null(order1);
            Assert.Null(order2);
        }
    }
}
