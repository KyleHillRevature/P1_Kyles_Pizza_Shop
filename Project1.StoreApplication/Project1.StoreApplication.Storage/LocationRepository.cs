using Project1.StoreApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Domain.Interfaces.Repository;

namespace Project1.StoreApplication.Storage
{
    public class LocationRepository : ILocationRepository
    {
        private readonly Kyles_Pizza_ShopContext _context;

        public LocationRepository(Kyles_Pizza_ShopContext context)
        { _context = context; }

        public List<Location> GetLocations()
        {
            return _context.Locations.FromSqlRaw<Location>("select * from Locations").ToList();
        }
    }
}
