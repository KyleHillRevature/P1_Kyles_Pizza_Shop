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
    public class LocationInventoryRepositoryTests
    {
        //public DbContextOptions<Kyles_Pizza_ShopContext> options { get; set; } = new DbContextOptionsBuilder<Kyles_Pizza_ShopContext>()
        //    .UseInMemoryDatabase(databaseName: "TestDb")
        //    .Options;

        //[Fact]
        //public void GetLocationInventoryTest()
        //{
        //    using (Kyles_Pizza_ShopContext _context = new Kyles_Pizza_ShopContext(options))
        //    {
        //        //    _context.Database.ExecuteSqlRaw("delete from LocationInventory");
        //        //_context.Database.ExecuteSqlRaw("delete from Locations");
        //        //_context.Database.ExecuteSqlRaw("delete from Products");
        //        //_context.SaveChanges();
        //        _context.Database.EnsureDeleted();// delete any Db from a previous test
        //        _context.Database.EnsureCreated();// create a new Db... you will need to seed it again.

        //        _context.Database.ExecuteSqlRaw(@"insert into Products values ('Calzone','Cheese and tomato',6),('Cheese pizza','Extra cheese for no cost!',11),
        //    ('Wings', 'Nice and greasy', 8)");
        //        _context.Locations.Add(new Location() { CityName = "Cleveland" });
        //        _context.SaveChanges();

        //        List<Product> products = _context.Products.FromSqlRaw<Product>("select * from Products").ToList();
        //        Location location = _context.Locations.FromSqlRaw<Location>("select * from Locations").First();
        //        _context.LocationInventories.Add(new LocationInventory() { LocationId = location.Id, ProductId = products[0].Id, Stock = 10 });
        //        _context.LocationInventories.Add(new LocationInventory() { LocationId = location.Id, ProductId = products[1].Id, Stock = 10 });
        //        _context.LocationInventories.Add(new LocationInventory() { LocationId = location.Id, ProductId = products[2].Id, Stock = 10 });
        //        _context.SaveChanges();

        //        Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
        //        LocationInventoryRepository locationInventoryRepository = new LocationInventoryRepository(newContext);
        //        List<LocationInventory> locationInventories = locationInventoryRepository.GetLocationInventory(location.Id);
        //        Assert.Equal(location.Id, locationInventories[0].LocationId);
        //        Assert.Equal(products[1].Id, locationInventories[1].ProductId);
        //        Assert.Equal(10, locationInventories[2].Stock);
        //        Assert.Equal(3, locationInventories.Count);
        //    }
        //}

        //[Fact]
        //public void DecreaseItemStockBy1Test() 
        //{
        //    _context.Database.ExecuteSqlRaw("delete from LocationInventory");
        //    _context.Database.ExecuteSqlRaw("delete from Locations");
        //    _context.Database.ExecuteSqlRaw("delete from Products");
        //    _context.SaveChanges();

        //    _context.Products.Add(new Product() {Name1="Calzone",Description1="Cheese and tomato",ProductPrice=6 });
        //    _context.Locations.Add(new Location() { CityName = "Cleveland" });
        //    _context.SaveChanges();

        //    Location location = _context.Locations.FromSqlRaw<Location>("select * from Locations").First();
        //    Product product = _context.Products.FromSqlRaw<Product>("select * from Products").First();
        //    _context.LocationInventories.Add(new LocationInventory() { LocationId = location.Id, ProductId = product.Id, Stock = 10 });
        //    _context.SaveChanges();

        //    LocationInventoryRepository locationInventoryRepository= new LocationInventoryRepository(_context);
        //    locationInventoryRepository.DecreaseItemStockBy1(product.Id, location.Id);

        //    Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
        //    LocationInventory locationInventory = newContext.LocationInventories.FromSqlRaw<LocationInventory>("select * from LocationInventory").First();
        //    Assert.Equal(9, locationInventory.Stock);
        //}

        //[Fact]
        //public void IncreaseItemStockBy1Test()
        //{
        //    _context.Database.ExecuteSqlRaw("delete from LocationInventory");
        //    _context.Database.ExecuteSqlRaw("delete from Locations");
        //    _context.Database.ExecuteSqlRaw("delete from Products");
        //    _context.SaveChanges();

        //    _context.Products.Add(new Product() { Name1 = "Calzone", Description1 = "Cheese and tomato", ProductPrice = 6 });
        //    _context.Locations.Add(new Location() { CityName = "Cleveland" });
        //    _context.SaveChanges();

        //    Location location = _context.Locations.FromSqlRaw<Location>("select * from Locations").First();
        //    Product product = _context.Products.FromSqlRaw<Product>("select * from Products").First();
        //    _context.LocationInventories.Add(new LocationInventory() { LocationId = location.Id, ProductId = product.Id, Stock = 10 });
        //    _context.SaveChanges();

        //    LocationInventoryRepository locationInventoryRepository = new LocationInventoryRepository(_context);
        //    locationInventoryRepository.IncreaseItemStockBy1(product.Id, location.Id);

        //    Kyles_Pizza_ShopContext newContext = new Kyles_Pizza_ShopContext();
        //    LocationInventory locationInventory = newContext.LocationInventories.FromSqlRaw<LocationInventory>("select * from LocationInventory").First();
        //    Assert.Equal(11, locationInventory.Stock);
        //}
    }
}
