using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [Route("html/api/[controller]/[action]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrder _order;

        public OrderItemController(IOrder order) {
            _order = order;
        }

        public async Task<OrderView> Add(OrderInput order)
        {
            Tuple<Order, Boolean, string> order1 = await _order.addItem(order);
            return ModelMapper.OrderToOrderView(order1);
        }

        public async Task<OrderView> Remove(OrderInput order)
        {
            Tuple<Order, Boolean, string> order1 = await _order.removeItem(order);
            return ModelMapper.OrderToOrderView(order1);
        }
    }
}
