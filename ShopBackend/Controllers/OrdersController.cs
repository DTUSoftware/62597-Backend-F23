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
            var orders= await _orderRepository.GetAll();
            if (orders.Any())
            {
                return Ok(orders);
            }

            return NotFound("customers does not existed!"); ;
        }

        // GET api/<OrdersController>/5
        [HttpGet("{orderId}", Name= "GetOrderById")]
        public async Task<ActionResult<Order>> Get(int orderId)
        {
            var order= await _orderRepository.Get(orderId);
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
            var result = await _orderRepository.Insert(order);
            if (result != default && result > 0)
            {
                return Ok("order is inserted successfully:");
            }

            return NotFound("order can not be registered");
        }


        // PUT api/<OrdersController>/5
        [HttpPut]
        public async Task<ActionResult<string>> Put([FromBody] Order order)   
        {
            var result = await _orderRepository.Update(order);
            if (result != default && result > 0)
            {
                return Ok("order is updated.");
            }

            return NotFound("order cannot be updated");
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<string>> Delete(int orderId)
        {
            
            var result= await _orderRepository.Delete(orderId);
            if (result != default && result > 0)
            {
                return Ok("order is deleted");
            }
            return NotFound("order cannot be deleted");
        }
    }
}
