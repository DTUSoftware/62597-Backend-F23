using Microsoft.AspNetCore.Mvc;
using ShopBackend.Dtos;
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
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var orders = (await _orderRepository.GetAll()).Select(order => order.AsOrderDto());
            if (orders.Any())
            {
                return Ok(orders);
            }

            return NotFound("The specified orders does not exist!"); ;
        }

        // GET api/<OrdersController>/5
        [HttpGet("{orderId}", Name= "GetOrderById")]
        public async Task<ActionResult<OrderDto>> Get(Guid orderId)
        {
            var order = await _orderRepository.Get(orderId);
            if (order != default)
            {
                return Ok(order.AsOrderDto());
            }

            return NotFound("The specified order does not exist!");
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] OrderDto order)
        {
            var result = await _orderRepository.Insert(order.AsOrderModel());
            if (result != default && result > 0)
            {
                return Ok("order is inserted successfully:");
            }

            return NotFound("order can not be registered");
        }


        // PUT api/<OrdersController>/5
        [HttpPut]
        public async Task<ActionResult<string>> Put([FromBody] OrderDto order)   
        {
            var orderToUpdate = await _orderRepository.Get(order.Id);
            if (orderToUpdate == default)
            {
                return NotFound("Order does not exsist!");
            }

            orderToUpdate.OrderDate = DateTime.Now;
            orderToUpdate.OrderStatus = order.OrderStatus;
            orderToUpdate.CustomerId = order.CustomerId;
            orderToUpdate.OrderDetails = order.OrderDetails != null ? new List<OrderDetail>(order.OrderDetails.Select(x => x.AsOrderDetailModel())) : new List<OrderDetail>();


            var result = await _orderRepository.Update(orderToUpdate);
            if (result != default && result > 0)
            {
                return Ok("Order is updated!");
            }

            return NotFound("Order cannot be updated!");
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<string>> Delete(Guid orderId)
        {
            var result = await _orderRepository.Delete(orderId);
            if (result != default && result > 0)
            {
                return Ok("Order is deleted!");
            }
            return NotFound("Order could not be deleted!");
        }
    }
}
