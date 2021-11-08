using Microsoft.Extensions.Logging;
using Project1.StoreApplication.Domain.InputModels;
using Project1.StoreApplication.Domain.Interfaces.Model;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using Project1.StoreApplication.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable

namespace Project1.StoreApplication.Domain.Models
{
    public partial class Order: IOrder
    {
        public Order()
        { OrderItems = new HashSet<OrderItem>(); }
        
        public Order(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IProductRepository productRepository, ILocationInventoryRepository locationInventoryRepository,
        ILogger<Order> logger, ILocationInventory locationInventory)
        {
            OrderItems = new HashSet<OrderItem>();
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _locationInventoryRepository = locationInventoryRepository;
            _locationInventory = locationInventory;
            _logger = logger;
        }
        public const string cartOrderDate = "1985-01-01";
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILocationInventoryRepository _locationInventoryRepository;
        private readonly ILogger<Order> _logger;
        private readonly ILocationInventory _locationInventory;

        public async Task<Tuple<Order, Boolean, string>> addItem(OrderInput order)
        {
            Order orderOfInterest = await _orderRepository.GetParticularOrder(order.OrderId);
            string message = ""; Boolean actionSucceeded = true;

            if (_locationInventory.itemIsAvailable(orderOfInterest.LocationId, order.ProductId))
            {
                Product product = _productRepository.GetProduct(order.ProductId);
                decimal totalPrice = orderOfInterest.TotalPrice + product.ProductPrice;
                _orderRepository.UpdateTotalPrice(order.OrderId, totalPrice);
                _orderItemRepository.InsertOrderItem(order.OrderId, product.Id);
                //_locationInventoryRepository.DecreaseItemStockBy1(product.Id, orderOfInterest.LocationId);
                _logger.LogInformation($"{product.Name1} was added to order {order.OrderId}. Brought total price to {totalPrice}.");
                orderOfInterest = await _orderRepository.GetParticularOrderNoTrack(order.OrderId);
                _logger.LogInformation($"Order now has {orderOfInterest.OrderItems.Count} different types of items.");
            }
            else { message = "That item is out of stock."; actionSucceeded = false; }
            return new Tuple<Order, Boolean, string>(orderOfInterest, actionSucceeded, message);
        }

        public async Task<Tuple<Order, Boolean, string>> removeItem(OrderInput order)
        {
            Order orderOfInterest = await _orderRepository.GetParticularOrder(order.OrderId);
            string message = ""; Boolean actionSucceeded = true;

            if (itemIsInCart(order.OrderId, order.ProductId))
            {
                Product product = _productRepository.GetProduct(order.ProductId);
                decimal totalPrice = orderOfInterest.TotalPrice - product.ProductPrice;
                _orderRepository.UpdateTotalPrice(order.OrderId, totalPrice);
                _orderItemRepository.Delete(order.OrderId, product.Id);
                //_locationInventoryRepository.IncreaseItemStockBy1(product.Id, orderOfInterest.LocationId);
                return new Tuple<Order, Boolean, string>(await _orderRepository.GetParticularOrderNoTrack(order.OrderId), true, "");
            }
            else { message = "Can't remove an item you don't have in your cart."; actionSucceeded = false; }
            return new Tuple<Order, Boolean, string>(orderOfInterest, actionSucceeded, message);
        }

        public async Task<Tuple<Order, Boolean, string>> createOrder(OrderInput order) 
        {
            Guid orderID = Guid.NewGuid();
            _orderRepository.AddNewOrder(orderID, Order.cartOrderDate, order.CustomerId, order.LocationId);
            order.OrderId = orderID;
            return await addItem(order);
        }

        public Boolean itemIsInCart(Guid orderId, int productId)
        {
            List<OrderItem> orderItems = _orderItemRepository.GetAllInstancesOfParticularProductTypeInAnOrder(orderId, productId);
            if (orderItems.Count == 0) return false;
            else return true;
        }
    }

    
    
}
