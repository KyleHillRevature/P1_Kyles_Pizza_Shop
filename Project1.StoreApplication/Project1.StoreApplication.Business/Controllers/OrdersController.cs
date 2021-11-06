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

namespace Project1.StoreApplication.Business.Controllers
{
    [Route("html/api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILocationInventoryRepository _locationInventoryRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrder _order;

        public OrdersController(IOrderItemRepository orderItemRepository , IOrderRepository orderRepository, ICustomerRepository customerRepository,
        ILocationRepository locationRepository, IProductRepository productRepository, ILocationInventoryRepository locationInventoryRepository, 
        ILogger<OrdersController> logger, IOrder order)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _locationRepository = locationRepository;
            _productRepository = productRepository;
            _locationInventoryRepository = locationInventoryRepository;
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
        public OrderView PutOrder(OrderInput order)
        {
            return _order.updateOrder(order);
        }
        

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public OrderView PostOrder(OrderInput order)
        {
            if (LocationInventory.itemIsAvailable(order.LocationId, order.ProductId) && order.Action.Equals("add"))
            {
                Guid orderID = Guid.NewGuid();
                Product product = _productRepository.GetProduct(order.ProductId);
                _orderRepository.AddNewOrder(orderID, Order.cartOrderDate, order.CustomerId, order.LocationId, product.ProductPrice);
                _orderItemRepository.InsertOrderItem(orderID, product.Id);
                _locationInventoryRepository.DecreaseItemStockBy1(product.Id,order.LocationId);
                
                OrderView orderView = new OrderView
                {
                    Id = orderID,
                    TotalPrice = product.ProductPrice,
                    OrderItems = new List<OrderItemView> { new OrderItemView() { Name1 = product.Name1, Quantity = 1 } },
                    actionSucceeded = true
                };

                return orderView;
            }
            else 
            {
                OrderView orderView = new OrderView();
                orderView.actionSucceeded = false;
                if (order.Action.Equals("remove")) orderView.message = "Can't remove an item you don't have in your cart.";
                else orderView.message = "That item is out of stock.";
                return orderView;
            }
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
