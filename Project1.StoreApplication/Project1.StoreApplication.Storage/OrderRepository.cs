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
    public class OrderRepository : IOrderRepository
    {
        private readonly Kyles_Pizza_ShopContext _context;
        public OrderRepository(Kyles_Pizza_ShopContext context)
        { _context = context; }

        public List<Order> AllOrdersForCustomer(int customerId, string cartMarkerDate)
        {
            //return _context.Orders.FromSqlRaw<Order>($"select * from Orders where CustomerID = {customerId} and OrderDate > '{cartMarkerDate}'  order by OrderDate").ToList();
            return _context.Orders.Include(o => o.Customer)
                                    .Include(o => o.Location)
                                    .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Product)
                                    .Where(o => o.CustomerId == customerId && o.OrderDate > DateTime.Parse(cartMarkerDate))
                                    .ToList();
        }
        public List<Order> AllOrdersForLocation(int locationId, string cartMarkerDate)
        {
            return _context.Orders.FromSqlRaw<Order>($"select * from Orders where LocationID = {locationId} and OrderDate > '{cartMarkerDate}' order by OrderDate").ToList();
        }
        public void SubmitOrder(Guid orderId)
        {
            _context.Database.ExecuteSqlRaw($"update Orders set OrderDate = getdate() where Id = '{orderId}'");
            _context.SaveChanges();
        }
        public Order GetParticularOrder(Guid orderId)
        { return _context.Orders.FromSqlRaw<Order>($"select * from Orders where Id = '{orderId}'").First(); }
        public void UpdateTotalPrice(Guid orderId, decimal totalPrice)
        { _context.Database.ExecuteSqlRaw($"update Orders set TotalPrice = {totalPrice} where Id = '{orderId}'");_context.SaveChanges(); }
        public void AddNewOrder(Guid orderId, string cartMarkerDate, int customerId, int locationId, decimal productPrice)
        {
            _context.Database.ExecuteSqlRaw($"insert into Orders values ('{orderId}','{cartMarkerDate}',{customerId},{locationId},{productPrice})");
            _context.SaveChanges();
        }
        public void Delete(Guid orderId)
        {
            _context.Database.ExecuteSqlRaw($"delete from Orders where Id = '{orderId}'");
            _context.SaveChanges();
        }
        public void Delete(string cartMarkerDate)
        {
            _context.Database.ExecuteSqlRaw($"delete from Orders where OrderDate = '{cartMarkerDate}'");
            _context.SaveChanges();
        }
    }
}
