using Microsoft.AspNetCore.Mvc;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Repositories;


namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
        {
            var addressList = await _addressRepository.GetAll();

            if (addressList != null && addressList.Any())
            {
                return Ok(addressList);
            }

            return NotFound("The specified addresses does not exist!");
        }

        // GET api/Addresses
        [HttpGet("{addressId}")]
        public async Task<ActionResult<AddressDto>> Get(Guid addressId)
        {
            var result = await _addressRepository.Get(addressId);
            if (result != default)
            {
                return Ok(result.AsAddressDto());
            }

            return NotFound("The specified address does not exist!");
        }

        // POST api/Addresses
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateAddressDto address)
        {
            var result = await _addressRepository.Insert(address.CreateAsAddressModel());
            if (result != default && result > 0)
            {
                return Created("shoppingApiServer", address.Email + " address is registered successfully.");
            }

            return NotFound("Address could not be registered!");
        }

        // PUT api/Addresses/{address}
        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] Address address)
        {
            var existed = await _addressRepository.Get(address.Id);
            if (existed == null)
            {
                return NotFound("The specified address could not be found!");
            }

            var result = await _addressRepository.Update(address);
            if (result != default && result > 0)
            {
                return Ok("Address is updated.");
            }

            return NotFound("Address cannot be updated");
        }

        // DELETE api/<AddressController>/{address}
        [HttpDelete("{addressId}")]
        public async Task<ActionResult<string>> Delete(Guid addressId)
        {

            var existed = await _addressRepository.Get(addressId);
            if (existed == null)
            {
                return NotFound("The specified address could not be found!");
            }

            var result = await _addressRepository.Delete(addressId);
            if (result != default && result > 0)
            {
                return Ok("The specified address has been deleted!");
            }

            return NotFound("The specified address could not be deleted!");
        }
    }
}
