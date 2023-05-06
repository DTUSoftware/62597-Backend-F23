using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ShopBackend.Discoverabillity;
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
        //private readonly LinkGenerator _linkGenerator;

        public CustomersController(ICustomerRepository customerRepository, IAuthService authService, IPasswordAuth passwordAuth)
        {
            _customerRepository = customerRepository;
            _authService = authService;
            _passwordAuth = passwordAuth;
            // _linkGenerator = linkGenerator;
        }
        //Get api/customers
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
        {
            var customers = (await _customerRepository.GetAll()).Select(customer => customer.AsCustomerDto());
            if (customers.Any())
            {
                var customerList = customers.ToList();
                foreach (CustomerDto custDto in customerList)
                {
                    custDto.Links = (List<Link>)CreateLinksForCustomer(custDto.Email, "GET");
                }

                return Ok(customerList);
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
            if (userRole != UserRoles.Admin.ToString() && userEmail != email)// both admin and customer should be allowed!!!!!!!!
            {
                return BadRequest("Access denied!");
            }

            var result = await _customerRepository.Get(email);
            if (result != default)
            {
                CustomerDto custDto = result.AsCustomerDto();
                custDto.Links = (List<Link>)CreateLinksForCustomer(custDto.Email, "GET");
                return Ok(custDto);
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

            if (string.IsNullOrEmpty(customerDto.Email))
            {
                return BadRequest("Customer email is required to register the customer!");
            }

            var isEmailValid = _passwordAuth.IsEmailValid(customerDto.Email);
            if (!isEmailValid)
            {
                return BadRequest("Email format is not valid!");
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

            if (string.IsNullOrEmpty(customerDto.Email))
            {
                return BadRequest("Customer email is required to update the customer!");
            }

            var isEmailValid = _passwordAuth.IsEmailValid(customerDto.Email);
            if (!isEmailValid)
            {
                return BadRequest("Email format is not valid!");
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
                return Ok(CreateLinksForCustomer(customerToUpdate.Email, "PUT"));
            }

            return NotFound("The specified customer does not exist!");
        }


        //Delete api/customers
        [HttpDelete("{email}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<string>> Delete(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Customer email is required to delete the customer!");
            }

            var userEmail = _authService.GetEmailFromToken(User);
            var userRole = _authService.GetRoleFromToken(User);
            if (userRole != UserRoles.Admin.ToString() && userEmail != email)
            {
                return BadRequest("Access denied!");
            }

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
        //Based on https://code-maze.com/hateoas-aspnet-core-web-api/
        private IEnumerable<Link> CreateLinksForCustomer(string email, string requestType)
        {
            /*
            var GetUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Get), values: new { email })!;
            var UpdateUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Update), values: new { email })!;
            var DeleteUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Delete), values: new { email })!;
            */
            var GetUrl = HttpContext + nameof(Get) + new { email };
            var DeleteUrl = HttpContext + nameof(Delete) + new { email };
            var UpdateUrl = HttpContext + nameof(Update) + new { email };

            switch (requestType)
            {
                case "GET":
                    return new List<Link> {
                        new Link(DeleteUrl, "delete_customer", "DELETE"),
                        new Link(UpdateUrl, "update_customer",  "PUT")
                    };
                case "PUT":
                    return new List<Link>
                    {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href: DeleteUrl, "delete_customer", "DELETE")
                    };
                case "POST":
                    return new List<Link> {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href: DeleteUrl, "delete_customer", "DELETE"),
                        new Link(href: UpdateUrl, "update_customer",  "PUT")
                     };
                default:
                    throw new Exception("Invalid requestType");
            }
        }
    }
}

