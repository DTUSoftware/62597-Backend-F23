using Microsoft.AspNetCore.Mvc;
using ShopBackend.Models;
using ShopBackend.Repositories;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        //Get api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            var customers = await _customerRepository.GetAll();

            if (customers != null && customers.Any())
            {
                return Ok(customers);
            }

            return NotFound("Customers do not existed!");
        }

        //Get api/Customers/example@gmail.com
        [HttpGet("{email}", Name = "GetCustomerByEmail")]
        public async Task<ActionResult<Customer>> Get(string email)
        {
            var customer = await _customerRepository.Get(email);

            if (customer != null)
            {
                return Ok(customer);
            }

            return NotFound("The specified customer does not exist!");
        }


        //Post api/Customers
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] Customer customer)
        {
            if (customer.Email == null)
            {
                return BadRequest("customer email cannot be null.");
            }

            var existed = await _customerRepository.Get(customer.Email);
            if (existed != null)
            {
                return BadRequest("Email is already in the databse.");
            }

            var result = await _customerRepository.Insert(customer);
            if (result != default && result > 0)
            {
                var location = Url.Action(nameof(Get), new { email = customer.Email }) ?? $"/{customer.Email}";
                return Created(location, customer.Email + " is inserted successfully.");
            }

            return NotFound("customer can not be registered");
        }


        //Put api/Customers
        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] Customer customer)
        {
            if (customer.Email == null)
            {
                return BadRequest("Email cannot be null");
            }

            var existed = await _customerRepository.Get(customer.Email);
            if (existed == null)
            {
                return NotFound("Csutomer cannot be found");
            }

            var result = await _customerRepository.Update(customer);
            if (result != default && result > 0)
            {
                return Ok("Customer is updated successfully.");
            }

            return NotFound("Customer cannot be updated");
        }


        //Delete api/Customers
        [HttpDelete]
        public async Task<ActionResult<string>> Delete(string customerEmail)
        {
            if (customerEmail == null)
            {
                return BadRequest("Email can not be null!");
            }

            var existed = await _customerRepository.Get(customerEmail);
            if (existed == null)
            {
                return NotFound("customer cannot be found.");
            }

            var result = await _customerRepository.Delete(customerEmail);
            if (result != default)
            {
                return Ok("Customer is deleted successfully.");
            }

            return NotFound("Customer cannot be deleted!");

        }

    }
}
