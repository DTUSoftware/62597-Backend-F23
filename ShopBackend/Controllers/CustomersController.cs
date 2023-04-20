using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        private readonly LinkGenerator _linkGenerator;

        public CustomersController(ICustomerRepository customerRepository, LinkGenerator linkGenerator)
        { 
           _customerRepository = customerRepository;
            _linkGenerator = linkGenerator;
        }


        //Get api/Customers
        [HttpGet]
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


        //Get api/Customers/5
        [HttpGet("{email}", Name="GetCustomerByEmail")]
        public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(string email)
        {
            var customer = await _customerRepository.Get(email);
            if(customer != default)
            {
                CustomerDto custDto = customer.AsCustomerDto();
                custDto.Links = (List<Link>)CreateLinksForCustomer(custDto.Email, "GET");
                return Ok(custDto);
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
                return Ok(CreateLinksForCustomer(customer.Email, "POST"));
            }

            return NotFound("Customer could not be registered!");
        }


        //Put api/Customers
        [HttpPut("{email}", Name = "UpdateCustomerByEmail")]
        public async Task<ActionResult<string>> UpdateCustomerByEmail([FromBody] CustomerDto customer)
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
            customerToUpdate.Address = customer.Address != null ? new List<Address>(customer.Address.Select(x => x.AsAddressModel())) : new List<Address>();
            customerToUpdate.Orders = customer.Orders != null ? new List<Order>(customer.Orders.Select(x => x.AsOrderModel())) : new List<Order>();

            var result = await _customerRepository.Update(customerToUpdate);
            if (result != default && result > 0)
            {
                return Ok(CreateLinksForCustomer(customerToUpdate.Email,"PUT"));
            }

            return NotFound("Customer could not be updated!");
        }


        //Delete api/Customers
        [HttpDelete("{email}",Name ="DeleteCustomerByEmail")]
        public async Task<ActionResult<string>> DeleteCustomerByEmail(string email)
        {
            var result = await _customerRepository.Delete(email);
            if (result != default && result > 0)
            {
                return Ok("Customer has been deleted!");
            }

            return NotFound("Customer could not be deleted!");
        }
        //Based on https://code-maze.com/hateoas-aspnet-core-web-api/
        private IEnumerable<Link> CreateLinksForCustomer(String email, String requestType)
        {
            switch (requestType)
            {
                case "GET":
                    var linksGet = new List<Link> {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteCustomerByEmail), values: new { email }),
            "delete_customer",
                        "DELETE"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateCustomerByEmail), values: new { email }),
        "update_customer",
        "PUT")
            };
                    return linksGet;
                case "PUT":
                    var linksPut = new List<Link>
                    {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetCustomerByEmail), values: new { email}),
                        "self",
                        "GET"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteCustomerByEmail), values: new { email }),
            "delete_customer",
            "DELETE")
            };
                    return linksPut;
                case "POST":
                    var linksPost = new List<Link> {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetCustomerByEmail), values: new { email}),
                        "self",
                        "GET"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteCustomerByEmail), values: new { email }),
                        "delete_customer",
                        "DELETE"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateCustomerByEmail), values: new { email }),
        "update_customer",
        "PUT")
            };
                    return linksPost;
                default:
                    throw new Exception("Invalid requestType");
            }
        }
    }
}
