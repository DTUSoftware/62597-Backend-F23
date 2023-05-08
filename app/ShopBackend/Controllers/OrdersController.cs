using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopBackend.Discoverabillity;
using ShopBackend.Dtos;
using ShopBackend.Repositories;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        //private readonly LinkGenerator _linkGenerator;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            //_linkGenerator = linkGenerator;
        }


        // GET: api/orders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = (await _orderRepository.GetAll()).Select(order => order.AsOrderDto());
            if (orders.Any())
            {
                var orderList = orders.ToList();
                foreach (OrderDto orderDto in orderList)
                {
                    orderDto.Links = (List<Link>)CreateLinksForOrder(orderDto.Id, "GET");
                }
                return Ok(orderList);
            }

            return NotFound("The specified orders does not exist!"); ;
        }


        // GET api/orders/{orderId}
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderId)
        {
            var order = await _orderRepository.Get(orderId);
            if (order != default)
            {
                OrderDto orderDto = order.AsOrderDto();
                orderDto.Links = (List<Link>)CreateLinksForOrder(orderId, "GET");
                return Ok(orderDto);
            }

            return NotFound("The specified order does not exist!");
        }


        // POST api/orders
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("FrontendPolicy")]
        public async Task<ActionResult<string>> CreateOrder([FromBody] CreateOrderDto order)
        {
            if (order.OrderDetails.Count == 0)
            {
                return BadRequest("ProductDetails is required to register the order!");
            }

            var result = await _orderRepository.Insert(order.CreateAsOrderModel());
            if (result != default && result > 0)
            {
                return Created("CreateOrder","Order is inserted successfully!");
            }

            return NotFound("Order could not be registered!");
        }


        // PUT api/orders
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> UpdateOrder([FromBody] UpdateOrderDto order)   
        {
            var orderToUpdate = await _orderRepository.Get(order.Id);
            if (orderToUpdate == default)
            {
                return NotFound("Order does not exsist!");
            }

            orderToUpdate.OrderDate = DateTime.Now;
            orderToUpdate.OrderStatus = order.OrderStatus;
            orderToUpdate.CheckMarketing = order.CheckMarketing;
            orderToUpdate.SubmitComment = order.SubmitComment;

            var result = await _orderRepository.Update(orderToUpdate);
            if (result != default && result > 0)
            {
                return Ok("Order has been updated!");
            }

            return NotFound("Order could not be updated!");
        }


        // DELETE api/orders/{orderId}
        [HttpDelete("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> DeleteOrder(Guid orderId)
        {
            var result = await _orderRepository.Delete(orderId);

            if (result != default && result > 0)
            {
                return Ok("Order has been deleted!");
            }
            return NotFound("Order could not be deleted!");
        }

        //Based on https://code-maze.com/hateoas-aspnet-core-web-api/
        private IEnumerable<Link> CreateLinksForOrder(Guid orderId, string requestType)
        {
            /*
             var GetUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Get), values: new { orderId})!;
             var DeleteUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Delete), values: new { orderId })!;
             var UpdateUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(Update), values: new { orderId })!;
            */
            var GetUrl = "https://dtu-api.herogamers.dev/api" +"/orders/" + orderId;
            var DeleteUrl = "https://dtu-api.herogamers.dev/api" + "/orders/" + orderId;
            var UpdateUrl = "https://dtu-api.herogamers.dev/api" + "/orders";

            switch (requestType)
            {
                case "GET":
                    return new List<Link> {
                        new Link(href: GetUrl, "delete_order", "DELETE"),
                        new Link(href: UpdateUrl , "update_order", "PUT")
                    };
                case "PUT":
                    return new List<Link> {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href:DeleteUrl, "delete_order", "DELETE")
                    };
                case "POST":
                    return new List<Link> {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href:DeleteUrl, "delete_order", "DELETE"),
                        new Link(href: UpdateUrl , "update_order", "PUT")
                    };
                default:
                    throw new Exception("Invalid requestType");
            }
        }
    }
}
