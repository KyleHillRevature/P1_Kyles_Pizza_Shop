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

        public OrdersController(IOrderItemRepository orderItemRepository , IOrderRepository orderRepository, ICustomerRepository customerRepository,
        ILocationRepository locationRepository, IProductRepository productRepository, ILocationInventoryRepository locationInventoryRepository, 
        ILogger<OrdersController> logger)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _locationRepository = locationRepository;
            _productRepository = productRepository;
            _locationInventoryRepository = locationInventoryRepository;
            _logger = logger;
        }

        //method is async because copy pasted the template
        // GET: api/Customers/userType_and_id
        [HttpGet("idType={idType}&id={id}")]
        public IEnumerable<Order> GetOrders(string idType, int id)
        {
            List<Order> orders = new List<Order>();
           
                if (idType.Equals("customer")) orders = _orderRepository.AllOrdersForCustomer(id, Order.cartOrderDate);
                else orders = _orderRepository.AllOrdersForLocation(id, Order.cartOrderDate);
                
            return orders;
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
            Order orderOfInterest = _orderRepository.GetParticularOrder(order.OrderId); 

            //handles simple add of new orderItem
            if (LocationInventory.itemIsAvailable(orderOfInterest.LocationId, order.ProductId) && order.Action.Equals("add"))
            {
                Product product = _productRepository.GetProduct(order.ProductId);
                decimal totalPrice = orderOfInterest.TotalPrice + product.ProductPrice;
                _orderRepository.UpdateTotalPrice(order.OrderId, totalPrice);
                _orderItemRepository.InsertOrderItem(order.OrderId, product.Id);
                _locationInventoryRepository.DecreaseItemStockBy1(product.Id, orderOfInterest.LocationId);
                _logger.LogInformation($"{product.Name1} was added to order {order.OrderId}. Brought total price to {totalPrice}.");

                OrderView orderView = new OrderView()
                {
                    TotalPrice = totalPrice,
                    OrderItems = OrderView.GetOrderItemViews(order.OrderId),
                    actionSucceeded = true
                };
                _logger.LogInformation($"Order now has {orderView.OrderItems.Count} different types of items.");
                return orderView;
            }
            else if (order.Action.Equals("remove") && itemIsInCart(order.OrderId,order.ProductId))
            {
                Product product = _productRepository.GetProduct(order.ProductId);
                decimal totalPrice = orderOfInterest.TotalPrice - product.ProductPrice;
                _orderRepository.UpdateTotalPrice(order.OrderId, totalPrice);
                _orderItemRepository.Delete(order.OrderId, product.Id);
                _locationInventoryRepository.IncreaseItemStockBy1(product.Id, orderOfInterest.LocationId);                

                OrderView orderView = new OrderView()
                {
                    TotalPrice = totalPrice,
                    OrderItems = OrderView.GetOrderItemViews(order.OrderId),
                    actionSucceeded = true
                };

                return orderView;
            }
            else
            {
                OrderView orderView = new OrderView();
                orderView.actionSucceeded = false;
                orderView.TotalPrice = orderOfInterest.TotalPrice;
                orderView.OrderItems = OrderView.GetOrderItemViews(order.OrderId);
                if (order.Action.Equals("remove")) orderView.message = "Can't remove an item you don't have in your cart.";
                else orderView.message = "That item is out of stock.";
                return orderView;
            }
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

        public Boolean itemIsInCart(Guid orderId, int productId) 
        {
            List<OrderItem> orderItems = _orderItemRepository.GetAllInstancesOfParticularProductTypeInAnOrder(orderId,productId);
            if (orderItems.Count == 0) return false;
            else return true;
        }
    }
}
