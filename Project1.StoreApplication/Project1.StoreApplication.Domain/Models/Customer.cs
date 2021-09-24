using System;
using System.Collections.Generic;


#nullable disable

namespace Project1.StoreApplication.Domain.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public static Boolean isValidName(string firstName, string lastName)
        {
            if (firstName.Length > 50 || lastName.Length > 50) return false;
            else return true;
        }
    }
}
