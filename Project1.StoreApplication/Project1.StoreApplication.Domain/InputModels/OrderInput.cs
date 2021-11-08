using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain.InputModels
{
    public class OrderInput
    {
        public Guid OrderId { get; set; }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId {get;set;}

    }
}
