using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Domain.Models
{
    public partial class Location
    {
        public Location()
        {
            LocationInventories = new HashSet<LocationInventory>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<LocationInventory> LocationInventories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
