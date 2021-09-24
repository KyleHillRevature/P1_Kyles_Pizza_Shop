using Project1.StoreApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain.Interfaces.Repository
{
    public interface ILocationInventoryRepository
    {
        List<LocationInventory> GetLocationInventory(int LocationId);
        public void DecreaseItemStockBy1(int productId, int locationId);
        void IncreaseItemStockBy1(int productId, int locationId);
    }
}
