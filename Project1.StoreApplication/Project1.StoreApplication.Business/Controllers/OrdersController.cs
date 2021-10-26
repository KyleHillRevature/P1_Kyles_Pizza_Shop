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
            //List<OrderItem> orderItems = new List<OrderItem>();
            //List<Customer> customers = new List<Customer>();
            //List<Location> locations = new List<Location>();
            //List<string> orderIdList = new List<string>();
            //List<Product> products = new List<Product>();
            List<OrderView> orderViews = new List<OrderView>();
            //List<OrderItemView> orderItemViews = new List<OrderItemView>();
            //try
            //{
                //throw new Exception("Was not able to access the database for the customers orders.");
                if (idType.Equals("customer")) orders = _orderRepository.AllOrdersForCustomer(id, Order.cartOrderDate);
                else orders = _orderRepository.AllOrdersForLocation(id, Order.cartOrderDate);
                
            //}
            //catch (Exception ex) {
            //    _logger.LogError(ex.Message);
            //    _logger.LogError(ex.StackTrace);
            //}
            #region
            //List<int> locationIdList = new List<int>();
            //foreach (Order order in orders)
            //{ 
            //    if (!locationIdList.Contains(order.LocationId))
            //        locationIdList.Add(order.LocationId);
            //    orderIdList.Add(order.Id.ToString());
            //}
            //string locationIdListString = string.Join<int>(",", locationIdList);
            //string orderIdListString = '\''+ string.Join<string>("','", orderIdList) + '\'';
            //orderItems = _context.OrderItems.FromSqlRaw<OrderItem>($"select * from OrderItems where OrderId in ({orderIdListString})").ToList();
            //customers = _context.Customers.FromSqlRaw<Customer>($"select * from Customers where Id = {id}").ToList();
            //locations = _context.Locations.FromSqlRaw<Location>($"select * from Locations where Id in ({locationIdListString})").ToList();
            //products = _context.Products.FromSqlRaw<Product>($"select * from Products").ToList();
            #endregion
            //orderItems = _orderItemRepository.GetAllOrderItems();
            //customers = _customerRepository.GetAll();
            //locations = _locationRepository.GetLocations();
            //products = _productRepository.GetProducts();

            //foreach (Order order in orders)
            //{
            //    OrderView orderView = new OrderView()
            //    {
            //        CustomerId = order.CustomerId,
            //        Id = order.Id,
            //        LocationId = order.LocationId,
            //        OrderDate = order.OrderDate,
            //        TotalPrice = order.TotalPrice
            //    };
            //    orderViews.Add(orderView);
            //}

            //foreach (OrderItem orderItem in orderItems)
            //{
            //    OrderItemView orderItemView = new OrderItemView();
            //    {
            //        orderItemView.Id = orderItem.Id;
            //        orderItemView.OrderId = orderItem.OrderId;
            //        orderItemView.ProductId = orderItem.ProductId;
            //    }
            //    orderItemViews.Add(orderItemView);        
            //}
            //foreach (OrderItemView orderItemView in orderItemViews) 
            //    foreach (Product product in products)
            //        if (product.Id == orderItemView.ProductId)
            //            {orderItemView.Name1 = product.Name1; orderItemView.ProductPrice = product.ProductPrice;}


            //foreach (OrderView orderView in orderViews) {
            //    foreach (OrderItemView orderItemView in orderItemViews)
            //        if (orderView.Id == orderItemView.OrderId)
            //            orderView.OrderItems.Add(orderItemView);
            //    foreach (Customer customer in customers)
            //        if (orderView.CustomerId == customer.Id)
            //            orderView.CustomerName = customer.FirstName + " " + customer.LastName;
            //    foreach (Location location in locations)
            //        if (orderView.LocationId == location.Id)
            //            orderView.LocationName = location.CityName; 
            //}
            //if (orderViews.Count != 0)
            //{
            //    if (idType.Equals("customer")) _logger.LogInformation($"{orderViews[0].CustomerName} viewed their {orderViews.Count} orders. ");
            //    else _logger.LogInformation($"{orderViews[0].LocationName} location viewed their {orderViews.Count} orders. ");
            //    _logger.LogInformation($"The order dates range from {orderViews[0].OrderDate} up to {orderViews.LastOrDefault().OrderDate}");
            //}
            //else _logger.LogInformation($"{idType} {id} viewed their empty list of orders.");
            
            return orders;
            //return orderViews;
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
