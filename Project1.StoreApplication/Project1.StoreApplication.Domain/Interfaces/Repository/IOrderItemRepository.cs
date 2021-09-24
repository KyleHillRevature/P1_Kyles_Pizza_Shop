using Project1.StoreApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain.Interfaces.Repository
{
    public interface IOrderItemRepository
    {
        List<OrderItem> GetAllOrderItems();
        void InsertOrderItem(Guid orderId, int productId);
        void Delete(Guid orderId, int productId);
        List<OrderItem> GetAllInstancesOfParticularProductTypeInAnOrder(Guid orderId, int productId);

    }
}
