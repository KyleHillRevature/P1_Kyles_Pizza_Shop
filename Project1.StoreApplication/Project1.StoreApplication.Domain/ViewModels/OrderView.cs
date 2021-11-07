using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project1.StoreApplication.Domain.ViewModels
{
    public class OrderView
    {
        public OrderView()
        {
            OrderItems = new HashSet<OrderItemView>();
        }
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public Boolean actionSucceeded { get; set; }
        public virtual ICollection<OrderItemView> OrderItems { get; set; }
        public string message { get; set; }
    }
}
