using Project1.StoreApplication.Domain.Models;
using Project1.StoreApplication.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain
{
    public class ModelMapper
    {
        public static OrderView OrderToOrderView(Order order)
        {
            
            
            return new OrderView();
        }
    }
}
