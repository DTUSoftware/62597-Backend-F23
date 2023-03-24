using Microsoft.AspNetCore.Mvc;
using ShopBackend.Dtos;
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
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
        {
            var customers = (await _customerRepository.GetAll()).Select(customer => customer.AsCustomerDto());
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound("The specified customers does not exist!");
        }


        //Get api/Customers/5
        [HttpGet("{email}", Name="GetCustomerByEmail")]
        public async Task<ActionResult<CustomerDto>> Get(string email)
        {
            var customer = await _customerRepository.Get(email);
            if(customer != default)
            {
                return Ok(customer.AsCustomerDto());
            }
            
            return NotFound("Customer not found!");
        }


        //Post api/Customers
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateCustomerDto customer)
        {
            if (customer.Email == null)
            {
                return BadRequest("Customer email is required to register the customer!");
            }
            var isEmailTaken = await _customerRepository.Get(customer.Email);
            if (isEmailTaken != default)
            {
                return BadRequest("This email is already in use!");
            }

            var result = await _customerRepository.Insert(customer.CreateAsCustomerModel());
            if(result != default && result > 0)
            {
                return Ok("Customer is inserted successfully!");
            }

            return NotFound("Customer could not be registered!");
        }


        //Put api/Customers
        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] CustomerDto customer)
        {
            if (customer.Email == null)
            {
                return BadRequest("Customer email is required to update the customer!");
            }
            var customerToUpdate = await _customerRepository.Get(customer.Email);
            if (customerToUpdate == default)
            {
                return NotFound("Customer does not exsist!");
            }

            customerToUpdate.Email = customer.Email;
            customerToUpdate.FirstName = customer.FirstName;
            customerToUpdate.LastName = customer.LastName;
            //customerToUpdate.Password = customer.Password;
            customerToUpdate.Phone = customer.Phone;
            customerToUpdate.Orders = customer.Orders != null ? new List<Order>(customer.Orders.Select(x => x.AsOrderModel())) : new List<Order>();

            var result = await _customerRepository.Update(customerToUpdate);
            if (result != default && result > 0)
            {
                return Ok("Customer has been updated!");
            }

            return NotFound("Customer could not be updated!");
        }


        //Delete api/Customers
        [HttpDelete("{email}")]
        public async Task<ActionResult<string>> Delete(string email)
        {
            var result = await _customerRepository.Delete(email);
            if (result != default && result > 0)
            {
                return Ok("Customer has been deleted!");
            }

            return NotFound("Customer could not be deleted!");
        }
    }
}
