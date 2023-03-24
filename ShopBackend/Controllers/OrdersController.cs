using Microsoft.AspNetCore.Mvc;
using ShopBackend.Models;
using ShopBackend.Repositories;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            var orders = await _orderRepository.GetAll();
            if (orders != null && orders.Count() > 0)
            {
                return Ok(orders);
            }

            return NotFound("Orders do not existed!"); ;
        }

        // GET api/<OrdersController>/5
        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<ActionResult<Order>> Get(int orderId)
        {
            var order = await _orderRepository.Get(orderId);
            if (order != null)
            {
                return Ok(order);
            }

            return NotFound("The specified order does not exist!");
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] Order order)
        {

            var existed = await _orderRepository.Get(order.Id);
            if (existed != null)
            {
                return BadRequest("OrderId is already in the databse.");
            }

            var result = await _orderRepository.Insert(order);
            if (result != default && result > 0)
            {
                var location = Url.Action(nameof(Get), new { id = order.Id }) ?? $"/{order.Id}";
                return Created(location, order.Id + " is registered successfully.");
            }

            return NotFound("Order cannot be registered");
        }


        // PUT api/<OrdersController>/5
        [HttpPut]
        public async Task<ActionResult<string>> Put([FromBody] Order order)
        {

            var existed = await _orderRepository.Get(order.Id);
            if (existed == null)
            {
                return NotFound("Order cannot be found");
            }

            var result = await _orderRepository.Update(order);
            if (result != default && result > 0)
            {
                return Ok("Order is updated successfully.");
            }

            return NotFound("Order cannot be updated!");
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<string>> Delete(int orderId)
        {
            var existed = await _orderRepository.Get(orderId);
            if (existed == null)
            {
                return NotFound("Order cannot be found");
            }

            var result = await _orderRepository.Delete(orderId);
            if (result != default && result > 0)
            {
                return Ok("Order is deleted successfully.");
            }

            return NotFound("Order cannot be deleted!");
        }
    }
}
