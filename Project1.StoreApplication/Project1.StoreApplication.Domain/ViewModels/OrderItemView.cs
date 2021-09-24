using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain.ViewModels
{
    public class OrderItemView
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public string Name1 { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

    }
}
