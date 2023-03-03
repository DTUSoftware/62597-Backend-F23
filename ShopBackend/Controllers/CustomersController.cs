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
            var customers= await _customerRepository.GetAll();
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound("The specified customer does not exist!");
        }

        //Get api/Customers/5
        [HttpGet("{email}", Name="GetCustomerByEmail")]
        public async Task<ActionResult<Customer>> Get(string email)
        {
            var customer=await _customerRepository.Get(email);
            if(customer != default)
            {
                return Ok(customer);
            }else
            {
                return NotFound("customer not found");
            }
        }


        //Post api/Customers
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] Customer customer)
        {
            if(customer.Email == null)
            {
                return BadRequest("customer email is required to register the customer!");
            }

            var result = await _customerRepository.Insert(customer);
            if(result != default && result > 0)
            {
                return Ok("Customer is inserted successfully");
            }

            return NotFound("customer can not be registered");
        }


        //Put api/Customers
        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] Customer customer)
        {
            if (customer.Email == null)
            {
                return BadRequest("customer email is required to update the customer!");
            }

            var result =await _customerRepository.Update(customer);
            if (result != default && result > 0)
            {
                return Ok("customer is updated.");
            }

            return NotFound("customer cannot be updated");
        }


        //Delete api/Customers
        [HttpDelete]
        public async Task<ActionResult<string>> Delete([FromBody] Customer customer)
        {
            if (customer.Email == null)
            {
                return BadRequest("customer email is required to delete the customer!");
            }

            var result = await _customerRepository.Update(customer);
            if (result != default && result > 0)
            {
                return Ok("customer is Deleted.");
            }

            return NotFound("customer cannot be deleted");

        }
        
    }
}
