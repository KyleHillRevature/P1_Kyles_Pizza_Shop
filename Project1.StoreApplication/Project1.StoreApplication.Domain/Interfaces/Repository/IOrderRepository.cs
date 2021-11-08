using Project1.StoreApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> AllOrdersForCustomerAsync(int customerId, string cartMarkerDate);
        List<Order> AllOrdersForLocation(int locationId, string cartMarkerDate);
        void SubmitOrder(Guid orderId);
        Task<Order> GetParticularOrder(Guid orderId);
        Task<Order> GetParticularOrderNoTrack(Guid orderId);
        void UpdateTotalPrice(Guid orderId, decimal totalPrice);
        void AddNewOrder(Guid orderId, string cartMarkerDate, int customerId, int locationId);
        void Delete(Guid orderId);
        void Delete(string cartMarkerDate);


    }
}
