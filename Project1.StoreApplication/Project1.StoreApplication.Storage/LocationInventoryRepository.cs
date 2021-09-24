using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using Project1.StoreApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Storage
{
    
    public class LocationInventoryRepository : ILocationInventoryRepository
    {
        private readonly Kyles_Pizza_ShopContext _context;

        public LocationInventoryRepository(Kyles_Pizza_ShopContext context)
        { _context = context; }

        public List<LocationInventory> GetLocationInventory(int LocationId)
        {
            return _context.LocationInventories.FromSqlRaw<LocationInventory>($"select * from LocationInventory where LocationId = {LocationId} order by ProductId").ToList();
        }
        public void DecreaseItemStockBy1(int productId, int locationId)
        { _context.Database.ExecuteSqlRaw($"update LocationInventory set Stock = Stock - 1 where ProductId = {productId} and LocationId = {locationId}");
            _context.SaveChanges();
        }
        public void IncreaseItemStockBy1(int productId, int locationId)
        {
            _context.Database.ExecuteSqlRaw($"update LocationInventory set Stock = Stock + 1 where ProductId = {productId} and LocationId = {locationId}");
            _context.SaveChanges();
        }
}
}
