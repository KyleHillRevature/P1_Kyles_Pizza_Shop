using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Domain.Models;
using Project1.StoreApplication.Domain.ViewModels;
using Project1.StoreApplication.Domain.InputModels;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Project1.StoreApplication.Domain.Interfaces.Model;
using Project1.StoreApplication.Domain;

namespace Project1.StoreApplication.Business.Controllers
{
    [Route("html/api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrder _order;

        public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger, IOrder order)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _order = order;
        }

        //method is async because copy pasted the template
        // GET: api/Customers/userType_and_id
        [HttpGet("idType={idType}&id={id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string idType, int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (idType.Equals("customer"))
                {
                    List<Order> orders = await _orderRepository.AllOrdersForCustomerAsync(id, Order.cartOrderDate);
                    if (orders.FirstOrDefault() == null) return NoContent();
                    else return Ok(orders);
                }
            else return _orderRepository.AllOrdersForLocation(id, Order.cartOrderDate);
        }

        // GET: api/Orders/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Order>> GetOrder(Guid id)
        //{
        //    var order = await _context.Orders.FindAsync(id);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return order;
        //}



        [HttpPut("submitOrder")]
        public void submitOrder(OrderInput order)
        {
            _orderRepository.SubmitOrder(order.OrderId);
            _logger.LogInformation($"Order {order.OrderId} was submitted.");
            
        }


// PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<OrderView> PutOrder(OrderInput order)
        {
            Tuple<Order, Boolean, string> order1 = await _order.updateOrder(order);
            return ModelMapper.OrderToOrderView(order1);
        }
        

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public OrderView PostOrder(OrderInput order)
        {
            return _order.createOrder(order);
            #region
            //_context.Database.ExecuteSqlRaw($"insert into Orders values ('{orderID}',getdate(),{order.CustomerId},{order.LocationId},{order.TotalPrice})");
            //string orderItemsInsert = $"insert into OrderItems (OrderId,ProductId) values ";
            //string locationInventoryUpdate = "";
            //FormattableString data;
            //foreach (OrderItem orderItem in order.OrderItems)
            //{ 
            //    data = $"('{orderID}',{orderItem.ProductId}),";
            //    orderItemsInsert += data.ToString();
            //    data = $"update LocationInventory set Stock = Stock - 1 where ProductId = {orderItem.ProductId} and LocationId = {order.LocationId} ";
            //    locationInventoryUpdate += data.ToString();  
            //}
            //int lastComma = orderItemsInsert.Length - 1;
            //orderItemsInsert = orderItemsInsert.Remove(lastComma);
            //_context.Database.ExecuteSqlRaw(orderItemsInsert);
            //_context.Database.ExecuteSqlRaw(locationInventoryUpdate);
            //_context.SaveChanges();
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (OrderExists(order.Id))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            #endregion
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public void DeleteOrder(Guid id)
        {
            _orderRepository.Delete(id);
        }

        [HttpDelete]
        public void DeleteOrder()
        {
            _orderRepository.Delete(Order.cartOrderDate);
        }

        //private bool OrderExists(Guid id)
        //{
        //    return _context.Orders.Any(e => e.Id == id);
        //}

        
    }
}
