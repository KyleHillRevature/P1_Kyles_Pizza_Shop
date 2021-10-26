using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Domain.Models
{
    public partial class Product
    {
        public Product()
        {
            //LocationInventories = new HashSet<LocationInventory>();
            //OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Name1 { get; set; }
        public string Description1 { get; set; }
        public decimal ProductPrice { get; set; }

        //public virtual ICollection<LocationInventory> LocationInventories { get; set; }
        //public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
