using Microsoft.AspNetCore.Mvc;
using ShopBackend.Models;
using ShopBackend.Repositories;


namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get()
        {
            var addressList = await _addressRepository.GetAll();

            if (addressList != null && addressList.Count() > 0)
            {
                return Ok(addressList);
            }

            return NotFound("Customers do not existed!");
        }

        // GET api/Addresses
        [HttpGet(Name = "GetAddressByEmailAndType")]
        public async Task<ActionResult<Address>> Get([FromBody] Address address)
        {
            var result = await _addressRepository.Get(address.Email, address.Type);
            if (result != default)
            {
                return Ok(result);
            }

            return NotFound("The specified address does not exist!");
        }

        // POST api/Addresses
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] Address address)
        {
            var existed = await _addressRepository.Get(address.Email, address.Type);
            if (existed != null)
            {
                return BadRequest("Address is already in the databse.");
            }

            var result = await _addressRepository.Insert(address);
            if (result != default && result > 0)
            {
                return Created("shoppingApiServer", address.Email + " address is registered successfully.");
            }

            return NotFound("Address cannot be registered");
        }

        // PUT api/Addresses/{address}
        [HttpPut]
        public async Task<ActionResult<string>> Put([FromBody] Address address)
        {
            var existed = await _addressRepository.Get(address.Email, address.Type);
            if (existed == null)
            {
                return NotFound("Address cannot be found!");
            }

            var result = await _addressRepository.Update(address);
            if (result != default && result > 0)
            {
                return Ok("Address is updated.");
            }

            return NotFound("Address cannot be updated");
        }

        // DELETE api/<AddressController>/{address}
        [HttpDelete]
        public async Task<ActionResult<string>> Delete([FromBody] Address address)
        {

            var existed = await _addressRepository.Get(address.Email, address.Type);
            if (existed == null)
            {
                return NotFound("Address cannot be found!");
            }

            var result = await _addressRepository.Delete(address.Email, address.Type);
            if (result != default && result > 0)
            {
                return Ok("Address is Deleted.");
            }

            return NotFound("Address cannot be deleted");

        }
    }
}
