using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project1.StoreApplication.Domain.ViewModels
{
    public class OrderView
    {
        public OrderView()
        {
            OrderItems = new HashSet<OrderItemView>();
        }
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal TotalPrice { get; set; }
        public Boolean actionSucceeded { get; set; }
        public virtual ICollection<OrderItemView> OrderItems { get; set; }
        public string message { get; set; }

        public static List<OrderItemView> GetOrderItemViews(Guid orderId) 
        {
            string query = @"select Name1, COUNT(*) as Quantity from Products 
                             join OrderItems on Products.Id = ProductId
                             where OrderId = @orderId
                             group by Name1";
            using (SqlConnection conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Kyles_Pizza_Shop;Trusted_Connection=True;"))
            {
                SqlCommand cmd = new SqlCommand(query,conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                List<OrderItemView> orderItemViews = new List<OrderItemView>();
                while (reader.Read())
                {
                    orderItemViews.Add(new OrderItemView { Name1 = reader.GetString(0), Quantity = reader.GetInt32(1) });
                }
                return orderItemViews;
                
            }
        }
    }
}
