using Project1.StoreApplication.Domain.InputModels;
using Project1.StoreApplication.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain.Interfaces.Model
{
    public interface IOrder
    {
        public OrderView updateOrder(OrderInput order);
        public Boolean itemIsInCart(Guid orderId, int productId);
    }
}
