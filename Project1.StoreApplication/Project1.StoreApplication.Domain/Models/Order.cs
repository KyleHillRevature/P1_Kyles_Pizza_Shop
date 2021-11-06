using Microsoft.Extensions.Logging;
using Project1.StoreApplication.Domain.InputModels;
using Project1.StoreApplication.Domain.Interfaces.Model;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using Project1.StoreApplication.Domain.ViewModels;
using System;
using System.Collections.Generic;

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

        public OrderView updateOrder(OrderInput order)
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
            else if (order.Action.Equals("remove") && itemIsInCart(order.OrderId, order.ProductId))
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

        public Boolean itemIsInCart(Guid orderId, int productId)
        {
            List<OrderItem> orderItems = _orderItemRepository.GetAllInstancesOfParticularProductTypeInAnOrder(orderId, productId);
            if (orderItems.Count == 0) return false;
            else return true;
        }
    }

    
    
}
