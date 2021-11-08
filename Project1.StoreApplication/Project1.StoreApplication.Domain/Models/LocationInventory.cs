using Project1.StoreApplication.Domain.Interfaces.Model;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

#nullable disable

namespace Project1.StoreApplication.Domain.Models
{
    public partial class LocationInventory : ILocationInventory
    {
        public LocationInventory() { }
        public LocationInventory(ILocationInventoryRepository locationInventoryRepo)
        { _locationInventoryRepo = locationInventoryRepo; }
        private readonly ILocationInventoryRepository _locationInventoryRepo;
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }

        public Boolean itemIsAvailable(int locationId, int productId)
        {
            int stock = _locationInventoryRepo.GetStock(locationId,productId);
            if (stock > 0) return true;
            else return false;
        }
    }
}
