using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project1.StoreApplication.Domain.Models;

namespace Project1.StoreApplication.Domain.Interfaces.Repository
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
        Product GetProduct(int productId);
    }
}
