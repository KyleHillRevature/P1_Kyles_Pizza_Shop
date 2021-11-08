using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Domain.Interfaces.Model
{
    public interface ILocationInventory
    {
        public Boolean itemIsAvailable(int locationId, int productId);
    }
}
