using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Repositories;
using ShopBackend.Security;
using ShopBackend.Utils;

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

        //Get api/customers
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
        {
            var customers = (await _customerRepository.GetAll()).Select(customer => customer.AsCustomerDto());
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound("The specified customers does not exist!");
        }

        //Get api/customers
        [HttpGet("{email}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<CustomerDto>> Get(string email)
        {
            var userEmail = _authService.GetEmailFromToken(User);
            var userRole = _authService.GetRoleFromToken(User);
            if (userRole != UserRoles.Admin.ToString() && userEmail != email)
            {
                return BadRequest("Access denied!");
            }

            var result = await _customerRepository.Get(email);
            if (result != default)
            {
                return Ok(result.AsCustomerDto());
            }

            return NotFound("The specified customer does not exist!");
        }


        //Post api/customers/register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Register([FromBody] CreateCustomerDto customerDto)
        {
            var isPasswordStrong = _passwordAuth.IsPasswordStrong(customerDto.Password);
            if (!isPasswordStrong)
            {
                return BadRequest("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!");
            }

            //CreateCustomerDto has Email as a required field parameter, meaning that Email it cannot be instantiated as null.
            if (customerDto.Email == null)
            {
                return BadRequest("Customer email is required to register the customer!");
            }

            var isEmailTaken = await _customerRepository.Get(customerDto.Email);
            if (isEmailTaken != default)
            {
                return BadRequest("This email is already in use!");
            }

            var customer = new Customer
            {
                Email = customerDto.Email,
                Password = _passwordAuth.GeneratePasswordHash(customerDto.Password),
                Role = UserRoles.Customer,
            };

            var result = await _customerRepository.Insert(customer);
            if (result != default && result > 0)
            {
                await _authService.AuthenticateUser(new LoginDto { Email = customerDto.Email, Password = customerDto.Password });
                var token = _authService.CreateToken();
                var msg = "Customer is inserted successfully!";
                return Ok(new { Token = token, Msg = msg });
            }

            return NotFound("Customer could not be registered!");
        }


        //Put api/customers
        [HttpPut]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<string>> Update([FromBody] UpdateCustomerDto customerDto)
        {
            var isPasswordStrong = _passwordAuth.IsPasswordStrong(customerDto.Password);
            if (!isPasswordStrong)
            {
                return BadRequest("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!");
            }

            //UpdateCustomerDto has Email as a required field parameter, meaning that Email it cannot be instantiated as null.
            if (customerDto.Email == null)
            {
                return BadRequest("Customer email is required to register the customer!");
            }

            var userEmail = _authService.GetEmailFromToken(User);
            var userRole = _authService.GetRoleFromToken(User);
            if (userRole != UserRoles.Admin.ToString() && userEmail != customerDto.Email)
            {
                return BadRequest("Access denied!");
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

            return NotFound("The specified customer does not exist!");
        }


        //Delete api/customers
        [HttpDelete("{email}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<string>> Delete(string email)
        {
            //This null check is not needed as it is against the scheme of the API endpoint. (http parameter email can never be null)
            if (email == null)
            {
                return BadRequest("Customer email is required to delete the customer!");
            }

            var userEmail = _authService.GetEmailFromToken(User);
            var userRole = _authService.GetRoleFromToken(User);
            if (userRole != UserRoles.Admin.ToString() && userEmail != email)
            {
                return BadRequest("Access denied!");
            }

            //Does this one make sense to if we are going to try and delete the customer anyway? (it would go to NotFound anyway in case result == default)
            var customerToDelete = await _customerRepository.Get(email);
            if (customerToDelete == default)
            {
                return NotFound("Customer does not exsist!");
            }

            var result = await _customerRepository.Delete(_authService.GetEmailFromToken(User));
            if (result != default)
            {
                return Ok("Customer has been deleted!");
            }

            return NotFound("Customer could not be deleted!");
        }
    }
}
