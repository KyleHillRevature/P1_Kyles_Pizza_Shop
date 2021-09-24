using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using Project1.StoreApplication.Domain.Models;

namespace Project1.StoreApplication.Business.Controllers
{
    [Route("html/api/[controller]")]
    [ApiController]
    public class LocationInventoriesController : ControllerBase
    {
        private readonly ILocationInventoryRepository _locationInventoryRepository;

        public LocationInventoriesController(ILocationInventoryRepository  locationInventoryRepository)
        {
            _locationInventoryRepository = locationInventoryRepository;
        }

        // GET: api/LocationInventories
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<LocationInventory>>> GetLocationInventories()
        //{
        //    return await _context.LocationInventories.ToListAsync();
        //}

        // GET: api/LocationInventories/5
        [HttpGet("{LocationId}")]
        public IEnumerable<LocationInventory> GetLocationInventory(int LocationId)
        {
            return _locationInventoryRepository.GetLocationInventory(LocationId);
        }

        // PUT: api/LocationInventories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLocationInventory(int id, LocationInventory locationInventory)
        //{
        //    if (id != locationInventory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(locationInventory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LocationInventoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/LocationInventories
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<LocationInventory>> PostLocationInventory(LocationInventory locationInventory)
        //{
        //    _context.LocationInventories.Add(locationInventory);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetLocationInventory", new { id = locationInventory.Id }, locationInventory);
        //}

        //// DELETE: api/LocationInventories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteLocationInventory(int id)
        //{
        //    var locationInventory = await _context.LocationInventories.FindAsync(id);
        //    if (locationInventory == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.LocationInventories.Remove(locationInventory);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool LocationInventoryExists(int id)
        //{
        //    return _context.LocationInventories.Any(e => e.Id == id);
        //}
    }
}
