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
    public class CustomerRepositoryTests
    {
        //public DbContextOptions<Kyles_Pizza_ShopContext> options { get; set; } = new DbContextOptionsBuilder<Kyles_Pizza_ShopContext>()
        //    .UseInMemoryDatabase(databaseName: "TestDb")
        //    .Options;
        Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext();

        [Fact]
        public void FindCustomerTest()
        {
            //using (Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext(options))
            //{
                //_context.Database.ExecuteSqlRaw("delete from Customers");
                //_context.SaveChanges();
                _context.Database.EnsureDeleted();// delete any Db from a previous test
                _context.Database.EnsureCreated();// create a new Db... you will need to seed it again.
                string firstName = "Jason";
                string lastName = "Ellis";
                _context.Customers.Add(new Customer() { FirstName = firstName, LastName = lastName });
                _context.SaveChanges();

                CustomerRepository customerRepository = new CustomerRepository(_context);
                List<Customer> customer = customerRepository.FindCustomer(firstName, lastName);
                Assert.Equal(firstName, customer[0].FirstName);
                Assert.Equal(lastName, customer[0].LastName);
           // }
        }

        [Fact]
        public void AddCustomerTest()
        {
            //using (Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext(options))
            //{
                //_context.Database.ExecuteSqlRaw("delete from Customers");
                //_context.SaveChanges();
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
                Customer customer = new Customer() { FirstName = "Jason", LastName = "Ellis" };
                CustomerRepository customerRepository = new CustomerRepository(_context);
                int actualId = customerRepository.AddCustomer(customer);

                //Customer customer1 = _context.Customers.FromSqlRaw($"select * from Customers where FirstName = '{customer.FirstName}' and LastName = '{customer.LastName}'").First();
                Customer customer1 = _context.Customers.Where(c => c.FirstName.Equals("Jason") && c.LastName.Equals("Ellis")).FirstOrDefault();
                int expectedId = customer1.Id;
                Assert.Equal(expectedId, actualId);
           // }
        }

        [Fact]
        public void GetAllTest()
        {
            //using (Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext(options))
            //{
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
                //_context.Database.ExecuteSqlRaw("delete from Customers");
                //_context.SaveChanges();
                _context.Customers.Add(new Customer() { FirstName = "James", LastName = "Bond" });
                _context.Customers.Add(new Customer() { FirstName = "Aretha", LastName = "Franklin" });
                _context.Customers.Add(new Customer() { FirstName = "Rory", LastName = "McIlroy" });
                _context.SaveChanges();

                CustomerRepository customerRepository = new CustomerRepository(_context);
                List<Customer> customers = customerRepository.GetAll();
                //Assert.True(customers[0].FirstName.Equals("James"));
                //Assert.True(customers[1].LastName.Equals("Franklin"));
                //Assert.True(customers[2].FirstName.Equals("Rory"));
                Assert.Equal(3, customers.Count);
            //}
        }
    }
}
