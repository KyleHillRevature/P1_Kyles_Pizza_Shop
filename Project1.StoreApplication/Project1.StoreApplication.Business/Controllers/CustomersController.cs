﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project1.StoreApplication.Domain.Interfaces.Repository;
using Project1.StoreApplication.Domain.Models;

namespace Project1.StoreApplication.Business.Controllers
{
    [Route("html/api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ILogger<CustomersController> _logger;
        private readonly IOrderRepository _orderRepository;

        public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger, IOrderRepository orderRepository)
        {
            _customerRepo = customerRepository;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        // GET: api/Customers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        //{
        //    return await _context.Customers.ToListAsync();
        //}

        // GET: api/Customers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customer>> GetCustomer(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return customer;
        //}

        //method is async because copy pasted the template
        // GET: api/Customers/firstAndLastName
        [HttpGet("firstName={firstName}&lastName={lastName}&userType={userType}")]
        public int ConfirmCustomerExists(string firstName, string lastName, string userType)
        {
            _logger.LogInformation($"{firstName} {lastName} attempted to sign in as a {userType} customer.");

            if (!Customer.isValidName(firstName, lastName)) return -2;  
            var customer = _customerRepo.FindCustomer(firstName, lastName);

            if (customer.Count == 1)
                if (userType.Equals("returning")) { 
                    _logger.LogInformation($"{firstName} {lastName} ({customer[0].Id}) signed in successfully"); 
                    List<Order> orders = _orderRepository.AllOrdersForCustomer(customer[0].Id, Order.cartOrderDate);
                    string pastOrdersString = orders.Count + " past orders, see below\n";
                    int orderIndex = 1;
                    foreach (Order order in orders)
                    { pastOrdersString += order.Id + " | " + order.OrderDate + " | " + order.Location.CityName + " | " + order.OrderItems.Count + " | " + order.TotalPrice ;
                        if (orderIndex != orders.Count) pastOrdersString += '\n'; orderIndex++; }
                    _logger.LogInformation(pastOrdersString);
                    


            return customer[0].Id; }
                //can't repeat names
                else return -1; 
            else
                //couldn't find you in the system
                if (userType.Equals("returning")) return -3; 
            else
            {
                _logger.LogInformation($"{firstName} {lastName} successfully made an account.");
                Customer customer1 = new Customer() { FirstName = firstName, LastName = lastName };
                return _customerRepo.AddCustomer(customer1);
            }
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCustomer(int id, Customer customer)
        //{
        //    if (id != customer.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(customer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public void PostCustomer(Customer customer)
        //{
        //    _customerRepo.AddCustomer(customer);
        //}
        [HttpGet("why/firstName={firstName}&lastName={lastName}")]
        public int PostCustomer(string firstName, string lastName)
        {
            Customer customer = new Customer() { FirstName = firstName, LastName = lastName };
            int value = _customerRepo.AddCustomer(customer);
            return value;
        }

        // DELETE: api/Customers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCustomer(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Customers.Remove(customer);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customers.Any(e => e.Id == id);
        //}
    }
}
