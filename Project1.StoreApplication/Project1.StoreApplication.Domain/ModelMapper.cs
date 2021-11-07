using Project1.StoreApplication.Domain.Models;
using Project1.StoreApplication.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain
{
    public class ModelMapper
    {
        public static OrderView OrderToOrderView(Tuple<Order, Boolean, string> tuple)
        {
            var itemsToItemViews =
               from item in tuple.Item1.OrderItems
               group item by item.Product.Name1;

            List<OrderItemView> orderItemViews = new List<OrderItemView>();
            foreach (var obj in itemsToItemViews)
                orderItemViews.Add(
                    new OrderItemView
                    {
                        Name1 = obj.Key,
                        Quantity = obj.Count()
                    }
                );

            OrderView orderView = new OrderView()
            {
                Id = tuple.Item1.Id,
                TotalPrice = tuple.Item1.TotalPrice,
                OrderItems = orderItemViews,
                actionSucceeded = tuple.Item2,
                message = tuple.Item3
            };
           
            return orderView;
        }
    }
}
