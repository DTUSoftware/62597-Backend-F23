using Microsoft.AspNetCore.Mvc;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Repositories;
using ShopBackend.Security;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuthService _authService;
        private readonly IPasswordAuth _passwordAuth;

        public CustomersController(ICustomerRepository customerRepository, IAuthService authService, IPasswordAuth passwordAuth)
        {
            _customerRepository = customerRepository;
            _authService = authService;
            _passwordAuth = passwordAuth;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.AuthenticateUser(loginDto);

            if (!result)
            {
                return Unauthorized("Incorrect username or password!");
            }

            var token = _authService.CreateToken();

            return Ok(token);
        }


        //Get api/Customers
        /*
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            var customers = (await _customerRepository.GetAll()).Select(customer => customer.AsCustomerDto());
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound("The specified customers does not exist!");
        }
        */

        //Get api/Customers/example@gmail.com
        [HttpGet]
        public async Task<ActionResult<Customer>> Get()
        {
            //Finds user email using token claims
            var result = await _customerRepository.Get(_authService.GetEmailFromToken(User));
            if (result != default)
            {
                return Ok(result.AsCustomerDto());
            }

            return NotFound("The specified customer does not exist!");
        }


        //Post api/Customers
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] CreateCustomerDto customerDto)
        {
            var isEmailTaken = await _customerRepository.Get(customerDto.Email);
            if (isEmailTaken != default)
            {
                return BadRequest("This email is already in use!");
            }

            var customer = new Customer
            {
                Email = customerDto.Email,
                Password = _passwordAuth.GeneratePasswordHash(customerDto.Password),
            };

            var result = await _customerRepository.Insert(customer);
            if (result != default && result > 0)
            {
                var token = _authService.CreateToken();
                var message = "Customer is inserted successfully!";
                return Ok(new { token, message });
            }

            return NotFound("Customer could not be registered!");
        }


        //Put api/Customers
        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] UpdateCustomerDto customerDto)
        {
            if (customerDto.Email == default)
            {
                return BadRequest("Customer email is required to update the customer!");
            }
            var customerToUpdate = await _customerRepository.Get(customerDto.Email);
            if (customerToUpdate == default)
            {
                return NotFound("Customer does not exsist!");
            }

            customerToUpdate.Email = customerDto.Email;
            customerToUpdate.Password = _passwordAuth.GeneratePasswordHash(customerDto.Password);
            customerToUpdate.Orders = customerDto.Orders != null ? new List<Order>(customerDto.Orders.Select(x => x.AsOrderModel())) : new List<Order>();

            var result = await _customerRepository.Update(customerToUpdate);
            if (result != default && result > 0)
            {
                return Ok("Customer has been updated!");
            }

            return NotFound("Customer could not be updated!");
        }


        //Delete api/Customers
        [HttpDelete]
        public async Task<ActionResult<string>> Delete()
        {
            var result = await _customerRepository.Get(_authService.GetEmailFromToken(User));
            if (result != default)
            {
                return Ok("Customer has been deleted!");
            }

            return NotFound("Customer could not be deleted!");
        }
    }
}
