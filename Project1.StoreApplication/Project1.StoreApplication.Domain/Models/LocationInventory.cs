using Project1.StoreApplication.Domain.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

#nullable disable

namespace Project1.StoreApplication.Domain.Models
{
    public partial class LocationInventory : ILocationInventory
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }

        public Boolean itemIsAvailable(int locationId, int productId)
        {
            string query = "select Stock from LocationInventory where LocationId = @locId and ProductId = @prodId";
            using (SqlConnection conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Kyles_Pizza_Shop;Trusted_Connection=True;"))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@locId", locationId);
                cmd.Parameters.AddWithValue("@prodId", productId);
                conn.Open();
                int stock = (int)cmd.ExecuteScalar();
                if (stock > 0) return true;
                else return false;
            }
        }
    }
}
