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
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly Kyles_Pizza_ShopContext _context;
        public OrderItemRepository(Kyles_Pizza_ShopContext context)
        { _context = context; }
        public List<OrderItem> GetAllOrderItems()
        {
            return _context.OrderItems.FromSqlRaw<OrderItem>($"select * from OrderItems").ToList();
        }
        public void InsertOrderItem(Guid orderId, int productId)
        {
            _context.Database.ExecuteSqlRaw($"insert into OrderItems (OrderId,ProductId) values ('{orderId}',{productId})");
            _context.SaveChanges();
        }
        public void Delete(Guid orderId, int productId)
        { _context.Database.ExecuteSqlRaw($"delete top (1) from OrderItems where OrderId = '{orderId}' and ProductId = {productId}");
            _context.SaveChanges();
        }

        public List<OrderItem> GetAllInstancesOfParticularProductTypeInAnOrder(Guid orderId, int productId)
        { return _context.OrderItems.FromSqlRaw<OrderItem>($"select * from OrderItems where OrderId = '{orderId}' and ProductId = {productId}").ToList(); }


    }
}
