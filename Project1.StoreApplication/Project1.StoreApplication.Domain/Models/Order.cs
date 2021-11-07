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
        ILogger<Order> logger)
        {
            OrderItems = new HashSet<OrderItem>();
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _locationInventoryRepository = locationInventoryRepository;
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

        public async Task<Tuple<Order,Boolean,string>> updateOrder(OrderInput order)
        {
            Order orderOfInterest = await _orderRepository.GetParticularOrder(order.OrderId);

            //handles simple add of new orderItem
            if (LocationInventory.itemIsAvailable(orderOfInterest.LocationId, order.ProductId) && order.Action.Equals("add"))
            {
                Product product = _productRepository.GetProduct(order.ProductId);
                decimal totalPrice = orderOfInterest.TotalPrice + product.ProductPrice;
                _orderRepository.UpdateTotalPrice(order.OrderId, totalPrice);
                _orderItemRepository.InsertOrderItem(order.OrderId, product.Id);
                _locationInventoryRepository.DecreaseItemStockBy1(product.Id, orderOfInterest.LocationId);
                _logger.LogInformation($"{product.Name1} was added to order {order.OrderId}. Brought total price to {totalPrice}.");
                orderOfInterest = await _orderRepository.GetParticularOrderNoTrack(order.OrderId);
                _logger.LogInformation($"Order now has {orderOfInterest.OrderItems.Count} different types of items.");
                return new Tuple<Order,Boolean,string>(orderOfInterest,true,"");
            }
            else if (order.Action.Equals("remove") && itemIsInCart(order.OrderId, order.ProductId))
            {
                Product product = _productRepository.GetProduct(order.ProductId);
                decimal totalPrice = orderOfInterest.TotalPrice - product.ProductPrice;
                _orderRepository.UpdateTotalPrice(order.OrderId, totalPrice);
                _orderItemRepository.Delete(order.OrderId, product.Id);
                _locationInventoryRepository.IncreaseItemStockBy1(product.Id, orderOfInterest.LocationId);
                return new Tuple<Order,Boolean,string>( await _orderRepository.GetParticularOrderNoTrack(order.OrderId),true,"");
            }
            else
            {
                string message = "";
                if (order.Action.Equals("remove")) message = "Can't remove an item you don't have in your cart.";
                else message = "That item is out of stock.";
                return new Tuple<Order, Boolean, string>(orderOfInterest, false, message);
            }

        }

        public OrderView createOrder(OrderInput order) 
        {
            if (LocationInventory.itemIsAvailable(order.LocationId, order.ProductId) && order.Action.Equals("add"))
            {
                Guid orderID = Guid.NewGuid();
                Product product = _productRepository.GetProduct(order.ProductId);
                _orderRepository.AddNewOrder(orderID, Order.cartOrderDate, order.CustomerId, order.LocationId, product.ProductPrice);
                _orderItemRepository.InsertOrderItem(orderID, product.Id);
                _locationInventoryRepository.DecreaseItemStockBy1(product.Id, order.LocationId);

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
        }

        public Boolean itemIsInCart(Guid orderId, int productId)
        {
            List<OrderItem> orderItems = _orderItemRepository.GetAllInstancesOfParticularProductTypeInAnOrder(orderId, productId);
            if (orderItems.Count == 0) return false;
            else return true;
        }
    }

    
    
}
