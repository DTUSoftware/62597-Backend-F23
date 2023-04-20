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
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly LinkGenerator _linkGenerator;

        public OrdersController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository,LinkGenerator linkGenerator)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _linkGenerator = linkGenerator;
        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
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


        // GET api/<OrdersController>/5
        [HttpGet("{orderId}", Name= "GetOrderById")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
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


        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateUpdateOrderDto order)
        {
            var result = await _orderRepository.Insert(order.CreateAsOrderModel());
            if (result != default && result > 0)
            {
            
                return Ok("Order is inserted successfully!");
            }

            return NotFound("Order could not be registered!");
        }


        // PUT api/<OrdersController>/5
        [HttpPut("{orderId}",Name = "UpdateOrderById")]
        public async Task<ActionResult<string>> UpdateOrderById(Guid orderId, [FromBody] CreateUpdateOrderDto order)   
        {
            var orderToUpdate = await _orderRepository.Get(orderId);
            if (orderToUpdate == default)
            {
                return NotFound("Order does not exsist!");
            }

            orderToUpdate.OrderDate = order.OrderDate;
            orderToUpdate.OrderStatus = order.OrderStatus;
            orderToUpdate.CustomerEmail = order.CustomerEmail;


            var result = await _orderRepository.Update(orderToUpdate);
            if (result != default && result > 0)
            {
                return Ok("Order has been updated!");
            }

            return NotFound("Order could not be updated!");
        }


        // DELETE api/<OrdersController>/5
        [HttpDelete("{orderId}",Name ="DeleteOrderById")]
        public async Task<ActionResult<string>> DeleteOrderById(Guid orderId)
        {

            var result = await _orderRepository.Delete(orderId);

            if (result != default && result > 0)
            {
                return Ok("Order has been deleted!");
            }
            return NotFound("Order could not be deleted!");
        }

        //Based on https://code-maze.com/hateoas-aspnet-core-web-api/
        private IEnumerable<Link> CreateLinksForOrder(Guid orderId, String requestType)
        {
            switch (requestType)
            {
                case "GET":
                    var linksGet = new List<Link> {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteOrderById), values: new { orderId }),
            "delete_order",
            "DELETE"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateOrderById), values: new { orderId }),
        "update_order",
        "PUT")
            };
                    return linksGet;
                case "PUT":
                    var linksPut = new List<Link>
                        {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetOrderById), values: new { orderId}),
            "self",
            "GET"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteOrderById), values: new { orderId }),
            "delete_order",
            "DELETE")
            };
                    return linksPut;
                case "POST":
                    var linksPost = new List<Link> {
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetOrderById), values: new { orderId}),
            "self",
            "GET"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteOrderById), values: new { orderId }),
            "delete_order",
            "DELETE"),
        new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateOrderById), values: new { orderId }),
        "update_order",
        "PUT")
            };
                    return linksPost;
                default:
                    throw new Exception("Invalid requestType");
            }
        }
    }
}
